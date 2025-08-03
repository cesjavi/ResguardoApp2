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

        private AppConfig _currentConfig;

private void LoadConfiguration()
{
    if (!File.Exists(_configFile))
        return;

    try
    {
        var json = File.ReadAllText(_configFile);
        _currentConfig = JsonSerializer.Deserialize<AppConfig>(json);

        if (_currentConfig?.BackupFolders != null)
        {
            backupFoldersListBox.Items.Clear();
            foreach (var folder in _currentConfig.BackupFolders)
                backupFoldersListBox.Items.Add(folder);
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
        _currentConfig ??= new AppConfig();
        _currentConfig.BackupFolders = backupFoldersListBox.Items.Cast<string>().ToList();

        Directory.CreateDirectory(_configDir);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_currentConfig, options);

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
    var sourceFolders = backupFoldersListBox.Items.Cast<string>().ToList();
    if (!sourceFolders.Any())
    {
        MessageBox.Show("No hay carpetas en la lista para respaldar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    if (portableDisksListBox.SelectedItem == null)
    {
        MessageBox.Show("Seleccione un disco de respaldo antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    var selectedDriveItem = portableDisksListBox.SelectedItem.ToString();
    var driveLetter = selectedDriveItem.Split(' ')[0].Replace("\\", "").ToUpper();

    var actual = DiscoUtil.ObtenerInfoDeDisco(driveLetter);

    if (_currentConfig?.DiscoRespaldo == null)
    {
        // Guardar por primera vez
        _currentConfig.DiscoRespaldo = actual;
        SaveConfiguration();
        MessageBox.Show("Disco de respaldo registrado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    else if (_currentConfig.DiscoRespaldo.VolumeSerialNumber != actual.VolumeSerialNumber ||
             _currentConfig.DiscoRespaldo.PNPDeviceID != actual.PNPDeviceID)
    {
        MessageBox.Show("El disco seleccionado no coincide con el disco de respaldo registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    var destinationRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");
    // Ejecutar la sincronización para cada carpeta origen
    foreach (var sourceFolder in sourceFolders)
    {
        var sourceDir = new DirectoryInfo(sourceFolder);
        var destDir = new DirectoryInfo(Path.Combine(destinationRoot, sourceDir.Name));
        if (!destDir.Exists)
            destDir.Create();
        SynchronizeDirectory(sourceDir, destDir);
    }

    MessageBox.Show("Respaldo completado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
} // <-- Aquí termina PerformBackup



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
