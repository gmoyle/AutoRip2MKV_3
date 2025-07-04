using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace AutoRip2MKV
{
    /// <summary>
    /// Provides MessageBox functionality that auto-closes after a timeout in headless environments
    /// </summary>
    public static class HeadlessMessageBox
    {
        private const int DefaultTimeoutSeconds = 30;
        
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        
        private const uint WM_COMMAND = 0x0111;
        private const uint IDOK = 1;
        private const uint IDCANCEL = 2;

        /// <summary>
        /// Shows a MessageBox that will auto-close after the specified timeout in headless environments
        /// </summary>
        /// <param name="text">The text to display in the message box</param>
        /// <param name="caption">The text to display in the title bar of the message box</param>
        /// <param name="buttons">One of the MessageBoxButtons values that specifies which buttons to display in the message box</param>
        /// <param name="timeoutSeconds">The number of seconds to wait before auto-closing (default: 30)</param>
        /// <returns>One of the DialogResult values</returns>
        public static DialogResult Show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, int timeoutSeconds = DefaultTimeoutSeconds)
        {
            // Check if we're running in a headless environment (no interactive session)
            if (IsHeadlessEnvironment())
            {
                Logger.Info("Headless environment detected. Auto-answering MessageBox: {0} - {1}", caption, text);
                
                // Return appropriate default response based on button type
                return buttons switch
                {
                    MessageBoxButtons.OK => DialogResult.OK,
                    MessageBoxButtons.OKCancel => DialogResult.OK,
                    MessageBoxButtons.YesNo => DialogResult.Yes,
                    MessageBoxButtons.YesNoCancel => DialogResult.Yes,
                    MessageBoxButtons.RetryCancel => DialogResult.Retry,
                    MessageBoxButtons.AbortRetryIgnore => DialogResult.Retry,
                    _ => DialogResult.OK
                };
            }

            // For interactive environments, show the MessageBox with auto-close functionality
            return ShowWithTimeout(text, caption, buttons, timeoutSeconds);
        }

        /// <summary>
        /// Determines if the application is running in a headless environment
        /// </summary>
        /// <returns>True if headless, false if interactive</returns>
        private static bool IsHeadlessEnvironment()
        {
            try
            {
                // Check for CI environment variables
                var ciVariables = new[]
                {
                    "CI", "CONTINUOUS_INTEGRATION", "BUILD_NUMBER", "JENKINS_URL",
                    "TRAVIS", "CIRCLECI", "APPVEYOR", "GITLAB_CI", "GITHUB_ACTIONS",
                    "AZURE_HTTP_USER_AGENT", "TF_BUILD"
                };

                foreach (var variable in ciVariables)
                {
                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable)))
                    {
                        return true;
                    }
                }

                // Check if there's no interactive user session
                return Environment.UserInteractive == false || 
                       Environment.GetEnvironmentVariable("DISPLAY") == null && Environment.OSVersion.Platform == PlatformID.Unix;
            }
            catch
            {
                // If we can't determine, assume interactive to be safe
                return false;
            }
        }

        /// <summary>
        /// Shows a MessageBox with timeout functionality for interactive environments
        /// </summary>
        private static DialogResult ShowWithTimeout(string text, string caption, MessageBoxButtons buttons, int timeoutSeconds)
        {
            var result = DialogResult.None;
            var completed = false;

            // Create a task to show the MessageBox
            var messageBoxTask = Task.Run(() =>
            {
                try
                {
                    result = MessageBox.Show(text, caption, buttons);
                    completed = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error showing MessageBox: {0}", caption);
                    result = DialogResult.OK;
                    completed = true;
                }
            });

            // Create a timeout task
            var timeoutTask = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                
                if (!completed)
                {
                    Logger.Warn("MessageBox '{0}' timed out after {1} seconds, auto-closing", caption, timeoutSeconds);
                    
                    // Try to find and close the MessageBox window
                    var hwnd = FindWindow("#32770", caption); // #32770 is the class name for dialog boxes
                    if (hwnd != IntPtr.Zero)
                    {
                        // Send OK button click
                        SendMessage(hwnd, WM_COMMAND, new IntPtr(IDOK), IntPtr.Zero);
                    }
                }
            });

            // Wait for either the MessageBox to complete or timeout
            Task.WaitAny(messageBoxTask, timeoutTask);

            // If timeout occurred and MessageBox is still showing, set default result
            if (!completed)
            {
                result = buttons switch
                {
                    MessageBoxButtons.OK => DialogResult.OK,
                    MessageBoxButtons.OKCancel => DialogResult.OK,
                    MessageBoxButtons.YesNo => DialogResult.Yes,
                    MessageBoxButtons.YesNoCancel => DialogResult.Yes,
                    MessageBoxButtons.RetryCancel => DialogResult.Retry,
                    MessageBoxButtons.AbortRetryIgnore => DialogResult.Retry,
                    _ => DialogResult.OK
                };
            }

            return result;
        }
        
        /// <summary>
        /// Safely deletes a file without showing dialogs in headless environments
        /// </summary>
        /// <param name="filePath">Path to the file to delete</param>
        /// <param name="uiOption">UI option (ignored in headless environments)</param>
        /// <param name="recycleOption">Recycle option</param>
        public static void SafeDeleteFile(string filePath, UIOption uiOption, RecycleOption recycleOption)
        {
            try
            {
                if (IsHeadlessEnvironment())
                {
                    // In headless environments, delete directly without UI
                    if (File.Exists(filePath))
                    {
                        Logger.Info("Deleting file in headless mode: {0}", filePath);
                        if (recycleOption == RecycleOption.SendToRecycleBin)
                        {
                            // Try to use recycle bin, fall back to direct delete if not available
                            try
                            {
                                FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            }
                            catch
                            {
                                File.Delete(filePath);
                            }
                        }
                        else
                        {
                            File.Delete(filePath);
                        }
                    }
                }
                else
                {
                    // In interactive environments, use the original method
                    FileSystem.DeleteFile(filePath, uiOption, recycleOption);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to delete file: {0}", filePath);
                throw;
            }
        }
        
        /// <summary>
        /// Safely deletes a directory without showing dialogs in headless environments
        /// </summary>
        /// <param name="directoryPath">Path to the directory to delete</param>
        /// <param name="uiOption">UI option (ignored in headless environments)</param>
        /// <param name="recycleOption">Recycle option</param>
        public static void SafeDeleteDirectory(string directoryPath, UIOption uiOption, RecycleOption recycleOption)
        {
            try
            {
                if (IsHeadlessEnvironment())
                {
                    // In headless environments, delete directly without UI
                    if (Directory.Exists(directoryPath))
                    {
                        Logger.Info("Deleting directory in headless mode: {0}", directoryPath);
                        if (recycleOption == RecycleOption.SendToRecycleBin)
                        {
                            // Try to use recycle bin, fall back to direct delete if not available
                            try
                            {
                                FileSystem.DeleteDirectory(directoryPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            }
                            catch
                            {
                                Directory.Delete(directoryPath, true);
                            }
                        }
                        else
                        {
                            Directory.Delete(directoryPath, true);
                        }
                    }
                }
                else
                {
                    // In interactive environments, use the original method
                    FileSystem.DeleteDirectory(directoryPath, uiOption, recycleOption);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to delete directory: {0}", directoryPath);
                throw;
            }
        }
    }
}
