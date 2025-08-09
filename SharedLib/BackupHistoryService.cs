using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SharedLib
{
    public static class BackupHistoryService
    {
        private static readonly string HistoryFile;

        static BackupHistoryService()
        {
            var logDir = Environment.GetEnvironmentVariable("RESGUARDO_LOG_PATH");
            if (string.IsNullOrWhiteSpace(logDir))
            {
                logDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "ResguardoApp");
            }

            HistoryFile = Path.Combine(logDir, "backup_history.txt");
        }

        public static void AddRecord(BackupRecord record)
        {
            var directory = Path.GetDirectoryName(HistoryFile);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(record);
            File.AppendAllText(HistoryFile, json + Environment.NewLine);
        }

        public static IList<BackupRecord> GetRecords()
        {
            if (!File.Exists(HistoryFile))
            {
                return new List<BackupRecord>();
            }

            var records = new List<BackupRecord>();
            foreach (var line in File.ReadLines(HistoryFile))
            {
                try
                {
                    var record = JsonSerializer.Deserialize<BackupRecord>(line);
                    if (record != null)
                    {
                        records.Add(record);
                    }
                }
                catch
                {
                    // Ignore malformed lines
                }
            }

            return records;
        }

        public static void Clear()
        {
            if (File.Exists(HistoryFile))
            {
                File.Delete(HistoryFile);
            }
        }
    }
}
