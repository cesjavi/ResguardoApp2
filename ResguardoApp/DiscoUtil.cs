using SharedLib;
using System;
using System.Runtime.InteropServices;
using System.Management;

namespace ResguardoApp
{
    

    public static class DiscoUtil
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetVolumeInformation(
            string lpRootPathName,
            System.Text.StringBuilder? lpVolumeNameBuffer,
            int nVolumeNameSize,
            out uint lpVolumeSerialNumber,
            out uint lpMaximumComponentLength,
            out uint lpFileSystemFlags,
            System.Text.StringBuilder? lpFileSystemNameBuffer,
            int nFileSystemNameSize);

        public static DiscoRespaldoInfo ObtenerInfoDeDisco(string letra)
        {
            var letraNormalizada = letra.ToUpper().Replace(":", string.Empty).Replace("\\", string.Empty);
            var info = new DiscoRespaldoInfo { Letra = letraNormalizada + ":" };

            try
            {
                string rootPath = letraNormalizada + @":\";
                System.Text.StringBuilder? volumeNameBuffer = null;
                System.Text.StringBuilder? fileSystemNameBuffer = null;
                bool success = GetVolumeInformation(
                    rootPath,
                    volumeNameBuffer, 0,
                    out uint serialNumber,
                    out _, out _,
                    fileSystemNameBuffer, 0
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
                using var query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                using ManagementObjectCollection drives = query.Get();
                foreach (ManagementObject drive in drives)
                {
                    using (drive)
                    {
                        using ManagementObjectCollection partitions = drive.GetRelated("Win32_DiskPartition");
                        foreach (ManagementObject partition in partitions)
                        {
                            using (partition)
                            {
                                using ManagementObjectCollection logicalDisks = partition.GetRelated("Win32_LogicalDisk");
                                foreach (ManagementObject ld in logicalDisks)
                                {
                                    using (ld)
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
