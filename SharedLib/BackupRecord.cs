using System;

namespace SharedLib
{
    [Serializable]
    public class BackupRecord
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Details { get; set; }
    }
}
