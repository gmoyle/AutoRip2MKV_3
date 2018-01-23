using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AutoRip2MKV
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckHandBrakeInstall();
            CheckMakeMKVInstall();
            GetDriveInfo();
            Console.ReadLine();
        }

        static void CheckHandBrakeInstall()
        {
            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakePath = myExecutablePath + "\\HandbrakeCLI\\HandbrakeCLI.exe";
            string handbrakeZip = myExecutablePath + "\\HandbrakeCLI.zip";



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
            string handbrakeHomePath = AppDomain.CurrentDomain.BaseDirectory + "\\HandbrakeCLI";
            string handbrakeZip = AppDomain.CurrentDomain.BaseDirectory + "\\HandbrakeCLI.zip";
            ZipFile.ExtractToDirectory(handbrakeZip, handbrakeHomePath);
        }

        static void CheckMakeMKVInstall()
        {

            string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\makemkvcon.exe";
            string tExpand = (string) Registry.GetValue(keyName, null, "Default if TestArray does not exist." );
            
            if (File.Exists (tExpand))
            {
                //code if key Exist
                Console.WriteLine("MakeMKV Lives!: {0}", tExpand);
            }
            else
            {
                //code if key Not Exist
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://www.makemkv.com/download/Setup_MakeMKV_v1.10.10.exe", "Setup_MakeMKV_v1.10.10.exe");
                }
            }




        }

       
        static void GetDriveInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    if (d.DriveFormat == "UDF" || d.DriveFormat == "CDRom" || d.DriveFormat == "M-Disc" || d.DriveFormat == "M2TS")
                    {
                        string volumeLabel = RemoveSpecialCharacters(d.VolumeLabel);
                    }
                }
            }
        }
        public static string RemoveSpecialCharacters(string fileName)
        {
            string test = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));

            Console.WriteLine("Cleaned: " + test);
            return test;

        }

    }

 }
