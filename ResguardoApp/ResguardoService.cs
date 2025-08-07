using System;
using System.IO;
using System.ServiceProcess;
using System.Text.Json;
using System.Timers;

namespace ResguardoApp
{
    public class ResguardoService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private AppConfig _config;
        private readonly string _configFile;
        private readonly string _logFile;
        private DateTime? _lastBackupDate;

        public ResguardoService()
        {
            ServiceName = "ResguardoAppService";
            _configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            _logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_resguardo_service.txt");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LoadConfiguration();

                if (_timer == null)
                {
                    _timer = new System.Timers.Timer();
                    _timer.Elapsed += new ElapsedEventHandler(OnTimer);
                }

                _timer.Interval = 60000; // 1 minuto
                _timer.Start();
            }
            catch (Exception ex)
            {
                File.AppendAllText(_logFile,
                    
                    DateTime.Now + Environment.NewLine +
                    ex.ToString() + Environment.NewLine +
                    (ex.InnerException?.ToString() ?? "") + Environment.NewLine);
                throw; // Dejá que el servicio falle igual para que el Event Viewer lo registre
            }
        }



        protected override void OnStop()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            LoadConfiguration();
            if (_config == null || string.IsNullOrEmpty(_config.BackupTime))
            {
                return;
            }

            if (!TimeSpan.TryParse(_config.BackupTime, out var backupTime))
            {
                return;
            }

            var now = DateTime.Now;
            var scheduled = now.Date.Add(backupTime);

            if (now >= scheduled && (_lastBackupDate == null || _lastBackupDate.Value.Date < now.Date))
            {
                BackupService.PerformBackup(_config);
                _lastBackupDate = now.Date;
            }
        }

        private void LoadConfiguration()
        {
            if (!File.Exists(_configFile))
                return;

            try
            {
                var json = File.ReadAllText(_configFile);
                _config = JsonSerializer.Deserialize<AppConfig>(json);
            }
            catch
            {
                // Log error
            }
        }
    }
}
