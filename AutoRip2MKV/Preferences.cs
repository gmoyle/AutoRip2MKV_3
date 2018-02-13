using AutoRip2MKV.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
            AutoRip2MKV.Program.CheckHandBrakeInstall();
            AutoRip2MKV.Program.CheckMakeMKVInstall();


           // StartTheTimer();
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

            Settings.Default.Save();
            Settings.Default.Upgrade();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBoxCurrentTitle.Text != "")

            {
                if (timeLeft > 0)
                {
                    // Display the new time left
                    // by updating the Time Left label.
                    timeLeft = timeLeft - 1;
                    timeLabel.Visible = true;
                    timeLabel.Text = timeLeft + " seconds";
                }
                else
                {
                    // If the user ran out of time, stop the timer
                    Properties.Settings.Default.Save(); // Saves settings in application configuration file
                    Properties.Settings.Default.Upgrade();
                    timer1.Stop();

                    if (AutoRip2MKV.Program.CheckVariables())
                    {
                        timeLabel.Text = "AutoRip Executed!";
                        //this.Hide();
                        if (Properties.Settings.Default.TempPath == "")
                        {
                            if (Properties.Settings.Default.FinalPath != "")
                            {
                                Program.Rip2MKV(Properties.Settings.Default.FinalPath);
                            }
                            else
                            {
                                Program.UpdateStatusText("RIP FAILED: SET AT LEAST ONE DESTINATION FOLDER");
                            }

                        }
                        else
                        {
                            Program.Rip2MKV(Properties.Settings.Default.TempPath);
                            AutoConvert.Checked = true;
                            timeLeft = Settings.Default.TimeoutValue;
                            StartTheTimer();
                        }
                    }
                    else
                    {
                        Program.UpdateStatusText("Preferences update. Please review and enable timer");
                        AutoConvert.Checked = false;
                        timeLeft = Settings.Default.TimeoutValue;
                    }
                }
            }
            else
            {
                timer2.Start();

            }
            this.Show();
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

        public void StartTheTimer()
        {
            if (textBoxCurrentTitle != null)
            {
                // Start the timer.
                timeLeft = Settings.Default.TimeoutValue;
                timeLabel.Text = timeLeft + " seconds";
                timer1.Stop();
                timer1.Start();
            }
            else
            {
                timer2.Start();

            }

        }
            private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoConvert.Checked == true)
            {
                timeLabel.Visible = true;
                StartTheTimer();
            }
            else
            {
                timer1.Stop();
                timeLabel.Visible = false;
            }

        }

        private void textBoxCurrentTitle_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCurrentTitle.Text != "")
            {
                StartTheTimer();
            }
            else
            {
                timer2.Start();

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            while (textBoxCurrentTitle.Text == "")
            {
                AutoRip2MKV.Program.GetDriveInfo("drive");
            }

            timer2.Stop();

        }
    }
}
