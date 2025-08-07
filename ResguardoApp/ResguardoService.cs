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

        public ResguardoService()
        {
            ServiceName = "ResguardoAppService";
            var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ResguardoApp");
            _configFile = Path.Combine(configDir, "config.json");
        }

        protected override void OnStart(string[] args)
        {
            LoadConfiguration();

            if (_timer == null)
            {
                _timer = new System.Timers.Timer();
                _timer.Elapsed += new ElapsedEventHandler(OnTimer);
            }

            _timer.Interval = 60000; // 1 minute
            _timer.Start();
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

            if (DateTime.Now.ToString("HH:mm") == _config.BackupTime)
            {
                BackupService.PerformBackup(_config);
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
