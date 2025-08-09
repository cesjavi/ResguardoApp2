using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public static IList<string> GetRecords()
        {
            if (!File.Exists(HistoryFile))
            {
                return new List<string>();
            }

            return File.ReadAllLines(HistoryFile).ToList();
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
