using System;
using System.Management;
namespace ResguardoApp;
/*public static class DiscoUtil
{
  * public static DiscoRespaldoInfo ObtenerInfoDeDisco(string letra)
    {
        var info = new DiscoRespaldoInfo { Letra = letra.ToUpper() };

        using (var mo = new ManagementObject($"win32_logicaldisk.deviceid=\"{letra.ToUpper()}:\""))
        {
            mo.Get();
            info.VolumeSerialNumber = mo["VolumeSerialNumber"]?.ToString();
        }

        var query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
        foreach (ManagementObject drive in query.Get())
        {
            var partitions = drive.GetRelated("Win32_DiskPartition");
            foreach (ManagementObject partition in partitions)
            {
                var logicalDisks = partition.GetRelated("Win32_LogicalDisk");
                foreach (ManagementObject ld in logicalDisks)
                {
                    if (ld["DeviceID"].ToString().Equals($"{letra.ToUpper()}:"))
                    {
                        info.PNPDeviceID = drive["PNPDeviceID"]?.ToString();
                        return info;
                    }
                }
            }
        }

        return info;
    }
}
*/