using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SharedLib
{
    public class BackupRecord
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Details { get; set; }
    }

    public static class BackupHistoryService
    {
        private static readonly string _historyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup_history.json");
        private static readonly string _logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_resguardo_service.txt");

        private static void LogError(string message, Exception? ex = null)
        {
            try
            {
                File.AppendAllText(_logFile,
                    DateTime.Now + " - " + message + Environment.NewLine +
                    (ex?.ToString() ?? string.Empty) + Environment.NewLine);
            }
            catch
            {
                // Swallow any exceptions from logging to avoid recursive failures
            }
        }

        public static void AddRecord(BackupRecord record)
        {
            try
            {
                List<BackupRecord> records = new();
                if (File.Exists(_historyFile))
                {
                    var json = File.ReadAllText(_historyFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var existing = JsonSerializer.Deserialize<List<BackupRecord>>(json);
                        if (existing != null)
                            records = existing;
                    }
                }
                records.Add(record);
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(_historyFile, JsonSerializer.Serialize(records, options));
            }
            catch (Exception ex)
            {
                LogError("Failed to add backup record", ex);
            }
        }

        public static IEnumerable<BackupRecord> GetRecords()
        {
            try
            {
                if (!File.Exists(_historyFile))
                    return Enumerable.Empty<BackupRecord>();

                var json = File.ReadAllText(_historyFile);
                var records = JsonSerializer.Deserialize<List<BackupRecord>>(json);
                return records ?? Enumerable.Empty<BackupRecord>();
            }
            catch (Exception ex)
            {
                LogError("Failed to read backup history", ex);
                return Enumerable.Empty<BackupRecord>();
            }
        }
    }
}
