using System;
using System.IO;
using System.Linq;

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
                // Ignorar errores de log para evitar fallos recursivos
            }
        }

        public static void PerformRestore(AppConfig config)
        {
            var sourceFolders = config.BackupFolders;
            if (!sourceFolders.Any())
            {
                return;
            }

            var discoRespaldo = config.DiscoRespaldo;
            if (discoRespaldo == null)
            {
                return;
            }

            var driveLetter = discoRespaldo.Letra.Replace("\\", "").ToUpper();
            var backupRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");

            foreach (var originalFolder in sourceFolders)
            {
                var folderName = new DirectoryInfo(originalFolder).Name;
                var sourceDir = new DirectoryInfo(Path.Combine(backupRoot, folderName));
                if (!sourceDir.Exists)
                {
                    LogError($"Backup folder not found: {sourceDir.FullName}");
                    continue;
                }

                var destDir = new DirectoryInfo(originalFolder);
                if (!destDir.Exists)
                    destDir.Create();

                try
                {
                    SynchronizeDirectory(sourceDir, destDir);
                }
                catch (Exception ex)
                {
                    LogError($"Failed to restore directory {originalFolder}", ex);
                }
            }
        }

        private static void SynchronizeDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (var sourceFile in source.GetFiles())
            {
                var destinationFile = new FileInfo(Path.Combine(destination.FullName, sourceFile.Name));
                if (!destinationFile.Exists || sourceFile.LastWriteTime > destinationFile.LastWriteTime)
                {
                    try
                    {
                        sourceFile.CopyTo(destinationFile.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        LogError($"Failed to copy file {sourceFile.FullName}", ex);
                    }
                }
            }

            foreach (var sourceSubDir in source.GetDirectories())
            {
                var destinationSubDir = new DirectoryInfo(Path.Combine(destination.FullName, sourceSubDir.Name));
                if (!destinationSubDir.Exists)
                {
                    destinationSubDir.Create();
                }
                try
                {
                    SynchronizeDirectory(sourceSubDir, destinationSubDir);
                }
                catch (Exception ex)
                {
                    LogError($"Failed to synchronize directory {sourceSubDir.FullName}", ex);
                }
            }
        }
    }
}
