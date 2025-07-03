using System;
using System.Diagnostics;
using System.Threading;

namespace AutoRip2MKV
{
    public class ProcessManager : IProcessManager
    {
        private readonly ILogger _logger;

        public ProcessManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ProcessResult StartProcess(string fileName, string arguments)
        {
            var result = new ProcessResult();
            
            try
            {
                _logger.Info("Starting process: {0} with arguments: {1}", fileName, arguments);
                
                var startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = false,
                    UseShellExecute = true,
                    FileName = fileName,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = arguments
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        while (!process.HasExited)
                        {
                            process.Refresh();
                            Thread.Sleep(2000);
                        }

                        result.ExitCode = process.ExitCode;
                        result.HasExited = true;
                        
                        _logger.Info("Process completed with exit code: {0}", process.ExitCode);
                    }
                    else
                    {
                        _logger.Error("Failed to start process: {0}", fileName);
                        result.Exception = new InvalidOperationException($"Failed to start process: {fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred while starting process: {0}", fileName);
                result.Exception = ex;
            }

            return result;
        }

        public void KillProcessesByName(string processName)
        {
            try
            {
                _logger.Info("Attempting to kill processes with name: {0}", processName);
                
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    try
                    {
                        _logger.Info("Killing process: {0} (ID: {1})", process.ProcessName, process.Id);
                        process.Kill();
                        process.WaitForExit(5000); // Wait up to 5 seconds for graceful exit
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn(ex, "Failed to kill process: {0} (ID: {1})", process.ProcessName, process.Id);
                    }
                    finally
                    {
                        process.Dispose();
                    }
                }
                
                _logger.Info("Completed killing {0} processes", processes.Length);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred while killing processes: {0}", processName);
            }
        }
    }
}
