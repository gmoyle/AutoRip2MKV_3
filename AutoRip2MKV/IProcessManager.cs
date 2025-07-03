using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutoRip2MKV
{
    public interface IProcessManager
    {
        ProcessResult StartProcess(string fileName, string arguments);
        Task<ProcessResult> StartProcessAsync(string fileName, string arguments, CancellationToken cancellationToken = default);
        void KillProcessesByName(string processName);
        Task KillProcessesByNameAsync(string processName, CancellationToken cancellationToken = default);
    }

    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public bool HasExited { get; set; }
        public Exception Exception { get; set; }
    }
}
