using System;

namespace SharedLib
{
    public class BackupProgress
    {
        public long TotalBytes { get; set; }
        public long ProcessedBytes { get; set; }
        public TimeSpan? EstimatedTimeRemaining { get; set; }
    }
}
