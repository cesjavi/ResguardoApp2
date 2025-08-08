using System;
using System.Runtime.InteropServices;
using System.Management;
using SharedLib;

namespace ResguardoApp
{
    

    public static class DiscoUtil
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetVolumeInformation(
            string lpRootPathName,
            System.Text.StringBuilder lpVolumeNameBuffer,
            int nVolumeNameSize,
            out uint lpVolumeSerialNumber,
            out uint lpMaximumComponentLength,
            out uint lpFileSystemFlags,
            System.Text.StringBuilder lpFileSystemNameBuffer,
            int nFileSystemNameSize);

        public static DiscoRespaldoInfo ObtenerInfoDeDisco(string letra)
        {
            var letraNormalizada = letra.ToUpper().Replace(":", string.Empty).Replace("\\", string.Empty);
            var info = new DiscoRespaldoInfo { Letra = letraNormalizada + ":" };

            try
            {
                string rootPath = letraNormalizada + @":\";
                bool success = GetVolumeInformation(
                    rootPath,
                    null, 0,
                    out uint serialNumber,
                    out _, out _,
                    null, 0
                );

                if (success)
                {
                    info.VolumeSerialNumber = serialNumber.ToString("X");
                }
                else
                {
                    info.VolumeSerialNumber = "ERROR";
                }
            }
            catch (Exception)
            {
                info.VolumeSerialNumber = "EXCEPTION";
                // log ex.Message si querés
            }

            try
            {
                var query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject drive in query.Get())
                {
                    var partitions = drive.GetRelated("Win32_DiskPartition");
                    foreach (ManagementObject partition in partitions)
                    {
                        var logicalDisks = partition.GetRelated("Win32_LogicalDisk");
                        foreach (ManagementObject ld in logicalDisks)
                        {
                            if (string.Equals(ld["DeviceID"]?.ToString(), $"{letraNormalizada}:", StringComparison.OrdinalIgnoreCase))
                            {
                                info.PNPDeviceID = drive["PNPDeviceID"]?.ToString();
                                return info;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Si falla la consulta, se deja PNPDeviceID como null
            }

            return info;
        }
    }
}
