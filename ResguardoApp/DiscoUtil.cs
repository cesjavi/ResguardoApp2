using System;
using System.Runtime.InteropServices;

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
            var info = new DiscoRespaldoInfo { Letra = letra.ToUpper() };

            try
            {
                string rootPath = letra.ToUpper() + @":\";
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
            catch (Exception ex)
            {
                info.VolumeSerialNumber = "EXCEPTION";
                // log ex.Message si quers
            }

            return info;
        }
    }
}
