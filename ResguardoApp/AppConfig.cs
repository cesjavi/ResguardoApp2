namespace ResguardoApp;
public class AppConfig
{
    public List<string> BackupFolders { get; set; } = new();
    public DiscoRespaldoInfo DiscoRespaldo { get; set; }
    public string BackupTime { get; set; }
}

public class DiscoRespaldoInfo
{
    public string Letra { get; set; }
    public string VolumeSerialNumber { get; set; }
    public string PNPDeviceID { get; set; }
}
