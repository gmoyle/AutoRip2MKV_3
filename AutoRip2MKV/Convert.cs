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
        public static void MainConvert(string[] args)
        {


        }

        public static void AddTitleToConvvertList()
        {
            string convertlist = "ConvertList.log";
            string newtitle = AutoRip2MKV.Properties.Settings.Default.CurrentTitle + "\r\n";

            try
            {
                File.AppendAllText(convertlist, newtitle);
            }
            catch
            {


            }
            finally
            {

            }
        }

        public static void ConvertWithHandbrake()
        {
            string convertlist = "ConvertList.log";
            bool convertfilessetting = AutoRip2MKV.Properties.Settings.Default.ConvWithHandbrake;
            string parameters = AutoRip2MKV.Properties.Settings.Default.HandBrakeParameters;
            string tempPath = AutoRip2MKV.Properties.Settings.Default.TempPath;
            string myExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
            string handbrakePath = myExecutablePath + @"\HandbrakeCLI\HandbrakeCLI.exe";


            if (convertfilessetting)
            {
                foreach (String titlestoconvert in File.ReadAllLines(convertlist))
                {
                    DirectoryInfo d = new DirectoryInfo(tempPath + "\\" + titlestoconvert);
                    FileInfo[] files = d.GetFiles("*.mkv");
                    foreach (FileInfo f in files)
                    {
                        string shortFilename = Path.GetFileNameWithoutExtension(f.FullName);

                        AutoRip2MKV.Convert.LaunchConversion(handbrakePath, f.FullName, shortFilename, parameters);  
                        File.WriteAllText(convertlist, titlestoconvert.Replace(titlestoconvert, null));


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
                UseShellExecute = true,
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

                if (exeProcess.ExitCode == 0)
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


                }
            }

        }

    }
}
