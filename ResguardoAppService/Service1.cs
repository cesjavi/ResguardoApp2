using System;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;
using System.Timers;
using ResguardoApp;
using System.Text.Json;

namespace ResguardoAppService
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private AppConfig _config;
        private DateTime? _lastBackupDate;

        public Service1()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("ResguardoAppService"))
            {
                EventLog.CreateEventSource("ResguardoAppService", "Application");
            }
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("ResguardoAppService", "Servicio iniciado.", EventLogEntryType.Information);
            // Lógica de respaldo programado

            // Podés usar un Timer, leer tu archivo de configuración, etc.

            _timer = new Timer(60000);
            _timer.Elapsed += OnTimer;

            LoadConfiguration();

            _timer.Start();

        }

        protected override void OnStop()
        {
                EventLog.WriteEntry("ResguardoAppService", "Servicio detenido.", EventLogEntryType.Information);

            // Lógica de limpieza/parada

            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
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
            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

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

