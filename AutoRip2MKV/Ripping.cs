using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

namespace AutoRip2MKV
{
    class Ripping
    {

        public static bool Is64BitOperatingSystem { get; private set; }

        public static string CurrentTitle { get; private set; }
        public static string DVDDriveToUse { get; private set; }
        public static bool DontRip = false;
        public static bool results;

        // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
        [STAThread]
        public static void Main(string[] args)
        {
            //OpenOrCloseCDDrive.Open();

            var DVDDriveToUse = GetDriveInfo("drive");
            var CurrentTitle = GetDriveInfo("label");

            //var CurrentTitle = "GregsTestLabel";
            
            Properties.Settings.Default.CurrentTitle = CurrentTitle;
            Properties.Settings.Default.DVDDrive = DVDDriveToUse;
            UpdateStatusText("Clear");

            SaveSettings();

            Application.Run(new AutoRip2MKV.Preferences());

        }

        public static void CheckHandBrakeInstall()
        {
            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakePath = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";
            string handbrakeZip = myExecutablePath + @"\HandbrakeCLI.zip";



            if (File.Exists(handbrakePath))
            {
                UpdateStatusText("Handbrake Lives");
            }
            else
            {
                if (File.Exists(handbrakeZip))
                {
                    
                    // Path to directory of files to compress and decompress.
                    string dirpath = myExecutablePath;
                    DirectoryInfo di = new DirectoryInfo(dirpath);
             
                    foreach (FileInfo fi in di.GetFiles("HandbrakeCLI.zip"))
                    {
                        Decompress();
                    }
                }
                    
            }
            UpdateStatusText(myExecutablePath);
            return;

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

                    AutoRip2MKV.Ripping.LaunchCommandLineApp(app, parameters);
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
                        AutoRip2MKV.Ripping.LaunchCommandLineApp(app, parameters);


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
        static void LaunchCommandLineApp(string app, string parameters)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = app,
                WindowStyle = ProcessWindowStyle.Minimized,
                Arguments = " " + parameters
            };


            UpdateStatusText("Launch: " + startInfo);

            Process exeProcess = Process.Start(startInfo);

            if (app == Properties.Settings.Default.MakeMKVPath)
            {
                while (!exeProcess.HasExited)
                {

                    Properties.Settings.Default.TimerGroup = false;
                    //exeProcess.WaitForExit();
                    // Discard cached information about the process.
                    exeProcess.Refresh();
                    // Wait 2 seconds.
                    // System.Threading.Thread.Sleep(2000);
                }
                // Discard cached information about the process.
                
                if (exeProcess.ExitCode == 0)
                {
                    RenameFiles();

                    if (Properties.Settings.Default.ConvWithHandbrake == true)
                    {
                        AutoRip2MKV.Convert.AddTitleToConvvertList();
                        AutoRip2MKV.Convert.ConvertWithHandbrake();
                    }

                    if (Properties.Settings.Default.FinalPath != "")
                    {
                        MoveFilesToFinalDestination();
                    }
                    results = true;
                    SMTPSender.Main(results);
                    Properties.Settings.Default.CurrentTitle ="";
                    Properties.Settings.Default.DVDDrive = "";
                    SaveSettings();

                    OpenOrCloseCDDrive.Open();

                return;
                }
                else
                {

                    SMTPSender.Main(results);
                    CleanupFailedRip();

                    return;
                }
            }
     
            return;
        }

        public static string GetDriveInfo(string results)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    if (d.DriveFormat == "UDF" || d.DriveFormat == "CDRom" || d.DriveFormat == "M-Disc" || d.DriveFormat == "M2TS")
                    {
                        if (results == "label")
                        {
                            string volumeLabel = GetVolumeLabel(d.VolumeLabel);

                            Properties.Settings.Default.CurrentTitle = volumeLabel;
                            Properties.Settings.Default.DVDDrive = d.Name;

                            return volumeLabel;
                        }
                        else if (results == "drive")
                        {
                            string drivePath = d.Name;
                            Properties.Settings.Default.CurrentTitle = d.VolumeLabel;
                            Properties.Settings.Default.DVDDrive = drivePath;

                            return drivePath;
                        }
                        SaveSettings();
                    }
                }
                else
                {
                    return null;
                }
            }
            return "Default";
        }
        public static string GetVolumeLabel(string fileName)
        {
            string driveLabel = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            return driveLabel;

        }

        public static void RenameFiles()
         {

            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.TempPath + "\\" + CurrentTitle);
            FileInfo[] files = d.GetFiles("*.mkv");

            foreach (FileInfo f in files)
            {

                if (f.FullName.Contains("title"))
                {
                    string newname = f.FullName.Replace("title", CurrentTitle);
                    if (File.Exists(newname))
                    {
                        UpdateStatusText("Deleted existing Filename:" + newname);
                        FileSystem.DeleteFile(newname, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
            string sourcedir = Properties.Settings.Default.TempPath + "\\" + CurrentTitle;
            string targetdir = Properties.Settings.Default.FinalPath + "\\" + CurrentTitle;

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
                    if (System.IO.Directory.Exists(targetdir))
                    {
                        UpdateStatusText("Movie Directory Already Exists, Deleting: " + targetdir);
                        FileSystem.DeleteFile(target, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        UpdateStatusText("Deleted existing file: " + target);
                        UpdateStatusText("Copying ... " + source + " ==> " + target);
                        File.Copy(source, target);
                        UpdateStatusText("Completed " + source + " ==> " + target);

                    }
                }
                if (File.Exists(target))
                {
                    FileSystem.DeleteFile(source, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    UpdateStatusText("Deleted: " + source);
                }
            }
             
            UpdateStatusText("Deleting.... " + sourcedir);
            FileSystem.DeleteDirectory(sourcedir, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            UpdateStatusText("Deleted: " + sourcedir);

            return;
        }

        public static void Rip2MKV(string destination)
        {
            string checkFinalPath = Properties.Settings.Default.FinalPath + "\\" + Properties.Settings.Default.CurrentTitle; ;
            AutoRip2MKV.Ripping.CheckForRecentRip(checkFinalPath);

            if (!DontRip)
            {
                AutoRip2MKV.Ripping.MakeWorkingDirs();
                string makeMKVPath = Properties.Settings.Default.MakeMKVPath;

                if (File.Exists(makeMKVPath))
                {
                    string ripPath = destination + "\\" + CurrentTitle;
                    UpdateStatusText("Ripping to: " + ripPath);
                    char[] charsToTrim = { '\\' };
                    string activeDisc = Properties.Settings.Default.DVDDrive.TrimEnd(charsToTrim);
                    string minTitleLength = Properties.Settings.Default.MinTitleLength;
                    var driveID = DVDDriveToUse;

                    string MakeMKVOptions = " --robot --messages=" + ripPath + "\\riplog.txt --decrypt --noscan --minlength=" + minTitleLength + " --directio=true mkv disc:0 all " + ripPath;

                    string app = makeMKVPath;

                    LaunchCommandLineApp(app, MakeMKVOptions);
                }
                else
                {
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = "MakeMKV not found";
                    string caption = "Error Detected in Input";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.RipRetry >= 1)
                {
                    OpenOrCloseCDDrive.Open();
                }   
            }
            SaveSettings();
            return;
        }

        public static void CheckForRecentRip(string checkFinalPath)
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
                        return;
                    }
                }
                return;
            }

            DontRip = false;
            Properties.Settings.Default.RipRetry = 0;
            SaveSettings();
            return;

        }

        public static bool CheckVariables()
        {
            try
            { 
            Directory.CreateDirectory(Properties.Settings.Default.FinalPath);
            Directory.CreateDirectory(Properties.Settings.Default.TempPath);
            }
            catch { }


            if (!System.IO.Directory.Exists(Properties.Settings.Default.TempPath))
            {
                if (Properties.Settings.Default.TempPath == "" || Properties.Settings.Default.TempPath == null)
                {
                    UpdateStatusText("TempPath was invalid, Set to C:\\temp\\Movies");
                    Properties.Settings.Default.TempPath = @"C:\temp\Movies";
                }
                else
                {
                    Properties.Settings.Default.TempPath = @"C:\temp\Movies";
                }
            }


            if (!System.IO.Directory.Exists(Properties.Settings.Default.FinalPath))
            {
                if (Properties.Settings.Default.FinalPath == "" && Properties.Settings.Default.TempPath == "")
                {
                    UpdateStatusText("FinalPath and TempPath invalid. TempPath Set to C:\\temp\\Movies");
                    Properties.Settings.Default.FinalPath = "";
                }
                else
                {
                    Properties.Settings.Default.FinalPath = "";
                }
                if (Properties.Settings.Default.FinalPath == "" && Properties.Settings.Default.TempPath == "")
                {
                    Properties.Settings.Default.Timeout = false;
                    return false;
                }
            }
            SaveSettings();
            return true;
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
                        var foldertodelete = Properties.Settings.Default.FinalPath + "\\" + CurrentTitle;
                        UpdateStatusText("Delete: " + foldertodelete);
                        FileSystem.DeleteDirectory(foldertodelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }

                }
               
            }
            if (Properties.Settings.Default.TempPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.TempPath != null)
                {
                    if (CurrentTitle != "")
                    {
                        var foldertodelete = Properties.Settings.Default.TempPath + "\\" + CurrentTitle;
                        UpdateStatusText("Delete: " + foldertodelete);
                        FileSystem.DeleteDirectory(foldertodelete, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
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
                    Directory.CreateDirectory(Properties.Settings.Default.TempPath + "\\" + CurrentTitle);
                }
                catch
                {
                    UpdateStatusText("Cannot create " + Properties.Settings.Default.TempPath + "\\" + CurrentTitle);
                }


                try
                {
                    Directory.CreateDirectory(Properties.Settings.Default.FinalPath + "\\" + CurrentTitle);
                }
                catch
                {
                    UpdateStatusText("Cannot create " + Properties.Settings.Default.FinalPath + "\\" + CurrentTitle);
                }
            }
        }

        private static void SaveSettings()
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
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
