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

namespace AutoRip2MKV
{
    class Program
    {
        public static bool Is64BitOperatingSystem { get; private set; }

        static void Main(string[] args)
        {
            CheckHandBrakeInstall();
            CheckMakeMKVInstall();
            GetDriveInfo();
            Rip2Temp();
            Console.ReadLine();
            Properties.Settings.Default.DVDDrive = @"F:\";
            //Properties.Settings.Default.Upgrade();
            //Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        static void CheckHandBrakeInstall()
        {
            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakePath = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";
            string handbrakeZip = myExecutablePath + @"\HandbrakeCLI.zip";



            if (File.Exists(handbrakePath))
            {
                Console.WriteLine("Handbrake Lives");
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
            Console.WriteLine(myExecutablePath);

        }

        public static void Decompress()
        {
            string handbrakeHomePath = AppDomain.CurrentDomain.BaseDirectory + @"\HandbrakeCLI";
            string handbrakeZip = AppDomain.CurrentDomain.BaseDirectory + @"\HandbrakeCLI.zip";
            ZipFile.ExtractToDirectory(handbrakeZip, handbrakeHomePath);
        }

        static void CheckMakeMKVInstall()
        {
            string tExpand = checkMakeMKVRegistry();

            if (File.Exists (tExpand))
            {
                //code if key Exist
                //Console.WriteLine("MakeMKV Lives!: {0}", tExpand);
                Properties.Settings.Default.MakeMKVPath = tExpand;
                Properties.Settings.Default.Save(); // Saves settings in application configuration file
                //Console.WriteLine("makeMKVPath: " + Properties.Settings.Default.MakeMKVPath);

                if (File.Exists(@"C:\Program Files (x86)\makemkv\makemkvcon64.exe"))
                {
                    if (Program.Is64BitOperatingSystem)
                    {
                        string makeMKV64Exists = @"C:\Program Files (x86)\MakeMKV\makemkvcon64.exe";
                        Properties.Settings.Default.MakeMKVPath = makeMKV64Exists;
                        //Properties.Settings.Default.Upgrade();
                        Properties.Settings.Default.Save(); // Saves settings in application configuration file
                        //Console.WriteLine("makeMKV_64_Path: " + Properties.Settings.Default.MakeMKVPath);
                    }


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
        }

        public static String checkMakeMKVRegistry()
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
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
        }

        public static string GetDriveInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    if (d.DriveFormat == "UDF" || d.DriveFormat == "CDRom" || d.DriveFormat == "M-Disc" || d.DriveFormat == "M2TS")
                    {
                        string volumeLabel = RemoveSpecialCharacters(d.VolumeLabel);
                        return volumeLabel;
                    }
                }
                else
                {
                    return null;
                }
            }
            return "Default";
        }
        public static string RemoveSpecialCharacters(string fileName)
        {
            string driveLabel = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            return driveLabel;

        }

        static void Rip2Temp()
        {
            string makeMKVPath = Properties.Settings.Default.MakeMKVPath;
            Console.WriteLine("Rip makeMKVPath: " + makeMKVPath);
            if (File.Exists(makeMKVPath))
            {

                string tempPath = Properties.Settings.Default.TempPath;
                string minTitleLength = Properties.Settings.Default.MinTitleLength;
                string driveID = Properties.Settings.Default.DVDDrive;
                string driveLabel = Program.GetDriveInfo();

                //string MakeMKVOptions = "bot --directio=true --messages \"" + tempPath + "\\MKVrip.txt \" --decrypt --minlength=" + minTitleLength + " mkv disc:" +  driveID + " " + driveLabel + "/" + tempPath + "/";
                string MakeMKVOptions = " mkv --decrypt --noscan --minlength=1200 --robot --directio=true disc:0 all " + tempPath;

                string app = makeMKVPath;
                Console.WriteLine("Rip2temp: " + app + MakeMKVOptions);
                LaunchCommandLineApp(app, MakeMKVOptions);
            }
            else
            {
                Console.WriteLine("no mkv found");
            }


        }

        static void Rip2Final()
        {

        }
        static void MakeTMPDir()
        {

        }
        static void MakeFinalDir()
        {

        }


    }

}
