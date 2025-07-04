using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoRip2MKV
{
    public class FileProgress
    {
        public string Operation { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public long BytesTransferred { get; set; }
        public long TotalBytes { get; set; }
        public double PercentComplete => TotalBytes > 0 ? (double)BytesTransferred / TotalBytes * 100 : 0;
        public TimeSpan TimeElapsed { get; set; }
        public string Status { get; set; }
    }

    public interface IFileOperations
    {
        Task<bool> FileExistsAsync(string path);
        Task<bool> DirectoryExistsAsync(string path);
        Task CreateDirectoryAsync(string path);
        Task CopyFileAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default, IProgress<FileProgress> progress = null);
        Task MoveFileAsync(string sourcePath, string destinationPath);
        Task DeleteFileAsync(string path);
        Task DeleteDirectoryAsync(string path, bool recursive = false);
        Task<FileInfo[]> GetFilesAsync(string directoryPath, string searchPattern = "*");
        Task<DriveInfo[]> GetDrivesAsync();
    }
}
