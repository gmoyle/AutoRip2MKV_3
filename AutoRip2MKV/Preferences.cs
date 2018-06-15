using AutoRip2MKV.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AutoRip2MKV
{
    public partial class Preferences : Form
    {

        // This integer variable keeps track of the 
        // remaining time.
        int timeLeft;

        public string CurrentTitle { get; private set; }

        public Preferences()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal;

           Ripping.CheckHandBrakeInstall();
           Ripping.CheckMakeMKVInstall();

            //AutoRip2MKV.Convert.AddTitleToConvvertList();
            //AutoRip2MKV.Convert.ConvertWithHandbrake();


            StartTheTimer();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Set window location
            if (Settings.Default.WindowLocation != null)
            {
                this.Location = Settings.Default.WindowLocation;
            }

            // Set window size
            if (Settings.Default.WindowSize != null)
            {
                this.Size = Settings.Default.WindowSize;
            }
            this.WindowState = FormWindowState.Normal;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
                //if the form is minimized  
                //hide it from the task bar  
                //and show the system tray icon (represented by the NotifyIcon control)  
                if (this.WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    notifyIcon.BalloonTipTitle = "AutoRip2MKV";
                    if (Settings.Default.CurrentTitle == "")
                    {
                        notifyIcon.BalloonTipText = "No Disc";
                    }
                    else
                    {
                        notifyIcon.BalloonTipText = Settings.Default.CurrentTitle;
                    }
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(1);
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void FormMain_FormClosing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Save_Click(object sender, EventArgs e)
        {

            // Copy window location to app settings
            Settings.Default.WindowLocation = this.Location;
            Settings.Default.WindowSize = this.Size;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = this.Size;
            }
            else
            {
                Settings.Default.WindowSize = this.RestoreBounds.Size;
            }
            Ripping.SaveSettings();
        }

        private void makeMKVParams_TextChanged(object sender, EventArgs e)
        {

        }

        private void keepMKV_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }

        private void textBoxCurrentTitle_TextChanged(object sender, EventArgs e)
        {
            if(textBoxCurrentTitle.Text != "")
            {
                buttonRipMovie.Enabled = true;
                textBoxCurrentTitle.Text = Settings.Default.CurrentTitle;
                dvdDriveID.Text = Settings.Default.DVDDrive;
            }
            else
            {
                StartTheTimer();
                buttonRipMovie.Enabled = false;
            }

        }

    
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.WorkerSupportsCancellation = true;
            var destination = e.Argument.ToString();
            string checkFinalPath = @Properties.Settings.Default.FinalPath + @"\" + Settings.Default.CurrentTitle;
            bool DontRip = Ripping.CheckForRecentRip(checkFinalPath);
            if (Settings.Default.LastRipTitle  == Settings.Default.CurrentTitle)
            {
                DontRip  = false;
            }
            if (!DontRip)
            {
                Ripping.MakeWorkingDirs();
                string makeMKVPath = Settings.Default.MakeMKVPath;

                if (System.IO.File.Exists(makeMKVPath))
                {
                    string ripPath = @destination + @"\" + CurrentTitle;
                    Ripping.UpdateStatusText("Ripping to: " + ripPath);
                    char[] charsToTrim = { '\\' };
                    string activeDisc = Settings.Default.DVDDrive.TrimEnd(charsToTrim);
                    string minTitleLength = Settings.Default.MinTitleLength;
                    var driveID = Ripping.DVDDriveToUse;

                    string MakeMKVOptions = " --robot --messages=" + @ripPath + "riplog.txt --decrypt --noscan --minlength=" + minTitleLength + " --directio=true mkv disc:0 all " + @ripPath;

                    string app = makeMKVPath;

                    Ripping.LaunchCommandLineApp(@app, @MakeMKVOptions);
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
                if (Settings.Default.RipRetry >= 2)
                {
                    return;
                }
            }
            Ripping.SaveSettings();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBoxCurrentTitle.Text != "")
            {
                buttonRipMovie.Enabled = false;
                ExecuteRipProcess();
            }
            
        }

        private void ExecuteRipProcess()
        {
            Ripping.SaveSettings();
            this.WindowState = FormWindowState.Minimized;
            List<object> arguments = new List<object>();
            if (textBoxCurrentTitle.Text != "")
            {
                if (Settings.Default.TempPath != "")
                {
                    arguments.Add(Settings.Default.TempPath +  @"\"  + Settings.Default.CurrentTitle);
                }
                else
                {
                    arguments.Add(Settings.Default.FinalPath + @"\" + Settings.Default.CurrentTitle);
                }
            }
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(arguments[0].ToString());
            }


        }

        /// <summary>
        /// Start timer
        /// </summary>
        public void StartTheTimer()
        {
            if (textBoxCurrentTitle.Text == "")
            {
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            else
            {
                if (Settings.Default.Timeout)
                {
                    // Start the timer.
                    timeLeft = Settings.Default.TimerValue;
                    timeLabel.Text = timeLeft.ToString() + " seconds";
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                }
                //Properties.Settings.Default.Reload();
            }

                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (timeLeft > 0)
            {
                // Display the new time left
                // by updating the Time Left label.
                timeLeft = timeLeft - 1;
                timeLabel.Enabled = true;
                timeLabel.Text = timeLeft + " seconds";
            }
            else
            {
                 ExecuteRipProcess();
            }
        }

        private void FailedCounter_Click(object sender, EventArgs e)
        {

        }

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }

        private void timeout_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Timeout = TimeOutCheckBox.Checked;
            StartTheTimer();
        }

        private void TimeOutValueBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void statusText_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "Republic Wireless":
                    Settings.Default.CurrentProvider = "@text.republicwireless.com";
                    break;
                case "Metro PCS":
                    Settings.Default.CurrentProvider = "@mymetropcs.com";
                    break;
                case "Tracfone":
                    Settings.Default.CurrentProvider = "@mmst5.tracfone.com";
                    break;
                case "Virgin Mobile":
                    Settings.Default.CurrentProvider = "@vmobl.com";
                    break;
                case "Sprint":
                    Settings.Default.CurrentProvider = "@messaging.sprintpcs.com";
                    break;
                case "Verizon":
                    Settings.Default.CurrentProvider = "@vtext.com";
                    break;
                case "T-Mobile":
                    Settings.Default.CurrentProvider = "@tmomail.net";
                    break;
                case "AT&T":
                    Settings.Default.CurrentProvider = "@txt.att.net";
                    break;
                case "Boost Mobile":
                    Settings.Default.CurrentProvider = "@sms.myboostmobile.com";
                    break;
                case "Cricket":
                    Settings.Default.CurrentProvider = "@sms.cricketwireless.net";
                    break;
                case "Google Fi (Project Fi)":
                    Settings.Default.CurrentProvider = "@msg.fi.google.com";
                    break;
                case "U.S.Cellular":
                    Settings.Default.CurrentProvider = "@email.uscc.net";
                    break;
                case "Ting":
                    Settings.Default.CurrentProvider = "@message.ting.com";
                    break;
                case "Consumer Cellular":
                    Settings.Default.CurrentProvider = "@mailmymobile.net";
                    break;
                case "C-Spire":
                    Settings.Default.CurrentProvider = "@cspire1.com";
                    break;
                case "Page Plus":
                    Settings.Default.CurrentProvider = "@vtext.com";
                    break;
                default:
                    Settings.Default.CurrentProvider = "@txt.att.net";
                    break;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void PhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void FromEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void EmailSettingsBox_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Ripping.SaveSettings();
            SMTPSender.Main(true);
            this.Refresh();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ripping.SaveSettings();
        }

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            //This is where the cancel function should go. all others are a lie.
            Process[] processes = Process.GetProcessesByName("makemkvcon64");
            foreach (Process p in processes)
            {
                IntPtr windowHandle = p.MainWindowHandle;
                backgroundWorker1.CancelAsync();
                p.Kill();
            }
            OpenOrCloseCDDrive.Open();
            Thread.Sleep(5000);
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenOrCloseCDDrive.Close();
            Thread.Sleep(5000);
            var DVDDriveToUse = Ripping.GetDriveInfo("drive");
            var CurrentTitle = Ripping.GetDriveInfo("label");
            Settings.Default.CurrentTitle = CurrentTitle;
            Settings.Default.DVDDrive = DVDDriveToUse;
            textBoxCurrentTitle.Text = CurrentTitle;
            dvdDriveID.Text = DVDDriveToUse;
            Ripping.SaveSettings();
            this.Refresh();
            StartTheTimer();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.WorkerSupportsCancellation = true;

            while (textBoxCurrentTitle.Text == "" || textBoxCurrentTitle.Text == null)
            {
                Thread.Sleep(500);
                Ripping.refreshdata();
            }
            StartTheTimer();
        }

        private void toolStripMenuAbout_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.Show();
        }

        private void dvdDriveID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    