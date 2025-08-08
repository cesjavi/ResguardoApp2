using System.Collections.Generic;

namespace SharedLib
{
    public class AppConfig
    {
        public List<string> BackupFolders { get; set; } = new List<string>();
        public DiscoRespaldoInfo? DiscoRespaldo { get; set; }
        public string? BackupTime { get; set; }
        public bool ForceBackupOnStart { get; set; }
    }

    public class DiscoRespaldoInfo
    {
        public string Letra { get; set; }
        public string VolumeSerialNumber { get; set; }
        public string PNPDeviceID { get; set; }
    }
}
