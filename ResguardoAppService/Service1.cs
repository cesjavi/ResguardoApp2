using System;
using System.IO;
using System.ServiceProcess;
using System.Text.Json;
using System.Timers;
using ResguardoApp;

namespace ResguardoAppService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private AppConfig _config;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(60000);
            _timer.Elapsed += OnTimer;

            LoadConfiguration();

            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            LoadConfiguration();
            if (_config != null && DateTime.Now.ToString("HH:mm") == _config.BackupTime)
            {
                BackupService.PerformBackup(_config);
            }
        }

        private void LoadConfiguration()
        {
            var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ResguardoApp");
            var configFile = Path.Combine(configDir, "config.json");

            if (!File.Exists(configFile))
            {
                _config = null;
                return;
            }

            try
            {
                var json = File.ReadAllText(configFile);
                _config = JsonSerializer.Deserialize<AppConfig>(json);
            }
            catch
            {
                _config = null;
            }
        }
    }
}

