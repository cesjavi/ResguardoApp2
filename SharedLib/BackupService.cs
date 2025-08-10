using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            PerformBackupAsync(config, null, CancellationToken.None).GetAwaiter().GetResult();
        }

        public static async Task PerformBackupAsync(AppConfig config, IProgress<BackupProgress>? progress, CancellationToken cancellationToken)
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
            var destinationRoot = Path.Combine($"{driveLetter}\\", "ResguardoApp");

            long totalBytes = 0;
            foreach (var folder in sourceFolders)
            {
                var dir = new DirectoryInfo(folder);
                if (dir.Exists)
                {
                    totalBytes += CalculateDirectorySize(dir);
                }
            }

            var progressData = new BackupProgress { TotalBytes = totalBytes, ProcessedBytes = 0 };
            var stopwatch = Stopwatch.StartNew();

            var success = true;
            foreach (var sourceFolder in sourceFolders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var sourceDir = new DirectoryInfo(sourceFolder);
                if (!sourceDir.Exists)
                {
                    LogError($"Source folder not found: {sourceDir.FullName}");
                    success = false;
                    continue;
                }

                var destDir = new DirectoryInfo(Path.Combine(destinationRoot, sourceDir.Name));
                if (!destDir.Exists)
                    destDir.Create();

                try
                {
                    await SynchronizeDirectory(sourceDir, destDir, progressData, progress, stopwatch, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogError($"Failed to synchronize directory {sourceDir.FullName}", ex);
                    success = false;
                }
            }

            stopwatch.Stop();

            var record = new BackupRecord
            {
                Timestamp = DateTime.Now,
                Status = success ? "Success" : "Error",
                Details = success ? null : "Backup completed with errors. Check logs.",
            };
            BackupHistoryService.AddRecord(record);
        }

        private static long CalculateDirectorySize(DirectoryInfo directory)
        {
            long size = 0;
            try
            {
                size += directory.GetFiles().Sum(f => f.Length);
                foreach (var dir in directory.GetDirectories())
                {
                    size += CalculateDirectorySize(dir);
                }
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate directory size for {directory.FullName}", ex);
            }
            return size;
        }

        private static async Task SynchronizeDirectory(DirectoryInfo source, DirectoryInfo destination, BackupProgress progressData, IProgress<BackupProgress>? progress, Stopwatch stopwatch, CancellationToken cancellationToken)
        {
            foreach (var sourceFile in source.GetFiles())
            {
                cancellationToken.ThrowIfCancellationRequested();

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

                progressData.ProcessedBytes += sourceFile.Length;
                if (progressData.ProcessedBytes > 0 && progressData.TotalBytes > 0)
                {
                    var elapsed = stopwatch.Elapsed;
                    var estimatedTotal = TimeSpan.FromTicks((long)(elapsed.Ticks * (double)progressData.TotalBytes / progressData.ProcessedBytes));
                    progressData.EstimatedTimeRemaining = estimatedTotal - elapsed;
                }
                progress?.Report(new BackupProgress
                {
                    TotalBytes = progressData.TotalBytes,
                    ProcessedBytes = progressData.ProcessedBytes,
                    EstimatedTimeRemaining = progressData.EstimatedTimeRemaining
                });
            }

            foreach (var sourceSubDir in source.GetDirectories())
            {
                cancellationToken.ThrowIfCancellationRequested();

                var destinationSubDir = new DirectoryInfo(Path.Combine(destination.FullName, sourceSubDir.Name));
                if (!destinationSubDir.Exists)
                {
                    destinationSubDir.Create();
                }
                try
                {
                    await SynchronizeDirectory(sourceSubDir, destinationSubDir, progressData, progress, stopwatch, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogError($"Failed to synchronize directory {sourceSubDir.FullName}", ex);
                }
            }
        }
    }
}
