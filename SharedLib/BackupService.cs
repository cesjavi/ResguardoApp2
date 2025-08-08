using System;
using System.IO;
using System.Linq;

namespace SharedLib
{
    public static class BackupService
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
                // Swallow any exceptions from logging to avoid recursive failures
            }
        }

        public static void PerformBackup(AppConfig config)
        {
            var sourceFolders = config.BackupFolders;
            if (!sourceFolders.Any())
            {
                // In a service, we might want to log this instead of showing a message box.
                return;
            }

            var discoRespaldo = config.DiscoRespaldo;
            if (discoRespaldo == null)
            {
                // Can't proceed without a registered backup disk.
                return;
            }

            var driveLetter = discoRespaldo.Letra.Replace("\\", "").ToUpper();
            var destinationRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");

            foreach (var sourceFolder in sourceFolders)
            {
                var sourceDir = new DirectoryInfo(sourceFolder);
                if (!sourceDir.Exists)
                {
                    LogError($"Source folder not found: {sourceDir.FullName}");
                    continue;
                }

                var destDir = new DirectoryInfo(Path.Combine(destinationRoot, sourceDir.Name));
                if (!destDir.Exists)
                    destDir.Create();

                try
                {
                    SynchronizeDirectory(sourceDir, destDir);
                }
                catch (Exception ex)
                {
                    LogError($"Failed to synchronize directory {sourceDir.FullName}", ex);
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
