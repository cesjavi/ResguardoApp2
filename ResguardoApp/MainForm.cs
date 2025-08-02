using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace ResguardoApp
{
    public partial class MainForm : Form
    {
        private readonly string _configDir;
        private readonly string _configFile;

        public MainForm()
        {
            InitializeComponent();
            // Define configuration path
            _configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ResguardoApp");
            _configFile = Path.Combine(_configDir, "config.json");

            // Wire up events
            this.Load += MainForm_Load;
            addFolderButton.Click += AddFolderButton_Click;
            removeFolderButton.Click += RemoveFolderButton_Click;
            saveConfigButton.Click += SaveConfigButton_Click;
            detectDrivesButton.Click += DetectDrivesButton_Click;
            backupButton.Click += BackupButton_Click;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            if (!File.Exists(_configFile))
            {
                return; // No config file yet, do nothing.
            }

            try
            {
                var json = File.ReadAllText(_configFile);
                var config = JsonSerializer.Deserialize<AppConfig>(json);
                if (config?.BackupFolders != null)
                {
                    backupFoldersListBox.Items.Clear();
                    foreach (var folder in config.BackupFolders)
                    {
                        backupFoldersListBox.Items.Add(folder);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la configuración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveConfiguration()
        {
            try
            {
                var config = new AppConfig
                {
                    BackupFolders = backupFoldersListBox.Items.Cast<string>().ToList()
                };

                // Ensure the directory exists
                Directory.CreateDirectory(_configDir);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);

                File.WriteAllText(_configFile, json);

                MessageBox.Show("Configuración guardada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddFolderButton_Click(object? sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Seleccione una carpeta para respaldar";
                dialog.UseDescriptionForTitle = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    {
                        if (!backupFoldersListBox.Items.Contains(dialog.SelectedPath))
                        {
                            backupFoldersListBox.Items.Add(dialog.SelectedPath);
                        }
                        else
                        {
                            MessageBox.Show("Esa carpeta ya está en la lista.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void RemoveFolderButton_Click(object? sender, EventArgs e)
        {
            if (backupFoldersListBox.SelectedItem != null)
            {
                backupFoldersListBox.Items.Remove(backupFoldersListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una carpeta para quitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveConfigButton_Click(object? sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void DetectDrivesButton_Click(object? sender, EventArgs e)
        {
            portableDisksListBox.Items.Clear();
            try
            {
                var drives = DriveInfo.GetDrives();
                                      //.Where(d => d.IsReady);

                foreach (var drive in drives)
                {
                    portableDisksListBox.Items.Add($"{drive.Name} ({drive.VolumeLabel})");
                }

                if (!portableDisksListBox.Items.Cast<string>().Any())
                {
                    portableDisksListBox.Items.Add("No se encontraron discos extraíbles.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al detectar discos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupButton_Click(object? sender, EventArgs e)
        {
            PerformBackup();
        }

        private void PerformBackup()
        {
            // 1. Get source folders
            var sourceFolders = backupFoldersListBox.Items.Cast<string>().ToList();
            if (!sourceFolders.Any())
            {
                MessageBox.Show("No hay carpetas en la lista para respaldar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Get destination drive
            if (portableDisksListBox.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un disco de destino.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract drive letter (e.g., "D:\ (USB Drive)" -> "D:\")
            var selectedDriveItem = portableDisksListBox.SelectedItem.ToString();
            var driveName = selectedDriveItem.Split(' ')[0];
            var destinationRoot = Path.Combine(driveName, "ResguardoApp");

            try
            {
                // 3. Create root backup directory
                Directory.CreateDirectory(destinationRoot);

                // 4. Loop and synchronize
                foreach (var sourceFolder in sourceFolders)
                {
                    var sourceDirInfo = new DirectoryInfo(sourceFolder);
                    var destinationSubFolder = Path.Combine(destinationRoot, sourceDirInfo.Name);
                    Directory.CreateDirectory(destinationSubFolder);

                    SynchronizeDirectory(new DirectoryInfo(sourceFolder), new DirectoryInfo(destinationSubFolder));
                }

                MessageBox.Show("El proceso de resguardo ha comenzado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error durante el resguardo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SynchronizeDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            // Sync all files
            foreach (var sourceFile in source.GetFiles())
            {
                var destinationFile = new FileInfo(Path.Combine(destination.FullName, sourceFile.Name));

                if (!destinationFile.Exists || sourceFile.LastWriteTime > destinationFile.LastWriteTime)
                {
                    sourceFile.CopyTo(destinationFile.FullName, true);
                }
            }

            // Sync all subdirectories
            foreach (var sourceSubDir in source.GetDirectories())
            {
                var destinationSubDir = new DirectoryInfo(Path.Combine(destination.FullName, sourceSubDir.Name));
                if (!destinationSubDir.Exists)
                {
                    destinationSubDir.Create();
                }
                SynchronizeDirectory(sourceSubDir, destinationSubDir);
            }
        }
    }
}
