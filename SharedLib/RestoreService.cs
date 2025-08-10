using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Management;

namespace SharedLib
{
    public static class RestoreService
    {
        private static readonly string _logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_resguardo_service.txt");

        private static void LogError(string message, Exception? ex = null)
        {
            try
            {
                File.AppendAllText(_logFile,
                    DateTime.Now + " - " + message + Environment.NewLine +
                    (ex?.ToString() ?? string.Empty) + Environment.NewLine);
            }
            catch
            {
                // Ignore logging failures
            }
        }

        public static void PerformRestore(AppConfig config)
        {
            var discoRespaldo = config.DiscoRespaldo;
            if (discoRespaldo == null)
            {
                return; // No hay disco de respaldo registrado
            }

            var driveLetter = discoRespaldo.Letra.Replace("\\", "").ToUpper();
            var actual = ObtenerInfoDeDisco(driveLetter);

            bool serialMatch = string.Equals(
                discoRespaldo.VolumeSerialNumber,
                actual.VolumeSerialNumber,
                StringComparison.OrdinalIgnoreCase);

            bool pnpMatch = true;
            if (!string.IsNullOrEmpty(discoRespaldo.PNPDeviceID) &&
                !string.IsNullOrEmpty(actual.PNPDeviceID))
            {
                pnpMatch = string.Equals(
                    discoRespaldo.PNPDeviceID,
                    actual.PNPDeviceID,
                    StringComparison.OrdinalIgnoreCase);
            }

            if (!serialMatch || !pnpMatch)
            {
                LogError("El disco seleccionado no coincide con el disco de respaldo registrado.");
                return;
            }

            var backupRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");
            var success = true;

            foreach (var folder in config.BackupFolders)
            {
                var originalDir = new DirectoryInfo(folder);
                var backupDir = new DirectoryInfo(Path.Combine(backupRoot, originalDir.Name));

                if (!backupDir.Exists)
                {
                    LogError($"Backup folder not found: {backupDir.FullName}");
                    success = false;
                    continue;
                }

                if (!originalDir.Exists)
                {
                    try
                    {
                        originalDir.Create();
                    }
                    catch (Exception ex)
                    {
                        LogError($"Failed to create destination folder {originalDir.FullName}", ex);
                        success = false;
                        continue;
                    }
                }

                try
                {
                    SynchronizeDirectory(backupDir, originalDir);
                }
                catch (Exception ex)
                {
                    LogError($"Failed to restore directory {originalDir.FullName}", ex);
                    success = false;
                }
            }

            var record = new BackupRecord
            {
                Timestamp = DateTime.Now,
                Status = success ? "Restore Success" : "Restore Error",
                Details = success ? null : "Restore completed with errors. Check logs."
            };
            BackupHistoryService.AddRecord(record);
        }

        private static void SynchronizeDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var sourceFile in source.GetFiles())
            {
                var destFile = new FileInfo(Path.Combine(destination.FullName, sourceFile.Name));
                if (!destFile.Exists || sourceFile.LastWriteTime > destFile.LastWriteTime)
                {
                    try
                    {
                        sourceFile.CopyTo(destFile.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        LogError($"Failed to copy file {sourceFile.FullName}", ex);
                    }
                }
            }

            foreach (var sourceSubDir in source.GetDirectories())
            {
                var destSubDir = new DirectoryInfo(Path.Combine(destination.FullName, sourceSubDir.Name));
                if (!destSubDir.Exists)
                {
                    destSubDir.Create();
                }

                try
                {
                    SynchronizeDirectory(sourceSubDir, destSubDir);
                }
                catch (Exception ex)
                {
                    LogError($"Failed to synchronize directory {sourceSubDir.FullName}", ex);
                }
            }
        }

        private static DiscoRespaldoInfo ObtenerInfoDeDisco(string letra)
        {
            var letraNormalizada = letra.ToUpper().Replace(":", string.Empty).Replace("\\", string.Empty);
            var info = new DiscoRespaldoInfo { Letra = letraNormalizada + ":" };

            try
            {
                string rootPath = letraNormalizada + @":\\";
                bool success = GetVolumeInformation(
                    rootPath,
                    null,
                    0,
                    out uint serialNumber,
                    out _, out _,
                    null,
                    0
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
                // ignore errors
            }

            return info;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetVolumeInformation(
            string lpRootPathName,
            System.Text.StringBuilder? lpVolumeNameBuffer,
            int nVolumeNameSize,
            out uint lpVolumeSerialNumber,
            out uint lpMaximumComponentLength,
            out uint lpFileSystemFlags,
            System.Text.StringBuilder? lpFileSystemNameBuffer,
            int nFileSystemNameSize);
    }
}

