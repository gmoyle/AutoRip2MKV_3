using AutoRip2MKV.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            AutoRip2MKV.Ripping.CheckHandBrakeInstall();
            AutoRip2MKV.Ripping.CheckMakeMKVInstall();

            //AutoRip2MKV.Convert.AddTitleToConvvertList();
            //AutoRip2MKV.Convert.ConvertWithHandbrake();

            //FailedCounter.Text = "Failed " + Settings.Default.RipRetry.ToString();
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
            //Task.Run(Convert.ConvertWithHandbrake());
        }

        private void FormMain_FormClosing(object sender, EventArgs e)
        {
            this.Close();
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

        private void ExitThread()
        {
            throw new NotImplementedException();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBoxCurrentTitle_TextChanged(object sender, EventArgs e)
        {

        }

    
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteRipProcess();
        }

        private void ExecuteRipProcess()
        {
            if (textBoxCurrentTitle.Text != "")
            {
                if (Settings.Default.TempPath != "")
                {
                    //this.Hide();
                    AutoRip2MKV.Ripping.Rip2MKV(Settings.Default.TempPath);
                }
                else
                {
                   // this.Hide();
                    Ripping.Rip2MKV(Settings.Default.FinalPath);
                }
                OpenOrCloseCDDrive.Open();
                this.Close();
            }
        }

        /// <summary>
        /// Start timer
        /// </summary>
        public void StartTheTimer()
        {
            if (Settings.Default.Timeout)
            {
                // Start the timer.
                timeLeft = Settings.Default.TimerValue;
                timeLabel.Text = timeLeft + " seconds";
                timer1.Start();
            }
            else
            {
                timer1.Stop();
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
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@text.republicwireless.com";
                    break;
                case "Metro PCS":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@mymetropcs.com";
                    break;
                case "Tracfone":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@mmst5.tracfone.com";
                    break;
                case "Virgin Mobile":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@vmobl.com";
                    break;
                case "Sprint":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@messaging.sprintpcs.com";
                    break;
                case "Verizon":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@vtext.com";
                    break;
                case "T-Mobile":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@tmomail.net";
                    break;
                case "AT&T":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@txt.att.net";
                    break;
                case "Boost Mobile":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@sms.myboostmobile.com";
                    break;
                case "Cricket":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@sms.cricketwireless.net";
                    break;
                case "Google Fi (Project Fi)":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@msg.fi.google.com";
                    break;
                case "U.S.Cellular":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@email.uscc.net";
                    break;
                case "Ting":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@message.ting.com";
                    break;
                case "Consumer Cellular":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@mailmymobile.net";
                    break;
                case "C-Spire":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@cspire1.com";
                    break;
                case "Page Plus":
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@vtext.com";
                    break;
                default:
                    AutoRip2MKV.Properties.Settings.Default.CurrentProvider = "@txt.att.net";
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
            SMTPSender.Main(true);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ripping.SaveSettings();
        }
    }
}
