using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
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
            _configDir = AppDomain.CurrentDomain.BaseDirectory;
            _configFile = Path.Combine(_configDir, "config.json");

            // Wire up events
            this.Load += MainForm_Load;
            addFolderButton.Click += AddFolderButton_Click;
            removeFolderButton.Click += RemoveFolderButton_Click;
            applyConfigButton.Click += SaveConfigButton_Click;
            detectDrivesButton.Click += DetectDrivesButton_Click;
            backupButton.Click += BackupButton_Click;
            installServiceButton.Click += InstallServiceButton_Click;
            backupTimePicker.ValueChanged += BackupTimePicker_ValueChanged;
        }

        private void InstallServiceButton_Click(object? sender, EventArgs e)
        {
            string serviceName = "ResguardoAppService";
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            try
            {
                if (IsServiceInstalled(serviceName))
                {
                    // Uninstall the service
                    ExecuteCommand($"sc delete {serviceName}");
                    MessageBox.Show("Servicio desinstalado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Install the service
                    ExecuteCommand($"sc create {serviceName} binPath= \"{exePath}\" start= auto");
                    MessageBox.Show("Servicio instalado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al instalar/desinstalar el servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(processInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start process.");
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Error executing command: {command}\nOutput: {output}\nError: {error}");
            }
        }

        private bool IsServiceInstalled(string serviceName)
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == serviceName);
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private AppConfig _currentConfig;
        private bool configChanged;

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

                if (!string.IsNullOrEmpty(_currentConfig?.BackupTime))
                {
                    if (DateTime.TryParse(_currentConfig.BackupTime, out var time))
                    {
                        backupTimePicker.Value = time;
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
                _currentConfig ??= new AppConfig();
                _currentConfig.BackupFolders = backupFoldersListBox.Items.Cast<string>().ToList();
                _currentConfig.BackupTime = backupTimePicker.Value.ToString("HH:mm");

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

        private void MarkConfigChanged()
        {
            configChanged = true;
            applyConfigButton.Enabled = true;
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
                            MarkConfigChanged();
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
                MarkConfigChanged();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una carpeta para quitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ApplyConfigButton_Click(object? sender, EventArgs e)
        {
            SaveConfiguration();
            configChanged = false;
            applyConfigButton.Enabled = false;
        }

        private void BackupTimePicker_ValueChanged(object? sender, EventArgs e)
        {
            MarkConfigChanged();
        }

        private void DetectDrivesButton_Click(object? sender, EventArgs e)
        {
            portableDisksListBox.Items.Clear();
            try
            {
                var drives = DriveInfo.GetDrives()
                    .Where(d => d.DriveType == DriveType.Removable && d.IsReady);

                foreach (var drive in drives)
                {
                    var label = string.IsNullOrWhiteSpace(drive.VolumeLabel)
                        ? "Sin etiqueta"
                        : drive.VolumeLabel;
                    portableDisksListBox.Items.Add($"{drive.Name} ({label})");
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
            if (_currentConfig == null)
            {
                MessageBox.Show("La configuración no ha sido cargada o guardada.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asegurarse de que el disco de respaldo está seleccionado y es el correcto
            if (portableDisksListBox.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un disco de respaldo antes de continuar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedDriveItem = portableDisksListBox.SelectedItem.ToString();
            var driveLetter = selectedDriveItem.Split(' ')[0].Replace("\\", "").ToUpper();
            var actual = DiscoUtil.ObtenerInfoDeDisco(driveLetter);

            if (_currentConfig.DiscoRespaldo == null)
            {
                _currentConfig.DiscoRespaldo = actual;
                SaveConfiguration();
                MessageBox.Show("Disco de respaldo registrado. Ahora puede realizar el respaldo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (_currentConfig.DiscoRespaldo.VolumeSerialNumber != actual.VolumeSerialNumber ||
                     _currentConfig.DiscoRespaldo.PNPDeviceID != actual.PNPDeviceID)
            {
                MessageBox.Show("El disco seleccionado no coincide con el disco de respaldo registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BackupService.PerformBackup(_currentConfig);
            MessageBox.Show("Respaldo completado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void installServiceButton_Click_1(object sender, EventArgs e)
        {

        }
    }
}
