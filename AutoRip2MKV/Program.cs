using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

namespace AutoRip2MKV
{
    class Program
    {

        public static bool Is64BitOperatingSystem { get; private set; }

        public static string CurrentTitle { get; private set; }
        public static string DVDDriveToUse { get; private set; }

        // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
        [STAThread]
        public static void Main(string[] args)
        {
            //OpenOrCloseCDDrive.Open();

            var DVDDriveToUse = GetDriveInfo("drive");
            var CurrentTitle = GetDriveInfo("label");
            Properties.Settings.Default.CurrentTitle = CurrentTitle;
            Properties.Settings.Default.DVDDrive = DVDDriveToUse;
            UpdateStatusText("Clear");

            Properties.Settings.Default.Save(); // Saves settings in application configuration file
            Properties.Settings.Default.Upgrade();

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
                Properties.Settings.Default.Save(); // Saves settings in application configuration file
                UpdateStatusText("MakeMKVPath: " + Properties.Settings.Default.MakeMKVPath);

                if (File.Exists(@"C:\Program Files (x86)\makemkv\makemkvcon64.exe"))
                {
                  
                    string makeMKV64Exists = @"C:\Program Files (x86)\MakeMKV\makemkvcon64.exe";
                    Properties.Settings.Default.MakeMKVPath = makeMKV64Exists;
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                    UpdateStatusText("makeMKV_64_Path: " + Properties.Settings.Default.MakeMKVPath);
                }

            }
            else
            {
                var mkvDownload = AppDomain.CurrentDomain.BaseDirectory + "Setup_MakeMKV_v1.10.10.exe";
                if (File.Exists (mkvDownload))
                {

                    string app = "Setup_MakeMKV_v1.10.10.exe";
                    string parameters = "/S /D";
                    UpdateStatusText("Installing MakeMKV:" + app + parameters);

                    Program.LaunchCommandLineApp(app, parameters);
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
                        Program.LaunchCommandLineApp(app, parameters);


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
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = app;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = " " + parameters;

  
                UpdateStatusText("Launch: " + startInfo);
            AutoRip2MKV.Preferences.ActiveForm.WindowState = FormWindowState.Minimized;

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
                        if (Properties.Settings.Default.FinalPath != "")
                        {
                            MoveFilesToFinalDestination();
                        }
                    Properties.Settings.Default.CurrentTitle ="";
                    Properties.Settings.Default.DVDDrive = "";
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                    OpenOrCloseCDDrive.Open();

                    return;
                    }
                    else
                    {
                        //Directory.Delete(topPath, true);
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
                            Properties.Settings.Default.Save(); // Saves settings in application configuration file
                            Properties.Settings.Default.Upgrade();

                            return volumeLabel;
                        }
                        else if (results == "drive")
                        {
                            string drivePath = d.Name;
                            Properties.Settings.Default.CurrentTitle = d.VolumeLabel;
                            Properties.Settings.Default.DVDDrive = drivePath;
                            Properties.Settings.Default.Save(); // Saves settings in application configuration file
                            Properties.Settings.Default.Upgrade();

                            return drivePath;
                        }

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
                        File.Delete(newname);
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
                        File.Delete(target);
                        UpdateStatusText("Deleted existing file: " + target);
                        UpdateStatusText("Copying ... " + source + " ==> " + target);
                        File.Copy(source, target);
                        UpdateStatusText("Completed " + source + " ==> " + target);

                    }
                }
                if (File.Exists(target))
                {
                    File.Delete(source);
                    UpdateStatusText("Deleted: " + source);
                }
            }
             
            UpdateStatusText("Deleting.... " + sourcedir);
            Directory.Delete(sourcedir);
            UpdateStatusText("Deleted: " + sourcedir);

            return;
        }

        public static void Rip2MKV(string destination)
        {
            Program.MakeWorkingDirs();

            string makeMKVPath = Properties.Settings.Default.MakeMKVPath;

            if (File.Exists(makeMKVPath))
            {

                string ripPath = destination + "\\" + CurrentTitle;
                UpdateStatusText("Ripping to: " + ripPath);
                char[] charsToTrim = { '\\' };
                string activeDisc = Properties.Settings.Default.DVDDrive.TrimEnd(charsToTrim);
                string minTitleLength = Properties.Settings.Default.MinTitleLength;
                var driveID = DVDDriveToUse;

                string MakeMKVOptions = " --robot --messages=" + ripPath + "\\riplog.txt --decrypt --noscan --minlength=" + minTitleLength + " --directio=true mkv disc:0 all " + ripPath ;

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
            
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
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
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                }
                else
                {
                    Properties.Settings.Default.TempPath = @"C:\temp\Movies";
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                }
            }


            if (!System.IO.Directory.Exists(Properties.Settings.Default.FinalPath))
            {
                if (Properties.Settings.Default.FinalPath == "" && Properties.Settings.Default.TempPath == "")
                {
                    UpdateStatusText("FinalPath and TempPath invalid. TempPath Set to C:\\temp\\Movies");
                    Properties.Settings.Default.FinalPath = "";
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                }
                else
                {
                    Properties.Settings.Default.FinalPath = "";
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                }
                if (Properties.Settings.Default.FinalPath == "" && Properties.Settings.Default.TempPath == "")
                {
                    Properties.Settings.Default.Timout = false;
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                    return false;
                }
            }
            return true;
        }

        static void CleanupFailedRip() 
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;
            if (Properties.Settings.Default.FinalPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.FinalPath != null)
                {
                    var foldertodelete = Properties.Settings.Default.FinalPath + "\\" + CurrentTitle;
                    UpdateStatusText("Delete: " + foldertodelete);
                    Directory.Delete(foldertodelete, true);

                }
               
            }
            if (Properties.Settings.Default.TempPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.TempPath != null)
                {
                    var foldertodelete = Properties.Settings.Default.TempPath + "\\" + CurrentTitle;
                    UpdateStatusText("Delete: " + foldertodelete);
                    Directory.Delete(foldertodelete, true);
                }
            }
            return;
        }
        public static void MakeWorkingDirs()
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;

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
       
        public static void UpdateStatusText(string update)
        {
            if (update == "Clear")
            {
                Properties.Settings.Default.StatusText = null;
                Properties.Settings.Default.Save(); // Saves settings in application configuration file
                Properties.Settings.Default.Upgrade();
            }
            else
            {
                Properties.Settings.Default.StatusText = update + Environment.NewLine + Properties.Settings.Default.StatusText;
            }
        }
    }

}
