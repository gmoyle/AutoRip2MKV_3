using System;
using System.Diagnostics;

namespace AutoRip2MKV
{
    public interface IProcessManager
    {
        ProcessResult StartProcess(string fileName, string arguments);
        void KillProcessesByName(string processName);
    }

    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public bool HasExited { get; set; }
        public Exception Exception { get; set; }
    }
}
