namespace AutoRip2MKV
{
    partial class Preferences
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Save = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.statusText = new System.Windows.Forms.TextBox();
            this.textBoxCurrentTitle = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dvdDriveID = new System.Windows.Forms.TextBox();
            this.makeMKVPath = new System.Windows.Forms.TextBox();
            this.handbrakeParams = new System.Windows.Forms.TextBox();
            this.tempPath = new System.Windows.Forms.TextBox();
            this.finalPath = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonRipMovie = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timeLabel = new System.Windows.Forms.Label();
            this.FailedCounter = new System.Windows.Forms.Label();
            this.TimeOutCheckBox = new System.Windows.Forms.CheckBox();
            this.TimeOutValueBox = new System.Windows.Forms.TextBox();
            this.RipSettingsBox = new System.Windows.Forms.GroupBox();
            this.EmailSettingsBox = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.EnableTTL = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.PhoneNumber = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.FromEmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RipSettingsBox.SuspendLayout();
            this.EmailSettingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(207, 308);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "&Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.AutoSizeChanged += new System.EventHandler(this.Save_Click);
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(393, 308);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 2;
            this.Close.Text = "&Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.FormMain_FormClosing);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Handbrake Parameters";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Temp Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Final Location";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "MakeMKV Path";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "DVD Drive";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Minimum Title Length";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Current Title";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // statusText
            // 
            this.statusText.AcceptsReturn = true;
            this.statusText.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "StatusText", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.statusText.Location = new System.Drawing.Point(497, 218);
            this.statusText.Multiline = true;
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(475, 113);
            this.statusText.TabIndex = 30;
            this.statusText.Text = global::AutoRip2MKV.Properties.Settings.Default.StatusText;
            this.statusText.TextChanged += new System.EventHandler(this.statusText_TextChanged);
            // 
            // textBoxCurrentTitle
            // 
            this.textBoxCurrentTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "CurrentTitle", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxCurrentTitle.Location = new System.Drawing.Point(125, 20);
            this.textBoxCurrentTitle.Name = "textBoxCurrentTitle";
            this.textBoxCurrentTitle.Size = new System.Drawing.Size(333, 20);
            this.textBoxCurrentTitle.TabIndex = 0;
            this.textBoxCurrentTitle.Text = global::AutoRip2MKV.Properties.Settings.Default.CurrentTitle;
            this.textBoxCurrentTitle.TextChanged += new System.EventHandler(this.textBoxCurrentTitle_TextChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = global::AutoRip2MKV.Properties.Settings.Default.KeepAfterConv;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AutoRip2MKV.Properties.Settings.Default, "KeepAfterConv", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(128, 178);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(142, 17);
            this.checkBox2.TabIndex = 60;
            this.checkBox2.Text = "Keep MKV After Convert";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::AutoRip2MKV.Properties.Settings.Default.ConvWithHandbrake;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AutoRip2MKV.Properties.Settings.Default, "ConvWithHandbrake", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(128, 155);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(141, 17);
            this.checkBox1.TabIndex = 550;
            this.checkBox1.Text = "Convert with Handbrake";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // dvdDriveID
            // 
            this.dvdDriveID.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "DVDDrive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dvdDriveID.Location = new System.Drawing.Point(128, 210);
            this.dvdDriveID.Name = "dvdDriveID";
            this.dvdDriveID.Size = new System.Drawing.Size(333, 20);
            this.dvdDriveID.TabIndex = 70;
            this.dvdDriveID.Text = global::AutoRip2MKV.Properties.Settings.Default.DVDDrive;
            // 
            // makeMKVPath
            // 
            this.makeMKVPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "MakeMKVPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.makeMKVPath.Location = new System.Drawing.Point(128, 236);
            this.makeMKVPath.Name = "makeMKVPath";
            this.makeMKVPath.Size = new System.Drawing.Size(333, 20);
            this.makeMKVPath.TabIndex = 80;
            this.makeMKVPath.Text = global::AutoRip2MKV.Properties.Settings.Default.MakeMKVPath;
            // 
            // handbrakeParams
            // 
            this.handbrakeParams.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "HandBrakeParameters", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.handbrakeParams.Location = new System.Drawing.Point(125, 50);
            this.handbrakeParams.Name = "handbrakeParams";
            this.handbrakeParams.Size = new System.Drawing.Size(333, 20);
            this.handbrakeParams.TabIndex = 10;
            this.handbrakeParams.Text = global::AutoRip2MKV.Properties.Settings.Default.HandBrakeParameters;
            // 
            // tempPath
            // 
            this.tempPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "TempPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tempPath.Location = new System.Drawing.Point(125, 76);
            this.tempPath.Name = "tempPath";
            this.tempPath.Size = new System.Drawing.Size(333, 20);
            this.tempPath.TabIndex = 20;
            this.tempPath.Text = global::AutoRip2MKV.Properties.Settings.Default.TempPath;
            // 
            // finalPath
            // 
            this.finalPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "FinalPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.finalPath.Location = new System.Drawing.Point(125, 102);
            this.finalPath.Name = "finalPath";
            this.finalPath.Size = new System.Drawing.Size(333, 20);
            this.finalPath.TabIndex = 30;
            this.finalPath.Text = global::AutoRip2MKV.Properties.Settings.Default.FinalPath;
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "MinTitleLength", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox2.Location = new System.Drawing.Point(125, 128);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(333, 20);
            this.textBox2.TabIndex = 40;
            this.textBox2.Text = global::AutoRip2MKV.Properties.Settings.Default.MinTitleLength;
            // 
            // buttonRipMovie
            // 
            this.buttonRipMovie.Location = new System.Drawing.Point(54, 308);
            this.buttonRipMovie.Name = "buttonRipMovie";
            this.buttonRipMovie.Size = new System.Drawing.Size(75, 23);
            this.buttonRipMovie.TabIndex = 31;
            this.buttonRipMovie.Text = "Rip Movie";
            this.buttonRipMovie.UseVisualStyleBackColor = true;
            this.buttonRipMovie.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(326, 263);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(61, 13);
            this.timeLabel.TabIndex = 32;
            this.timeLabel.Text = "Countdown";
            this.timeLabel.Click += new System.EventHandler(this.timeLabel_Click);
            // 
            // FailedCounter
            // 
            this.FailedCounter.AutoSize = true;
            this.FailedCounter.Enabled = false;
            this.FailedCounter.Location = new System.Drawing.Point(817, 199);
            this.FailedCounter.Name = "FailedCounter";
            this.FailedCounter.Size = new System.Drawing.Size(93, 13);
            this.FailedCounter.TabIndex = 33;
            this.FailedCounter.Text = "FailedCounterText";
            this.FailedCounter.Visible = false;
            this.FailedCounter.TextChanged += new System.EventHandler(this.Form1_Load);
            this.FailedCounter.Click += new System.EventHandler(this.FailedCounter_Click);
            // 
            // TimeOutCheckBox
            // 
            this.TimeOutCheckBox.AutoSize = true;
            this.TimeOutCheckBox.Checked = global::AutoRip2MKV.Properties.Settings.Default.Timeout;
            this.TimeOutCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AutoRip2MKV.Properties.Settings.Default, "Timeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TimeOutCheckBox.Location = new System.Drawing.Point(232, 262);
            this.TimeOutCheckBox.Name = "TimeOutCheckBox";
            this.TimeOutCheckBox.Size = new System.Drawing.Size(88, 17);
            this.TimeOutCheckBox.TabIndex = 100;
            this.TimeOutCheckBox.Text = "Enable Timer";
            this.TimeOutCheckBox.UseVisualStyleBackColor = true;
            this.TimeOutCheckBox.CheckedChanged += new System.EventHandler(this.timeout_CheckedChanged);
            // 
            // TimeOutValueBox
            // 
            this.TimeOutValueBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "TimerValue", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TimeOutValueBox.Location = new System.Drawing.Point(197, 260);
            this.TimeOutValueBox.Name = "TimeOutValueBox";
            this.TimeOutValueBox.Size = new System.Drawing.Size(29, 20);
            this.TimeOutValueBox.TabIndex = 90;
            this.TimeOutValueBox.Text = "30";
            this.TimeOutValueBox.TextChanged += new System.EventHandler(this.TimeOutValueBox_TextChanged);
            // 
            // RipSettingsBox
            // 
            this.RipSettingsBox.Controls.Add(this.timeLabel);
            this.RipSettingsBox.Controls.Add(this.TimeOutValueBox);
            this.RipSettingsBox.Controls.Add(this.TimeOutCheckBox);
            this.RipSettingsBox.Controls.Add(this.textBoxCurrentTitle);
            this.RipSettingsBox.Controls.Add(this.makeMKVPath);
            this.RipSettingsBox.Controls.Add(this.label4);
            this.RipSettingsBox.Controls.Add(this.textBox2);
            this.RipSettingsBox.Controls.Add(this.checkBox2);
            this.RipSettingsBox.Controls.Add(this.finalPath);
            this.RipSettingsBox.Controls.Add(this.checkBox1);
            this.RipSettingsBox.Controls.Add(this.tempPath);
            this.RipSettingsBox.Controls.Add(this.label8);
            this.RipSettingsBox.Controls.Add(this.handbrakeParams);
            this.RipSettingsBox.Controls.Add(this.label7);
            this.RipSettingsBox.Controls.Add(this.dvdDriveID);
            this.RipSettingsBox.Controls.Add(this.label6);
            this.RipSettingsBox.Controls.Add(this.label1);
            this.RipSettingsBox.Controls.Add(this.label3);
            this.RipSettingsBox.Controls.Add(this.label2);
            this.RipSettingsBox.Location = new System.Drawing.Point(10, 12);
            this.RipSettingsBox.Name = "RipSettingsBox";
            this.RipSettingsBox.Size = new System.Drawing.Size(467, 288);
            this.RipSettingsBox.TabIndex = 36;
            this.RipSettingsBox.TabStop = false;
            this.RipSettingsBox.Text = "Rip  Settings";
            // 
            // EmailSettingsBox
            // 
            this.EmailSettingsBox.Controls.Add(this.button2);
            this.EmailSettingsBox.Controls.Add(this.button1);
            this.EmailSettingsBox.Controls.Add(this.textBox3);
            this.EmailSettingsBox.Controls.Add(this.label13);
            this.EmailSettingsBox.Controls.Add(this.textBox4);
            this.EmailSettingsBox.Controls.Add(this.EnableTTL);
            this.EmailSettingsBox.Controls.Add(this.label12);
            this.EmailSettingsBox.Controls.Add(this.label11);
            this.EmailSettingsBox.Controls.Add(this.textBox1);
            this.EmailSettingsBox.Controls.Add(this.PhoneNumber);
            this.EmailSettingsBox.Controls.Add(this.comboBox1);
            this.EmailSettingsBox.Controls.Add(this.label10);
            this.EmailSettingsBox.Controls.Add(this.label9);
            this.EmailSettingsBox.Controls.Add(this.FromEmail);
            this.EmailSettingsBox.Location = new System.Drawing.Point(497, 12);
            this.EmailSettingsBox.Name = "EmailSettingsBox";
            this.EmailSettingsBox.Size = new System.Drawing.Size(475, 184);
            this.EmailSettingsBox.TabIndex = 37;
            this.EmailSettingsBox.TabStop = false;
            this.EmailSettingsBox.Text = "Email Settings";
            this.EmailSettingsBox.Enter += new System.EventHandler(this.EmailSettingsBox_Enter);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(419, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "&Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(419, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 39);
            this.button1.TabIndex = 564;
            this.button1.Text = "Test Email";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "SMTPPass", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Location = new System.Drawing.Point(107, 107);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(306, 20);
            this.textBox3.TabIndex = 563;
            this.textBox3.Text = global::AutoRip2MKV.Properties.Settings.Default.SMTPPass;
            this.textBox3.UseSystemPasswordChar = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(26, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 13);
            this.label13.TabIndex = 562;
            this.label13.Text = "SMTP Address";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "SMTPAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(107, 133);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(306, 20);
            this.textBox4.TabIndex = 561;
            this.textBox4.Text = global::AutoRip2MKV.Properties.Settings.Default.SMTPAddress;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // EnableTTL
            // 
            this.EnableTTL.AutoSize = true;
            this.EnableTTL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EnableTTL.Checked = global::AutoRip2MKV.Properties.Settings.Default.EnableTTL;
            this.EnableTTL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableTTL.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::AutoRip2MKV.Properties.Settings.Default, "EnableTTL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EnableTTL.Location = new System.Drawing.Point(331, 159);
            this.EnableTTL.Name = "EnableTTL";
            this.EnableTTL.Size = new System.Drawing.Size(82, 17);
            this.EnableTTL.TabIndex = 560;
            this.EnableTTL.Text = "Enable TTL";
            this.EnableTTL.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 558;
            this.label12.Text = "SMTP Password";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(39, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 557;
            this.label11.Text = "SMTP User";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "SMTPUser", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(107, 81);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(306, 20);
            this.textBox1.TabIndex = 555;
            this.textBox1.Text = global::AutoRip2MKV.Properties.Settings.Default.SMTPUser;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // PhoneNumber
            // 
            this.PhoneNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "PhoneNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PhoneNumber.Location = new System.Drawing.Point(107, 54);
            this.PhoneNumber.Name = "PhoneNumber";
            this.PhoneNumber.Size = new System.Drawing.Size(136, 20);
            this.PhoneNumber.TabIndex = 554;
            this.PhoneNumber.Text = global::AutoRip2MKV.Properties.Settings.Default.PhoneNumber;
            this.PhoneNumber.TextChanged += new System.EventHandler(this.PhoneNumber_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteCustomSource.AddRange(new string[] {
            "AT&T",
            "T-Mobile",
            "Verizon",
            "Sprint",
            "Virgin Mobile",
            "Tracfone",
            "Metro PCS",
            "Boost Mobile",
            "Cricket",
            "Republic Wireless",
            "Google Fi (Project Fi).com",
            "U.S. Cellular",
            "Ting",
            "Consumer Cellular",
            "C-Spire",
            "Page Plus"});
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "CurrentProvider", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.comboBox1.DisplayMember = "comboBox1";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "AT&T",
            "T-Mobile",
            "Verizon",
            "Sprint",
            "Virgin Mobile",
            "Tracfone",
            "Metro PCS",
            "Boost Mobile",
            "Cricket",
            "Republic Wireless",
            "Google Fi (Project Fi).com",
            "U.S. Cellular",
            "Ting",
            "Consumer Cellular",
            "C-Spire",
            "Page Plus"});
            this.comboBox1.Location = new System.Drawing.Point(249, 54);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(164, 21);
            this.comboBox1.TabIndex = 553;
            this.comboBox1.Text = global::AutoRip2MKV.Properties.Settings.Default.CurrentProvider;
            this.comboBox1.ValueMember = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 552;
            this.label10.Text = "PhoneNumber";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(71, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 551;
            this.label9.Text = "From";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FromEmail
            // 
            this.FromEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AutoRip2MKV.Properties.Settings.Default, "FromEmail", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FromEmail.Location = new System.Drawing.Point(107, 28);
            this.FromEmail.Name = "FromEmail";
            this.FromEmail.Size = new System.Drawing.Size(306, 20);
            this.FromEmail.TabIndex = 0;
            this.FromEmail.Text = global::AutoRip2MKV.Properties.Settings.Default.FromEmail;
            this.FromEmail.TextChanged += new System.EventHandler(this.FromEmail_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(497, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Logs";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 358);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FailedCounter);
            this.Controls.Add(this.EmailSettingsBox);
            this.Controls.Add(this.buttonRipMovie);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.RipSettingsBox);
            this.DataBindings.Add(new System.Windows.Forms.Binding("MaximumSize", global::AutoRip2MKV.Properties.Settings.Default, "WindowSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MaximumSize = global::AutoRip2MKV.Properties.Settings.Default.WindowSize;
            this.Name = "Preferences";
            this.Text = "AutoRip2MKV Preferences";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.RipSettingsBox.ResumeLayout(false);
            this.RipSettingsBox.PerformLayout();
            this.EmailSettingsBox.ResumeLayout(false);
            this.EmailSettingsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Save;
        private new System.Windows.Forms.Button Close;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox finalPath;
        private System.Windows.Forms.TextBox tempPath;
        private System.Windows.Forms.TextBox handbrakeParams;
        private System.Windows.Forms.TextBox makeMKVPath;
        private System.Windows.Forms.TextBox dvdDriveID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCurrentTitle;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.Button buttonRipMovie;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label FailedCounter;
        private System.Windows.Forms.CheckBox TimeOutCheckBox;
        private System.Windows.Forms.TextBox TimeOutValueBox;
        private System.Windows.Forms.GroupBox RipSettingsBox;
        private System.Windows.Forms.GroupBox EmailSettingsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FromEmail;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox PhoneNumber;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox EnableTTL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}