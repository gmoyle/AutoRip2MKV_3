using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoRip2MKV
{
    public interface IFileOperations
    {
        Task<bool> FileExistsAsync(string path);
        Task<bool> DirectoryExistsAsync(string path);
        Task CreateDirectoryAsync(string path);
        Task CopyFileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);
        Task MoveFileAsync(string sourcePath, string destinationPath);
        Task DeleteFileAsync(string path);
        Task DeleteDirectoryAsync(string path, bool recursive = false);
        Task<FileInfo[]> GetFilesAsync(string directoryPath, string searchPattern = "*");
        Task<DriveInfo[]> GetDrivesAsync();
    }
}
