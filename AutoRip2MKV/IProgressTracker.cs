using System;
using System.Threading;

namespace AutoRip2MKV
{
    public interface IProgressTracker
    {
        event EventHandler<ProgressEventArgs> ProgressChanged;
        void ReportProgress(string operation, double percentComplete, string status);
        void ReportProgress(FileProgress fileProgress);
        void ReportProgress(string processStatus);
        void StartOperation(string operationName);
        void CompleteOperation(string operationName, bool success, string result = null);
    }

    public class ProgressEventArgs : EventArgs
    {
        public string Operation { get; set; }
        public double PercentComplete { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCompleted { get; set; }
        public bool Success { get; set; }
        public string Result { get; set; }
    }

    public class ProgressTracker : IProgressTracker
    {
        private readonly ILogger _logger;
        private readonly SynchronizationContext _syncContext;

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public ProgressTracker(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _syncContext = SynchronizationContext.Current;
        }

        public void ReportProgress(string operation, double percentComplete, string status)
        {
            var args = new ProgressEventArgs
            {
                Operation = operation,
                PercentComplete = percentComplete,
                Status = status,
                Timestamp = DateTime.Now,
                IsCompleted = false
            };

            _logger.Debug("Progress: {0} - {1:F1}% - {2}", operation, percentComplete, status);
            RaiseProgressChanged(args);
        }

        public void ReportProgress(FileProgress fileProgress)
        {
            var args = new ProgressEventArgs
            {
                Operation = $"{fileProgress.Operation} File",
                PercentComplete = fileProgress.PercentComplete,
                Status = fileProgress.Status,
                Timestamp = DateTime.Now,
                IsCompleted = false
            };

            _logger.Debug("File Progress: {0} - {1:F1}% - {2}", fileProgress.Operation, fileProgress.PercentComplete, fileProgress.Status);
            RaiseProgressChanged(args);
        }

        public void ReportProgress(string processStatus)
        {
            var args = new ProgressEventArgs
            {
                Operation = "Process",
                PercentComplete = -1, // Indeterminate
                Status = processStatus,
                Timestamp = DateTime.Now,
                IsCompleted = false
            };

            _logger.Debug("Process Progress: {0}", processStatus);
            RaiseProgressChanged(args);
        }

        public void StartOperation(string operationName)
        {
            var args = new ProgressEventArgs
            {
                Operation = operationName,
                PercentComplete = 0,
                Status = "Starting...",
                Timestamp = DateTime.Now,
                IsCompleted = false
            };

            _logger.Info("Starting operation: {0}", operationName);
            RaiseProgressChanged(args);
        }

        public void CompleteOperation(string operationName, bool success, string result = null)
        {
            var args = new ProgressEventArgs
            {
                Operation = operationName,
                PercentComplete = 100,
                Status = success ? "Completed successfully" : "Failed",
                Timestamp = DateTime.Now,
                IsCompleted = true,
                Success = success,
                Result = result
            };

            _logger.Info("Operation {0}: {1} - {2}", operationName, success ? "completed" : "failed", result ?? "");
            RaiseProgressChanged(args);
        }

        private void RaiseProgressChanged(ProgressEventArgs args)
        {
            if (_syncContext != null)
            {
                _syncContext.Post(_ => ProgressChanged?.Invoke(this, args), null);
            }
            else
            {
                ProgressChanged?.Invoke(this, args);
            }
        }
    }
}
