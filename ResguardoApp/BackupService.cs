using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ResguardoApp
{
    public static class BackupService
    {
        public static void PerformBackup(AppConfig config)
        {
            var sourceFolders = config.BackupFolders;
            if (!sourceFolders.Any())
            {
                // In a service, we might want to log this instead of showing a message box.
                return;
            }

            if (config.DiscoRespaldo == null)
            {
                // Can't proceed without a registered backup disk.
                return;
            }

            var driveLetter = config.DiscoRespaldo.Letra.Replace("\\", "").ToUpper();
            var destinationRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");

            foreach (var sourceFolder in sourceFolders)
            {
                var sourceDir = new DirectoryInfo(sourceFolder);
                if (!sourceDir.Exists)
                {
                    Console.WriteLine($"Source folder not found: {sourceDir.FullName}");
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
                    Console.WriteLine($"Failed to synchronize directory {sourceDir.FullName}: {ex.Message}");
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
                        Console.WriteLine($"Failed to copy file {sourceFile.FullName}: {ex.Message}");
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
                    Console.WriteLine($"Failed to synchronize directory {sourceSubDir.FullName}: {ex.Message}");
                }
            }
        }
    }
}
