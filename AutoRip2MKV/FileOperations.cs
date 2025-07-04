using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace AutoRip2MKV
{
    public class FileOperations : IFileOperations
    {
        private readonly ILogger _logger;

        public FileOperations(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            return await Task.Run(() => File.Exists(path));
        }

        public async Task<bool> DirectoryExistsAsync(string path)
        {
            return await Task.Run(() => Directory.Exists(path));
        }

        public async Task CreateDirectoryAsync(string path)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        _logger.Info("Creating directory: {0}", path);
                        Directory.CreateDirectory(path);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to create directory: {0}", path);
                    throw;
                }
            });
        }

        public async Task CopyFileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default, IProgress<FileProgress> progress = null)
        {
            await Task.Run(() =>
            {
                try
                {
                    _logger.Info("Copying file from {0} to {1}", sourcePath, destinationPath);
                    
                    // Ensure destination directory exists
                    var destinationDir = Path.GetDirectoryName(destinationPath);
                    if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                    {
                        Directory.CreateDirectory(destinationDir);
                    }

                    using (var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                    using (var destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                    {
                        var buffer = new byte[8192];
                        int bytesRead;
                        long totalBytesRead = 0;
                        var totalBytes = source.Length;
                        var startTime = DateTime.Now;
                        var lastProgressTime = startTime;
                        
                        var fileProgress = new FileProgress
                        {
                            Operation = "Copy",
                            SourcePath = sourcePath,
                            DestinationPath = destinationPath,
                            TotalBytes = totalBytes,
                            Status = "Starting copy operation"
                        };
                        
                        progress?.Report(fileProgress);
                        
                        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cancellationToken.ThrowIfCancellationRequested(); // Check for cancellation
                            destination.Write(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;
                            
                            // Report progress every 1MB or 1 second, whichever comes first
                            var now = DateTime.Now;
                            if (totalBytesRead % (1024 * 1024) == 0 || (now - lastProgressTime).TotalSeconds >= 1)
                            {
                                fileProgress.BytesTransferred = totalBytesRead;
                                fileProgress.TimeElapsed = now - startTime;
                                fileProgress.Status = $"Copying... {fileProgress.PercentComplete:F1}% complete";
                                progress?.Report(fileProgress);
                                lastProgressTime = now;
                            }
                        }
                        
                        // Report completion
                        fileProgress.BytesTransferred = totalBytes;
                        fileProgress.TimeElapsed = DateTime.Now - startTime;
                        fileProgress.Status = "Copy completed";
                        progress?.Report(fileProgress);
                    }
                    
                    _logger.Info("Successfully copied file from {0} to {1}", sourcePath, destinationPath);
                }
                catch (OperationCanceledException)
                {
                    _logger.Info("File copy operation cancelled: {0} to {1}", sourcePath, destinationPath);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to copy file from {0} to {1}", sourcePath, destinationPath);
                    throw;
                }
            }, cancellationToken);
        }

        public async Task MoveFileAsync(string sourcePath, string destinationPath)
        {
            await Task.Run(() =>
            {
                try
                {
                    _logger.Info("Moving file from {0} to {1}", sourcePath, destinationPath);
                    
                    // Ensure destination directory exists
                    var destinationDir = Path.GetDirectoryName(destinationPath);
                    if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                    {
                        Directory.CreateDirectory(destinationDir);
                    }

                    File.Move(sourcePath, destinationPath);
                    _logger.Info("Successfully moved file from {0} to {1}", sourcePath, destinationPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to move file from {0} to {1}", sourcePath, destinationPath);
                    throw;
                }
            });
        }

        public async Task DeleteFileAsync(string path)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (File.Exists(path))
                    {
                        _logger.Info("Deleting file: {0}", path);
                        HeadlessMessageBox.SafeDeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        _logger.Info("Successfully deleted file: {0}", path);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to delete file: {0}", path);
                    throw;
                }
            });
        }

        public async Task DeleteDirectoryAsync(string path, bool recursive = false)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        _logger.Info("Deleting directory: {0} (recursive: {1})", path, recursive);
                        HeadlessMessageBox.SafeDeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        _logger.Info("Successfully deleted directory: {0}", path);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to delete directory: {0}", path);
                    throw;
                }
            });
        }

        public async Task<FileInfo[]> GetFilesAsync(string directoryPath, string searchPattern = "*")
        {
            return await Task.Run(() =>
            {
                try
                {
                    var directory = new DirectoryInfo(directoryPath);
                    if (directory.Exists)
                    {
                        return directory.GetFiles(searchPattern);
                    }
                    return new FileInfo[0];
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to get files from directory: {0}", directoryPath);
                    throw;
                }
            });
        }

        public async Task<DriveInfo[]> GetDrivesAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return DriveInfo.GetDrives();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to get drive information");
                    throw;
                }
            });
        }
    }
}
