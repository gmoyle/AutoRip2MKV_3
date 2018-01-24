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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // AllocConsole();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            sender = AutoRip2MKV.Properties.Settings.Default.MakeMKVPath;
        }

        //[DllImport("kernel32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        // static extern bool AllocConsole();
    }
}
