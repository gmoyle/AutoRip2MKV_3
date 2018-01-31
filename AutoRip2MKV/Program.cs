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

            CheckHandBrakeInstall();
            CheckMakeMKVInstall();
            var DVDDriveToUse = GetDriveInfo("drive");
            var CurrentTitle = GetDriveInfo("label");
            Properties.Settings.Default.CurrentTitle = CurrentTitle;
            Properties.Settings.Default.DVDDrive = DVDDriveToUse;
            Program.CheckVariables();
            UpdateStatusText("Clear");

            Properties.Settings.Default.Save(); // Saves settings in application configuration file
            Properties.Settings.Default.Upgrade();

            Application.Run(new AutoRip2MKV.Preferences());
        }

        static void CheckHandBrakeInstall()
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
            ZipFile.ExtractToDirectory(handbrakeZip, handbrakeHomePath);
            return;
        }

        static void CheckMakeMKVInstall()
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
                    Program.LaunchCommandLineApp(app, parameters);
                }
                else
                {  
                    //code if key Not Exist
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile("https://www.makemkv.com/download/Setup_MakeMKV_v1.10.10.exe", "Setup_MakeMKV_v1.10.10.exe");
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
            // For the example
            // const string ex1 = "C:\\";
            //const string ex2 = "C:\\Dir";

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = app;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = " " + parameters;

            try
            {
                Process exeProcess = Process.Start(startInfo);

                while (!exeProcess.HasExited)
                {
                    // Discard cached information about the process.
                    exeProcess.Refresh();
                    // Wait 2 seconds.
                    System.Threading.Thread.Sleep(2000);
                }
                if (exeProcess.ExitCode == 0)
                {
                    RenameFiles();
                    MoveFilesToFinalDestination();
                    return;
                }
                else 
                {
                    //Directory.Delete(topPath, true);
                    CleanupFailedRip();
                    return;
                }
            }
            catch
            {
                UpdateStatusText("App execution failed");
            }
            finally
            {
                Environment.Exit(0);
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
                            return volumeLabel;
                        }
                        else if (results == "drive")
                        {
                            string drivePath = d.Name;
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
            FileInfo[] files = d.GetFiles();

            foreach (FileInfo f in files)
            {
                File.Move(f.FullName, f.FullName.Replace("title", CurrentTitle));
            }
            return;
        }

        public static void MoveFilesToFinalDestination()
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;
            Directory.Move(Properties.Settings.Default.TempPath + "\\" + CurrentTitle, Properties.Settings.Default.FinalPath + "\\" + CurrentTitle);
            return;
        }

        public static void Rip2MKV(string destination)
        {
            Program.MakeWorkingDirs();

            string makeMKVPath = Properties.Settings.Default.MakeMKVPath;

            UpdateStatusText("Rip makeMKVPath: " + makeMKVPath);
            
            if (File.Exists(makeMKVPath))
            {

                string ripPath = destination + "\\" + CurrentTitle;
                string minTitleLength = Properties.Settings.Default.MinTitleLength;
                var driveID = DVDDriveToUse;

                string MakeMKVOptions = " mkv --decrypt --noscan --minlength=1200 --robot --directio=true disc:0 1 " + ripPath;

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

                    // Closes the app .

                    Environment.Exit(0);

                }
            }
            
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            return;
        }

        static void CheckVariables()
        {
            try
            {
                Directory.CreateDirectory(Properties.Settings.Default.TempPath);
            }
            catch
            {
                Properties.Settings.Default.TempPath = "";
            }

            try
            {
                Directory.CreateDirectory(Properties.Settings.Default.FinalPath);
            }
            catch
            {
                Properties.Settings.Default.FinalPath = "";
            }

            if (Properties.Settings.Default.FinalPath == "" && Properties.Settings.Default.TempPath == "")
            {
                Properties.Settings.Default.Timout = false;
                Properties.Settings.Default.Save(); // Saves settings in application configuration file
                Properties.Settings.Default.Upgrade();
            }
            return;
        }

        static void CleanupFailedRip() 
        {
            CurrentTitle = Properties.Settings.Default.CurrentTitle;
            if (Properties.Settings.Default.FinalPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.FinalPath != null)
                {
                    //Directory.Delete(Properties.Settings.Default.FinalPath + "\\" + CurrentTitle, true);
                    var test = Properties.Settings.Default.FinalPath + "\\" + CurrentTitle;
                }
               
            }
            if (Properties.Settings.Default.TempPath != "" && CurrentTitle != null)
            {
                if (Properties.Settings.Default.TempPath != null)
                {
                    //Directory.Delete(Properties.Settings.Default.TempPath + "\\" + CurrentTitle, true);
                    var test = Properties.Settings.Default.TempPath + "\\" + CurrentTitle;
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
            return;
        }

        public static void UpdateStatusText(string update)
        {
            if (update == "Clear")
            {
                Properties.Settings.Default.StatusText = null;
            }
            else
            {
                Properties.Settings.Default.StatusText = Properties.Settings.Default.StatusText + "\n" + update + "\n";
            }
        }
    }
}
