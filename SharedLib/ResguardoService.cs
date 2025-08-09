using System;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;
using System.Text.Json;
using System.Timers;
using SharedLib;

namespace SharedLib
{
    public class ResguardoService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private AppConfig _config;
        private readonly string _configFile;
        private readonly string _logFile;
        private DateTime? _lastBackupDate;
        private DateTime? _lastSkipLogDate;
        private string? _lastLogMessage;
        private DateTime _lastLogTime;

        public ResguardoService()
        {
            ServiceName = "ResguardoAppService";
            _configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var logDir = Environment.GetEnvironmentVariable("RESGUARDO_LOG_PATH");
            if (string.IsNullOrWhiteSpace(logDir))
            {
                logDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "ResguardoApp");
            }
            _logFile = Path.Combine(logDir, "error_resguardo_service.txt");
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
                if (_config?.ForceBackupOnStart == true)
                {
                    BackupService.PerformBackup(_config);
                    _lastBackupDate = DateTime.Now.Date;
                }
                _timer.Start();
            }
            catch (Exception ex)
            {
                SafeLog(
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
            var config = _config;
            var backupTimeString = config?.BackupTime;
            if (string.IsNullOrEmpty(backupTimeString))
            {
                return;
            }

            if (!TimeSpan.TryParse(backupTimeString, out var backupTime))
            {
                return;
            }

            var now = DateTime.Now;
            var scheduled = now.Date.Add(backupTime);

            if (now.DayOfWeek == config.BackupDay &&
                now >= scheduled &&
                (_lastBackupDate == null || _lastBackupDate.Value.Date < now.Date))
            {
                try
                {
                    if (config != null)
                    {
                        BackupService.PerformBackup(config);
                    }
                    _lastBackupDate = now.Date;
                }
                catch (Exception ex)
                {
                    SafeLog(
                        DateTime.Now + Environment.NewLine +
                        ex.ToString() + Environment.NewLine +
                        (ex.InnerException?.ToString() ?? "") + Environment.NewLine);
                }
            }
            else
            {
#if DEBUG
                LogSkip(now);
#else
                if (_lastSkipLogDate == null || _lastSkipLogDate.Value.Date < now.Date)
                {
                    LogSkip(now);
                }
#endif
            }
        }

        private void LogSkip(DateTime now)
        {
            var message = $"{now} - Resguardo omitido: no es la hora programada ({_config?.BackupTime ?? "N/A"}).{Environment.NewLine}";
            if (message != _lastLogMessage || (now - _lastLogTime) > TimeSpan.FromMinutes(1))
            {
                File.AppendAllText(_logFile, message);
                _lastLogMessage = message;
                _lastLogTime = now;
                _lastSkipLogDate = now.Date;
            }
        }

        public void ForceBackup()
        {
            LoadConfiguration();
            if (_config == null)
            {
                return;
            }

            BackupService.PerformBackup(_config);
            _lastBackupDate = DateTime.Now.Date;
        }

        private void LoadConfiguration()
        {
            if (!File.Exists(_configFile))
            {
                SafeLog(
                    DateTime.Now + " - Config file not found: " + _configFile + Environment.NewLine);
                return;
            }

            try
            {
                var json = File.ReadAllText(_configFile);
                _config = JsonSerializer.Deserialize<AppConfig>(json);
            }
            catch (Exception ex)
            {
                SafeLog(
                    DateTime.Now + Environment.NewLine +
                    ex.ToString() + Environment.NewLine +
                    (ex.InnerException?.ToString() ?? "") + Environment.NewLine);
            }
        }

        private void SafeLog(string message)
        {
            try
            {
                var dir = Path.GetDirectoryName(_logFile);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.AppendAllText(_logFile, message);
            }
            catch (Exception logEx)
            {
                try
                {
                    EventLog.WriteEntry(ServiceName, $"Failed to write to log file: {logEx}", EventLogEntryType.Error);
                }
                catch
                {
                    // Ignored: nothing else we can do if logging fails
                }
            }
        }
    }
}
