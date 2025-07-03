using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<ProcessResult> StartProcessAsync(string fileName, string arguments, CancellationToken cancellationToken = default, IProgress<string> progress = null)
        {
            var result = new ProcessResult();
            
            try
            {
                _logger.Info("Starting process async: {0} with arguments: {1}", fileName, arguments);
                
                var startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,  // Changed to false for async operation
                    FileName = fileName,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    
                    if (process != null)
                    {
                        progress?.Report($"Started process {fileName}");
                        
                        // Wait for process exit asynchronously with cancellation support
                        await Task.Run(() => 
                        {
                            var startTime = DateTime.Now;
                            var lastProgressTime = startTime;
                            
                            while (!process.HasExited && !cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                Thread.Sleep(1000);
                                
                                // Report progress every 5 seconds
                                var now = DateTime.Now;
                                if ((now - lastProgressTime).TotalSeconds >= 5)
                                {
                                    var elapsed = now - startTime;
                                    progress?.Report($"Process {fileName} running for {elapsed.ToString(@"mm\:ss")}");
                                    lastProgressTime = now;
                                }
                            }
                        }, cancellationToken);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                if (!process.HasExited)
                                {
                                    process.Kill();
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Warn("Failed to kill process during cancellation: {0}. Error: {1}", fileName, ex.Message);
                            }
                            
                            result.Exception = new OperationCanceledException("Process was cancelled");
                            return result;
                        }

                        result.ExitCode = process.ExitCode;
                        result.HasExited = true;
                        
                        _logger.Info("Process completed async with exit code: {0}", process.ExitCode);
                    }
                    else
                    {
                        _logger.Error("Failed to start process async: {0}", fileName);
                        result.Exception = new InvalidOperationException($"Failed to start process: {fileName}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Info("Process start cancelled: {0}", fileName);
                result.Exception = new OperationCanceledException("Process was cancelled");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred while starting process async: {0}", fileName);
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
                        _logger.Warn("Failed to kill process: {0} (ID: {1}). Error: {2}", process.ProcessName, process.Id, ex.Message);
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

        public async Task KillProcessesByNameAsync(string processName, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Info("Attempting to kill processes async with name: {0}", processName);
                
                var processes = Process.GetProcessesByName(processName);
                var killTasks = new List<Task>();
                
                foreach (var process in processes)
                {
                    killTasks.Add(Task.Run(() => 
                    {
                        try
                        {
                            _logger.Info("Killing process async: {0} (ID: {1})", process.ProcessName, process.Id);
                            process.Kill();
                            
                            // Wait for exit with cancellation support
                            var waitTask = Task.Run(() => process.WaitForExit(5000));
                            waitTask.Wait(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn("Failed to kill process async: {0} (ID: {1}). Error: {2}", process.ProcessName, process.Id, ex.Message);
                        }
                        finally
                        {
                            process.Dispose();
                        }
                    }, cancellationToken));
                }
                
                await Task.WhenAll(killTasks);
                
                _logger.Info("Completed killing {0} processes async", processes.Length);
            }
            catch (OperationCanceledException)
            {
                _logger.Info("Kill processes operation cancelled: {0}", processName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred while killing processes async: {0}", processName);
            }
        }
    }
}
