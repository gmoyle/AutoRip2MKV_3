using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace AutoRip2MKV
{
    public class Ripping
    {
        static Mutex m;

        public static bool Is64BitOperatingSystem { get; private set; }

        public static string CurrentTitle { get; private set; }
        public static string DVDDriveToUse { get; private set; }
        public static bool DontRip = false;

        // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
        [STAThread]
        public static void Main(string[] args)
        {

            try
            {
                Logger.LogOperationStart("Main");
                bool first = false;
                m = new Mutex(true, Application.ProductName.ToString(), out first);
                if ((first))
                {

                    Logger.Info("First instance running.");
                    
                    // Migrate credentials from plain text storage if needed
                    try
                    {
                        var credentialManager = ServiceContainer.Instance.Resolve<ICredentialManager>();
                        credentialManager.MigrateFromPlainText();
                    }
                    catch (InvalidOperationException ex)
                    {
                        Logger.Error(ex, "Operation failed during credential migration");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Unexpected error during credential migration");
                    }

                    var DVDDriveToUse = GetDriveInfo("drive");
                    var CurrentTitle = GetDriveInfo("label");

                    Logger.Info("Using DVD Drive: {0}, Current Title: {1}", DVDDriveToUse, CurrentTitle);

                    Properties.Settings.Default.CurrentTitle = CurrentTitle;
                    Properties.Settings.Default.DVDDrive = DVDDriveToUse;
                    UpdateStatusText("Clear");

                    SaveSettings();

                    Application.Run(new Preferences());
                }
                else
                {
                    Logger.Warn("Another instance is already running.");
                }
                Logger.LogOperationComplete("Main", TimeSpan.Zero); // Replace with actual timing logic if needed
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Fatal(ex, "Unauthorized access");
                UpdateStatusText("Application needs proper permissions to run");
            }
            catch (IOException ex)
            {
                Logger.Fatal(ex, "I/O error occurred");
                UpdateStatusText("An I/O error occurred");
            }
            catch (Exception ex)
            {
                Logger.LogOperationFailure("Main", ex);
                throw;
            }


        }

        public static void refreshdata()
        {
            var DVDDriveToUse = GetDriveInfo("drive");
            var CurrentTitle = GetDriveInfo("label");

            Properties.Settings.Default.CurrentTitle = CurrentTitle;
            Properties.Settings.Default.DVDDrive = DVDDriveToUse;

            SaveSettings();
            return;
        }
        public static void CheckHandBrakeInstall()
        {
            try
            {
                Logger.LogOperationStart("CheckHandBrakeInstall");
                string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                string handbrakePath = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";
                string handbrakeZip = myExecutablePath + @"\HandbrakeCLI.zip";

                Logger.Debug("Checking for HandBrake at: {0}", handbrakePath);

                if (File.Exists(handbrakePath))
                {
                    Logger.Info("HandBrake CLI found at: {0}", handbrakePath);
                    UpdateStatusText("Handbrake Lives");
                }
                else
                {
                    Logger.Warn("HandBrake CLI not found, checking for zip file: {0}", handbrakeZip);
                    if (File.Exists(handbrakeZip))
                    {
                        Logger.Info("HandBrake zip file found, extracting...");
                        // Path to directory of files to compress and decompress.
                        string dirpath = myExecutablePath;
                        DirectoryInfo di = new DirectoryInfo(dirpath);
                 
                        foreach (FileInfo fi in di.GetFiles("HandbrakeCLI.zip"))
                        {
                            Decompress();
                        }
                    }
                    else
                    {
                        Logger.Error("HandBrake CLI zip file not found at: {0}", handbrakeZip);
                    }
                }
                UpdateStatusText(myExecutablePath);
                Logger.LogOperationComplete("CheckHandBrakeInstall", TimeSpan.Zero);
            }
            catch (Exception ex)
            {
                Logger.LogOperationFailure("CheckHandBrakeInstall", ex);
                throw;
            }
        }

        public static void Decompress()
        {
            string handbrakeHomePath = AppDomain.CurrentDomain.BaseDirectory + @"\HandbrakeCLI";
            string handbrakeZip = AppDomain.CurrentDomain.BaseDirectory + @"\HandbrakeCLI.zip";
            UpdateStatusText("Extract Handbrake to: " + handbrakeHomePath);
            ZipFile.ExtractToDirectory(handbrakeZip, handbrakeHomePath);
            return;
        }

        public static void CheckMakeMKVInstall()
        {

            string tExpand = CheckMakeMKVRegistry();

            if (File.Exists (tExpand))
            {
                //code if key Exist
                UpdateStatusText("MakeMKV Lives!: " +  tExpand);
                Properties.Settings.Default.MakeMKVPath = tExpand;
                UpdateStatusText("MakeMKVPath: " + Properties.Settings.Default.MakeMKVPath);

                if (File.Exists(@"C:\Program Files (x86)\makemkv\makemkvcon64.exe"))
                {
                  
                    string makeMKV64Exists = @"C:\Program Files (x86)\MakeMKV\makemkvcon64.exe";
                    Properties.Settings.Default.MakeMKVPath = makeMKV64Exists;
                    UpdateStatusText("makeMKV_64_Path: " + Properties.Settings.Default.MakeMKVPath);
                }
                SaveSettings();
            }
            else
            {
                var mkvDownload = AppDomain.CurrentDomain.BaseDirectory + "Setup_MakeMKV_v1.12.0.exe";
                if (File.Exists (mkvDownload))
                {

                    string app = "Setup_MakeMKV_v1.10.10.exe";
                    string parameters = "/S /D";
                    UpdateStatusText("Installing MakeMKV:" + app + parameters);

                    LaunchCommandLineApp(app, parameters);
                }
                else
                {  
                    //code if key Not Exist
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile("https://www.makemkv.com/download/Setup_MakeMKV_v1.10.10.exe", "Setup_MakeMKV_v1.10.10.exe");

                        UpdateStatusText("Downloading MakeMKV: https://www.makemkv.com/download/Setup_MakeMKV_v1.10.10.exe");

                        string app = "Setup_MakeMKV_v1.10.10.exe";
                        string parameters = "/S /D";
                        LaunchCommandLineApp(app, parameters);


                    }
                }

            }
            return;
        }

        public static String CheckMakeMKVRegistry()
        {
            string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\makemkvcon.exe";
            string tExpand = (string)Registry.GetValue(keyName, null, "Default if MakeMKV does not exist.");
            //Console.WriteLine("tExpand: " + tExpand);
            return tExpand;
        }

        /// <summary>
        /// Launch the legacy application with some options set.
        /// </summary>
        public static void LaunchCommandLineApp(string app, string parameters)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = app,
                WindowStyle = ProcessWindowStyle.Minimized,
                Arguments = @parameters
            };

            UpdateStatusText("Launch: " + startInfo);

            Process exeProcess = Process.Start(startInfo);

            if (app == Properties.Settings.Default.MakeMKVPath)
            {
                while (!exeProcess.HasExited)
                {

                    // Discard cached information about the process.
                    exeProcess.Refresh();
                    Thread.Sleep(2000);
                }
                
                if (exeProcess.ExitCode == 0)
                {
                    RenameFiles();

                    if (Properties.Settings.Default.ConvWithHandbrake == true)
                    {
                        Convert.AddTitleToConvvertList();
                        Convert.ConvertWithHandbrake();
                    }

                    if (Properties.Settings.Default.FinalPath != "")
                    {
                        MoveFilesToFinalDestination();
                    }

                    Properties.Settings.Default.LastRipTitle = Properties.Settings.Default.CurrentTitle;
                    SMTPSender.Main(true);
                    SaveSettings();
                    OpenOrCloseCDDrive.Open();
                    Application.Exit();
                    return;
                }
                else
                {
                    SMTPSender.Main(false);
                    CleanupFailedRip();
                    OpenOrCloseCDDrive.Open();
                    Application.Exit();
                    return;
                }
            }

            return;
        }

        public static string GetDriveInfo(string results)
        {
            var allDrives = DriveInfo.GetDrives();

            foreach (var d in allDrives)
            {
                if (d.IsReady)
                {
                    switch (d.DriveFormat)
                    {
                        case "UDF" or "CDRom" or "M-Disc" or "M2TS":
                            return results switch
                            {
                                "label" => HandleDriveLabel(d),
                                "drive" => HandleDrivePath(d),
                                _ => "Default"
                            };
                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return "Default";
        }
        
        private static string HandleDriveLabel(DriveInfo d)
        {
            string volumeLabel = GetVolumeLabel(d.VolumeLabel).Replace(" ", "_");

            Properties.Settings.Default.CurrentTitle = volumeLabel;
            Properties.Settings.Default.DVDDrive = d.Name;

            return volumeLabel;
        }

        private static string HandleDrivePath(DriveInfo d)
        {
            string drivePath = d.Name;
            Properties.Settings.Default.CurrentTitle = d.VolumeLabel.Replace(" ", "_");
            Properties.Settings.Default.DVDDrive = drivePath;

            return drivePath;
        }
        public static string GetVolumeLabel(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
                
            string driveLabel = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            string cleanedLabel = driveLabel.Replace(" ", "");
            return cleanedLabel;

        }

        public static void RenameFiles()
         {

            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.TempPath + @"\" + CurrentTitle);
            FileInfo[] files = d.GetFiles("*.mkv");

            foreach (FileInfo f in files)
            {

                if (f.FullName.Contains("title"))
                {
                    string newname = f.FullName.Replace("title", CurrentTitle);
                    if (File.Exists(newname))
                    {
                        UpdateStatusText("Deleted existing Filename:" + newname);
                        HeadlessMessageBox.SafeDeleteFile(newname, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    File.Move(f.FullName, newname);
                    UpdateStatusText("Rename:" + f.FullName + " to:" + newname);
                }
                else
                {
                    UpdateStatusText("Rename not needed");
                }
            }
            return;
        }

        public static void MoveFilesToFinalDestination()
         {
        
            string CurrentTitle = Properties.Settings.Default.CurrentTitle;
            string sourcedir = Properties.Settings.Default.TempPath + @"\" + CurrentTitle;
            string targetdir = Properties.Settings.Default.FinalPath + @"\" + CurrentTitle;

            // Path to directory of files to compress and decompress.
            string dirpath = sourcedir;
            DirectoryInfo di = new DirectoryInfo(dirpath);

            foreach (FileInfo fi in di.GetFiles())
            {
                string source = sourcedir + "//" + fi.Name;
                string target = targetdir + "//" + fi.Name;

                try
                {
                    UpdateStatusText("Copying ... " + source + " ==> " + target);
                    File.Copy(source, target);
                    UpdateStatusText("Completed " + source + " ==> " + target);
                }
                catch
                {
                    if (Directory.Exists(targetdir))
                    {
                        UpdateStatusText("Movie Directory Already Exists, Deleting: " + targetdir);
                        HeadlessMessageBox.SafeDeleteFile(target, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        UpdateStatusText("Deleted existing file: " + target);
                        UpdateStatusText("Copying ... " + source + " ==> " + target);
                        File.Copy(source, target);
                        UpdateStatusText("Completed " + source + " ==> " + target);

                    }
                }
                if (File.Exists(target))
                {
                    HeadlessMessageBox.SafeDeleteFile(source, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    UpdateStatusText("Deleted: " + source);
                }
            }
             
            UpdateStatusText("Deleting.... " + sourcedir);
            HeadlessMessageBox.SafeDeleteDirectory(sourcedir, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            UpdateStatusText("Deleted: " + sourcedir);

            return;
        }



        public static bool CheckForRecentRip(string checkFinalPath)
        {
            // Path to already ripped  files.
            DirectoryInfo di = new DirectoryInfo(checkFinalPath);
            if (di.Exists)
            {
                foreach (FileInfo fi in di.GetFiles("*.mkv"))
                {
                    DateTime currentDateTime = DateTime.Now;
                    DateTime filedate = fi.LastWriteTime;

                    // compare date and time
                    TimeSpan diff1 = currentDateTime.Subtract(filedate);

                    if (diff1.Hours < 24)
                    {
                        DontRip = true;
                        Properties.Settings.Default.RipRetry = Properties.Settings.Default.RipRetry + 1;
                        SaveSettings();
                        return DontRip;
                    }
                }
                return DontRip;
            }

            DontRip = false;
            Properties.Settings.Default.RipRetry = 0;
            SaveSettings();
            return DontRip;

        }

        public static bool CheckVariables()
        {
            try
            {
                Logger.LogOperationStart("CheckVariables");
                
                // Use the new configuration manager for proper validation
                var configManager = ServiceContainer.Instance.Resolve<IConfigurationManager>();
                var validationResult = configManager.ValidateAndInitialize();
                
                // Log validation results
                if (!validationResult.IsValid)
                {
                    Logger.Error("Configuration validation failed:");
                    foreach (var error in validationResult.Errors)
                    {
                        Logger.Error("  - {0}", error);
                        UpdateStatusText($"Config Error: {error}");
                    }
                }
                
                // Log warnings
                foreach (var warning in validationResult.Warnings)
                {
                    Logger.Warn("  - {0}", warning);
                    UpdateStatusText($"Config Warning: {warning}");
                }
                
                Logger.LogOperationComplete("CheckVariables", TimeSpan.Zero);
                return validationResult.IsValid;
            }
            catch (Exception ex)
            {
                Logger.LogOperationFailure("CheckVariables", ex);
                UpdateStatusText($"Configuration check failed: {ex.Message}");
                return false;
            }
        }

        static void CleanupFailedRip() 
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;
            if (Properties.Settings.Default.FinalPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.FinalPath != null)
                {
                    if (CurrentTitle != "")
                    {
                        var foldertodelete = @Properties.Settings.Default.FinalPath + @"\" + CurrentTitle;
                        UpdateStatusText("Delete: " + foldertodelete);
                        HeadlessMessageBox.SafeDeleteDirectory(foldertodelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }

                }
               
            }
            if (Properties.Settings.Default.TempPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.TempPath != null)
                {
                    if (CurrentTitle != "")
                    {
                        var foldertodelete = @Properties.Settings.Default.TempPath + @"\" + CurrentTitle;
                        UpdateStatusText("Delete: " + foldertodelete);
                        HeadlessMessageBox.SafeDeleteDirectory(foldertodelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                }
            }
            return;
        }
        public static void MakeWorkingDirs()
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;

            if (CurrentTitle != "")
            {
                try
                {
                    Directory.CreateDirectory(@Properties.Settings.Default.TempPath + @"\" + CurrentTitle);
                }
                catch
                {
                    UpdateStatusText("Cannot create " + @Properties.Settings.Default.TempPath + @"\" + CurrentTitle);
                }


                try
                {
                    Directory.CreateDirectory(@Properties.Settings.Default.FinalPath + @"\" + CurrentTitle);
                }
                catch
                {
                    UpdateStatusText("Cannot create " + @Properties.Settings.Default.FinalPath + @"\" + CurrentTitle);
                }
            }
        }

        public static void SaveSettings()
        {

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

        }

        public static void UpdateStatusText(string update)
        {
            if (update == "Clear")
            {
                Properties.Settings.Default.StatusText = null;
            }
            else
            {
                Properties.Settings.Default.StatusText = update + Environment.NewLine + Properties.Settings.Default.StatusText;
            }
            SaveSettings();
        }
    }

}
