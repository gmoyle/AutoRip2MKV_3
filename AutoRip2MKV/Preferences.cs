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

            if (textBoxCurrentTitle.Text != "")
            {
                if (Settings.Default.TempPath != "" || Settings.Default.TempPath != null)
                    AutoRip2MKV.Ripping.Rip2MKV(Settings.Default.TempPath);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxCurrentTitle.Text != "")
            {
                Ripping.Rip2MKV(Settings.Default.FinalPath);
            }
        }

    }
}
