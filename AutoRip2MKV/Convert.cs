using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AutoRip2MKV
{

    class Convert
    {
        public static string convertlist = @Application.StartupPath + "\\ConvertList.log";
        public static string title = AutoRip2MKV.Properties.Settings.Default.CurrentTitle;
        public static string newtitle = title + "\r\n";

        public static void MainConvert(string[] args)
        {


        }

        public static void AddTitleToConvvertList()
        {

            if(File.Exists(convertlist))
            {
                File.AppendAllText(convertlist, newtitle);
            }
            else
            {
                File.WriteAllText(convertlist, newtitle);
            }



        }

        public static void ConvertWithHandbrake()
        {

            bool convertfilessetting = AutoRip2MKV.Properties.Settings.Default.ConvWithHandbrake;
            string parameters = AutoRip2MKV.Properties.Settings.Default.HandBrakeParameters;
            string tempPath = AutoRip2MKV.Properties.Settings.Default.TempPath;
            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakePath = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";



            if (convertfilessetting)
            {

                if (!File.Exists(convertlist))
                {
                    File.WriteAllText(convertlist, newtitle);
                }

                foreach (String titlestoconvert in File.ReadAllLines(convertlist))
                {
                    DirectoryInfo d = new DirectoryInfo(tempPath + "\\" + titlestoconvert);
                    if (d.Exists)
                    {
                        FileInfo[] files = d.GetFiles("*.mkv");
                        foreach (FileInfo f in files)
                        {
                            string shortFilename = tempPath + "\\" + title + "\\" + Path.GetFileNameWithoutExtension(f.FullName);

                            Convert.LaunchConversion(handbrakePath, f.FullName, shortFilename, parameters);
                            File.WriteAllText(convertlist, titlestoconvert.Replace(titlestoconvert, null));


                        }
                    }

                }
            }

        }

        static void LaunchConversion(String app, String source, String destination, String parameters)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "\"" + app + "\"",
                WindowStyle = ProcessWindowStyle.Minimized,
                Arguments = " -i \"" + source + "\" -o \"" + destination + parameters 
            };

            AutoRip2MKV.Ripping.UpdateStatusText("Launch: " + startInfo);



            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakeCommandLine = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";


            Process exeProcess = Process.Start(startInfo);

            if (app == handbrakeCommandLine)
            {
                while (!exeProcess.HasExited)
                {
                    exeProcess.Refresh();
                }

                if (exeProcess.ExitCode != 0)
                {
                    // Initializes the variables to pass to the MessageBox.Show method.

                    string message = "Conversion";
                    string caption = "Failed Conversion";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox
                    result = MessageBox.Show(message, caption, buttons);
                }
                else
                {

                    return;
                }
            }

        }

    }
}
