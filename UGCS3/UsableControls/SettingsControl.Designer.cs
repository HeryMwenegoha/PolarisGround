namespace UGCS3.UsableControls
{
    partial class SettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.GraphGroupBox = new System.Windows.Forms.GroupBox();
            this.millisecondsLabel = new System.Windows.Forms.Label();
            this.TimeOffsetTextBox = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Antenna_Connectbutton = new System.Windows.Forms.Button();
            this.AntennaComPortcomboBox = new System.Windows.Forms.ComboBox();
            this.DownTiltbutton = new System.Windows.Forms.Button();
            this.RightPanbutton = new System.Windows.Forms.Button();
            this.UpTiltbutton = new System.Windows.Forms.Button();
            this.LeftPanbutton = new System.Windows.Forms.Button();
            this.Parameter_button = new System.Windows.Forms.Button();
            this.log_button = new System.Windows.Forms.Button();
            this.accZ_checkBox = new System.Windows.Forms.CheckBox();
            this.accY_checkBox = new System.Windows.Forms.CheckBox();
            this.accX_checkBox = new System.Windows.Forms.CheckBox();
            this.climbrate_checkBox = new System.Windows.Forms.CheckBox();
            this.altitude_CheckBox = new System.Windows.Forms.CheckBox();
            this.camera_comboBox = new System.Windows.Forms.ComboBox();
            this.camera_button = new System.Windows.Forms.Button();
            this.metadata_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BatteryVoltageTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.BatteryGainTextBox = new System.Windows.Forms.TextBox();
            this.BatteryAnalogTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.YawRateCheckBox = new System.Windows.Forms.CheckBox();
            this.PitchRateCheckBox = new System.Windows.Forms.CheckBox();
            this.RollRateCheckBox = new System.Windows.Forms.CheckBox();
            this.PitchCheckBox = new System.Windows.Forms.CheckBox();
            this.AllRatesCheckBox = new System.Windows.Forms.CheckBox();
            this.RollCheckBox = new System.Windows.Forms.CheckBox();
            this.YawCheckBox = new System.Windows.Forms.CheckBox();
            this.AhrsCheckBox = new System.Windows.Forms.CheckBox();
            this.ZedGraphPane = new ZedGraph.ZedGraphControl();
            this.RadioGroupBox = new System.Windows.Forms.GroupBox();
            this.INS_Calibrate_Button = new System.Windows.Forms.Button();
            this.RadioCalibrateButton = new System.Windows.Forms.Button();
            this.ThrottleYawRadioPanel = new System.Windows.Forms.Panel();
            this.ThrLabel = new System.Windows.Forms.Label();
            this.YawLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.YawProgressBar = new System.Windows.Forms.ProgressBar();
            this.RollPitchRadioPanel = new System.Windows.Forms.Panel();
            this.PtchLabel = new System.Windows.Forms.Label();
            this.RollLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.RollProgressBar = new System.Windows.Forms.ProgressBar();
            this.HilGroupBox = new System.Windows.Forms.GroupBox();
            this.udpconnectButton = new System.Windows.Forms.Button();
            this.startXplaneButton = new System.Windows.Forms.Button();
            this.Hil_Rev_YawCheckBox = new System.Windows.Forms.CheckBox();
            this.Hil_Rev_ThrottleCheckBox = new System.Windows.Forms.CheckBox();
            this.Hil_Rev_PitchCheckBox = new System.Windows.Forms.CheckBox();
            this.Hil_Rev_RollCheckBox = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sndPortTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.FcsIpTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.recPortTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SimIpTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MainPanel.SuspendLayout();
            this.GraphGroupBox.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.RadioGroupBox.SuspendLayout();
            this.ThrottleYawRadioPanel.SuspendLayout();
            this.RollPitchRadioPanel.SuspendLayout();
            this.HilGroupBox.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.GraphGroupBox);
            this.MainPanel.Controls.Add(this.RadioGroupBox);
            this.MainPanel.Controls.Add(this.HilGroupBox);
            this.MainPanel.Location = new System.Drawing.Point(4, 4);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(912, 835);
            this.MainPanel.TabIndex = 0;
            // 
            // GraphGroupBox
            // 
            this.GraphGroupBox.Controls.Add(this.millisecondsLabel);
            this.GraphGroupBox.Controls.Add(this.TimeOffsetTextBox);
            this.GraphGroupBox.Controls.Add(this.panel5);
            this.GraphGroupBox.Controls.Add(this.Parameter_button);
            this.GraphGroupBox.Controls.Add(this.log_button);
            this.GraphGroupBox.Controls.Add(this.accZ_checkBox);
            this.GraphGroupBox.Controls.Add(this.accY_checkBox);
            this.GraphGroupBox.Controls.Add(this.accX_checkBox);
            this.GraphGroupBox.Controls.Add(this.climbrate_checkBox);
            this.GraphGroupBox.Controls.Add(this.altitude_CheckBox);
            this.GraphGroupBox.Controls.Add(this.camera_comboBox);
            this.GraphGroupBox.Controls.Add(this.camera_button);
            this.GraphGroupBox.Controls.Add(this.metadata_button);
            this.GraphGroupBox.Controls.Add(this.groupBox1);
            this.GraphGroupBox.Controls.Add(this.YawRateCheckBox);
            this.GraphGroupBox.Controls.Add(this.PitchRateCheckBox);
            this.GraphGroupBox.Controls.Add(this.RollRateCheckBox);
            this.GraphGroupBox.Controls.Add(this.PitchCheckBox);
            this.GraphGroupBox.Controls.Add(this.AllRatesCheckBox);
            this.GraphGroupBox.Controls.Add(this.RollCheckBox);
            this.GraphGroupBox.Controls.Add(this.YawCheckBox);
            this.GraphGroupBox.Controls.Add(this.AhrsCheckBox);
            this.GraphGroupBox.Controls.Add(this.ZedGraphPane);
            this.GraphGroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.GraphGroupBox.Location = new System.Drawing.Point(4, 214);
            this.GraphGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.GraphGroupBox.Name = "GraphGroupBox";
            this.GraphGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.GraphGroupBox.Size = new System.Drawing.Size(904, 600);
            this.GraphGroupBox.TabIndex = 3;
            this.GraphGroupBox.TabStop = false;
            this.GraphGroupBox.Text = "Graph Plane";
            // 
            // millisecondsLabel
            // 
            this.millisecondsLabel.AutoSize = true;
            this.millisecondsLabel.Location = new System.Drawing.Point(506, 489);
            this.millisecondsLabel.Name = "millisecondsLabel";
            this.millisecondsLabel.Size = new System.Drawing.Size(84, 17);
            this.millisecondsLabel.TabIndex = 15;
            this.millisecondsLabel.Text = "milliseconds";
            // 
            // TimeOffsetTextBox
            // 
            this.TimeOffsetTextBox.Location = new System.Drawing.Point(434, 486);
            this.TimeOffsetTextBox.Name = "TimeOffsetTextBox";
            this.TimeOffsetTextBox.Size = new System.Drawing.Size(66, 22);
            this.TimeOffsetTextBox.TabIndex = 14;
            this.TimeOffsetTextBox.Text = "0 ";
            this.TimeOffsetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Controls.Add(this.Antenna_Connectbutton);
            this.panel5.Controls.Add(this.AntennaComPortcomboBox);
            this.panel5.Controls.Add(this.DownTiltbutton);
            this.panel5.Controls.Add(this.RightPanbutton);
            this.panel5.Controls.Add(this.UpTiltbutton);
            this.panel5.Controls.Add(this.LeftPanbutton);
            this.panel5.Location = new System.Drawing.Point(594, 466);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(303, 80);
            this.panel5.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::UGCS3.Properties.Resources.antenna_tracker_button;
            this.pictureBox1.InitialImage = global::UGCS3.Properties.Resources.antenna_tracker_button;
            this.pictureBox1.Location = new System.Drawing.Point(99, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // Antenna_Connectbutton
            // 
            this.Antenna_Connectbutton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Antenna_Connectbutton.Location = new System.Drawing.Point(184, 43);
            this.Antenna_Connectbutton.Name = "Antenna_Connectbutton";
            this.Antenna_Connectbutton.Size = new System.Drawing.Size(107, 29);
            this.Antenna_Connectbutton.TabIndex = 14;
            this.Antenna_Connectbutton.Text = "Connect";
            this.Antenna_Connectbutton.UseVisualStyleBackColor = true;
            // 
            // AntennaComPortcomboBox
            // 
            this.AntennaComPortcomboBox.FormattingEnabled = true;
            this.AntennaComPortcomboBox.Location = new System.Drawing.Point(183, 5);
            this.AntennaComPortcomboBox.Name = "AntennaComPortcomboBox";
            this.AntennaComPortcomboBox.Size = new System.Drawing.Size(107, 24);
            this.AntennaComPortcomboBox.TabIndex = 15;
            // 
            // DownTiltbutton
            // 
            this.DownTiltbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownTiltbutton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DownTiltbutton.Location = new System.Drawing.Point(29, 50);
            this.DownTiltbutton.Name = "DownTiltbutton";
            this.DownTiltbutton.Size = new System.Drawing.Size(27, 27);
            this.DownTiltbutton.TabIndex = 3;
            this.DownTiltbutton.Text = "V";
            this.DownTiltbutton.UseVisualStyleBackColor = true;
            // 
            // RightPanbutton
            // 
            this.RightPanbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightPanbutton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RightPanbutton.Location = new System.Drawing.Point(60, 24);
            this.RightPanbutton.Name = "RightPanbutton";
            this.RightPanbutton.Size = new System.Drawing.Size(23, 26);
            this.RightPanbutton.TabIndex = 2;
            this.RightPanbutton.Text = ">";
            this.RightPanbutton.UseVisualStyleBackColor = true;
            // 
            // UpTiltbutton
            // 
            this.UpTiltbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpTiltbutton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UpTiltbutton.Location = new System.Drawing.Point(29, 4);
            this.UpTiltbutton.Name = "UpTiltbutton";
            this.UpTiltbutton.Size = new System.Drawing.Size(27, 25);
            this.UpTiltbutton.TabIndex = 1;
            this.UpTiltbutton.Text = "Λ";
            this.UpTiltbutton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.UpTiltbutton.UseVisualStyleBackColor = true;
            // 
            // LeftPanbutton
            // 
            this.LeftPanbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftPanbutton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LeftPanbutton.Location = new System.Drawing.Point(3, 25);
            this.LeftPanbutton.Name = "LeftPanbutton";
            this.LeftPanbutton.Size = new System.Drawing.Size(23, 25);
            this.LeftPanbutton.TabIndex = 0;
            this.LeftPanbutton.Text = "<";
            this.LeftPanbutton.UseVisualStyleBackColor = true;
            // 
            // Parameter_button
            // 
            this.Parameter_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Parameter_button.Location = new System.Drawing.Point(308, 486);
            this.Parameter_button.Name = "Parameter_button";
            this.Parameter_button.Size = new System.Drawing.Size(101, 27);
            this.Parameter_button.TabIndex = 12;
            this.Parameter_button.Text = "Parameter";
            this.Parameter_button.UseVisualStyleBackColor = true;
            // 
            // log_button
            // 
            this.log_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.log_button.Location = new System.Drawing.Point(434, 519);
            this.log_button.Name = "log_button";
            this.log_button.Size = new System.Drawing.Size(101, 27);
            this.log_button.TabIndex = 11;
            this.log_button.Text = "log";
            this.log_button.UseVisualStyleBackColor = true;
            // 
            // accZ_checkBox
            // 
            this.accZ_checkBox.AutoSize = true;
            this.accZ_checkBox.Location = new System.Drawing.Point(317, 467);
            this.accZ_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.accZ_checkBox.Name = "accZ_checkBox";
            this.accZ_checkBox.Size = new System.Drawing.Size(101, 21);
            this.accZ_checkBox.TabIndex = 10;
            this.accZ_checkBox.Text = "accZ(m/s2)";
            this.accZ_checkBox.UseVisualStyleBackColor = true;
            // 
            // accY_checkBox
            // 
            this.accY_checkBox.AutoSize = true;
            this.accY_checkBox.Location = new System.Drawing.Point(149, 467);
            this.accY_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.accY_checkBox.Name = "accY_checkBox";
            this.accY_checkBox.Size = new System.Drawing.Size(101, 21);
            this.accY_checkBox.TabIndex = 9;
            this.accY_checkBox.Text = "accY(m/s2)";
            this.accY_checkBox.UseVisualStyleBackColor = true;
            // 
            // accX_checkBox
            // 
            this.accX_checkBox.AutoSize = true;
            this.accX_checkBox.Location = new System.Drawing.Point(4, 467);
            this.accX_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.accX_checkBox.Name = "accX_checkBox";
            this.accX_checkBox.Size = new System.Drawing.Size(105, 21);
            this.accX_checkBox.TabIndex = 6;
            this.accX_checkBox.Text = "accX (m/s2)";
            this.accX_checkBox.UseVisualStyleBackColor = true;
            // 
            // climbrate_checkBox
            // 
            this.climbrate_checkBox.AutoSize = true;
            this.climbrate_checkBox.Location = new System.Drawing.Point(149, 443);
            this.climbrate_checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.climbrate_checkBox.Name = "climbrate_checkBox";
            this.climbrate_checkBox.Size = new System.Drawing.Size(126, 21);
            this.climbrate_checkBox.TabIndex = 7;
            this.climbrate_checkBox.Text = "climbrate(cm/s)";
            this.climbrate_checkBox.UseVisualStyleBackColor = true;
            // 
            // altitude_CheckBox
            // 
            this.altitude_CheckBox.AutoSize = true;
            this.altitude_CheckBox.Location = new System.Drawing.Point(4, 443);
            this.altitude_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.altitude_CheckBox.Name = "altitude_CheckBox";
            this.altitude_CheckBox.Size = new System.Drawing.Size(67, 21);
            this.altitude_CheckBox.TabIndex = 8;
            this.altitude_CheckBox.Text = "Alt(m)";
            this.altitude_CheckBox.UseVisualStyleBackColor = true;
            // 
            // camera_comboBox
            // 
            this.camera_comboBox.FormattingEnabled = true;
            this.camera_comboBox.Location = new System.Drawing.Point(109, 524);
            this.camera_comboBox.Name = "camera_comboBox";
            this.camera_comboBox.Size = new System.Drawing.Size(151, 24);
            this.camera_comboBox.TabIndex = 5;
            // 
            // camera_button
            // 
            this.camera_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.camera_button.Location = new System.Drawing.Point(4, 519);
            this.camera_button.Name = "camera_button";
            this.camera_button.Size = new System.Drawing.Size(99, 33);
            this.camera_button.TabIndex = 4;
            this.camera_button.Text = "Camera On";
            this.camera_button.UseVisualStyleBackColor = true;
            // 
            // metadata_button
            // 
            this.metadata_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.metadata_button.Location = new System.Drawing.Point(307, 519);
            this.metadata_button.Name = "metadata_button";
            this.metadata_button.Size = new System.Drawing.Size(105, 28);
            this.metadata_button.TabIndex = 3;
            this.metadata_button.Text = "Metadata";
            this.metadata_button.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BatteryVoltageTextBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.BatteryGainTextBox);
            this.groupBox1.Controls.Add(this.BatteryAnalogTextBox);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Location = new System.Drawing.Point(653, 377);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(243, 87);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Battery (Power Module)";
            // 
            // BatteryVoltageTextBox
            // 
            this.BatteryVoltageTextBox.Location = new System.Drawing.Point(177, 15);
            this.BatteryVoltageTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.BatteryVoltageTextBox.Name = "BatteryVoltageTextBox";
            this.BatteryVoltageTextBox.Size = new System.Drawing.Size(61, 22);
            this.BatteryVoltageTextBox.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(148, 21);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 17);
            this.label11.TabIndex = 2;
            this.label11.Text = "V";
            // 
            // BatteryGainTextBox
            // 
            this.BatteryGainTextBox.Location = new System.Drawing.Point(77, 57);
            this.BatteryGainTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.BatteryGainTextBox.Name = "BatteryGainTextBox";
            this.BatteryGainTextBox.Size = new System.Drawing.Size(61, 22);
            this.BatteryGainTextBox.TabIndex = 1;
            // 
            // BatteryAnalogTextBox
            // 
            this.BatteryAnalogTextBox.Location = new System.Drawing.Point(77, 18);
            this.BatteryAnalogTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.BatteryAnalogTextBox.Name = "BatteryAnalogTextBox";
            this.BatteryAnalogTextBox.Size = new System.Drawing.Size(61, 22);
            this.BatteryAnalogTextBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 62);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Gain";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 23);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Incoming";
            // 
            // YawRateCheckBox
            // 
            this.YawRateCheckBox.AutoSize = true;
            this.YawRateCheckBox.Location = new System.Drawing.Point(493, 418);
            this.YawRateCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.YawRateCheckBox.Name = "YawRateCheckBox";
            this.YawRateCheckBox.Size = new System.Drawing.Size(151, 21);
            this.YawRateCheckBox.TabIndex = 1;
            this.YawRateCheckBox.Text = "Yaw Rate (rad/sec)";
            this.YawRateCheckBox.UseVisualStyleBackColor = true;
            // 
            // PitchRateCheckBox
            // 
            this.PitchRateCheckBox.AutoSize = true;
            this.PitchRateCheckBox.Location = new System.Drawing.Point(317, 418);
            this.PitchRateCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.PitchRateCheckBox.Name = "PitchRateCheckBox";
            this.PitchRateCheckBox.Size = new System.Drawing.Size(156, 21);
            this.PitchRateCheckBox.TabIndex = 1;
            this.PitchRateCheckBox.Text = "Pitch Rate (rad/sec)";
            this.PitchRateCheckBox.UseVisualStyleBackColor = true;
            // 
            // RollRateCheckBox
            // 
            this.RollRateCheckBox.AutoSize = true;
            this.RollRateCheckBox.Location = new System.Drawing.Point(149, 418);
            this.RollRateCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.RollRateCheckBox.Name = "RollRateCheckBox";
            this.RollRateCheckBox.Size = new System.Drawing.Size(149, 21);
            this.RollRateCheckBox.TabIndex = 1;
            this.RollRateCheckBox.Text = "Roll Rate (rad/sec)";
            this.RollRateCheckBox.UseVisualStyleBackColor = true;
            // 
            // PitchCheckBox
            // 
            this.PitchCheckBox.AutoSize = true;
            this.PitchCheckBox.Location = new System.Drawing.Point(316, 391);
            this.PitchCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.PitchCheckBox.Name = "PitchCheckBox";
            this.PitchCheckBox.Size = new System.Drawing.Size(96, 21);
            this.PitchCheckBox.TabIndex = 1;
            this.PitchCheckBox.Text = "Pitch (rad)";
            this.PitchCheckBox.UseVisualStyleBackColor = true;
            // 
            // AllRatesCheckBox
            // 
            this.AllRatesCheckBox.AutoSize = true;
            this.AllRatesCheckBox.Location = new System.Drawing.Point(4, 418);
            this.AllRatesCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.AllRatesCheckBox.Name = "AllRatesCheckBox";
            this.AllRatesCheckBox.Size = new System.Drawing.Size(128, 21);
            this.AllRatesCheckBox.TabIndex = 1;
            this.AllRatesCheckBox.Text = "Rates (rad/sec)";
            this.AllRatesCheckBox.UseVisualStyleBackColor = true;
            // 
            // RollCheckBox
            // 
            this.RollCheckBox.AutoSize = true;
            this.RollCheckBox.Location = new System.Drawing.Point(148, 391);
            this.RollCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.RollCheckBox.Name = "RollCheckBox";
            this.RollCheckBox.Size = new System.Drawing.Size(89, 21);
            this.RollCheckBox.TabIndex = 1;
            this.RollCheckBox.Text = "Roll (rad)";
            this.RollCheckBox.UseVisualStyleBackColor = true;
            // 
            // YawCheckBox
            // 
            this.YawCheckBox.AutoSize = true;
            this.YawCheckBox.Location = new System.Drawing.Point(492, 391);
            this.YawCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.YawCheckBox.Name = "YawCheckBox";
            this.YawCheckBox.Size = new System.Drawing.Size(91, 21);
            this.YawCheckBox.TabIndex = 1;
            this.YawCheckBox.Text = "Yaw (rad)";
            this.YawCheckBox.UseVisualStyleBackColor = true;
            // 
            // AhrsCheckBox
            // 
            this.AhrsCheckBox.AutoSize = true;
            this.AhrsCheckBox.Location = new System.Drawing.Point(4, 391);
            this.AhrsCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.AhrsCheckBox.Name = "AhrsCheckBox";
            this.AhrsCheckBox.Size = new System.Drawing.Size(94, 21);
            this.AhrsCheckBox.TabIndex = 1;
            this.AhrsCheckBox.Text = "Ahrs (rad)";
            this.AhrsCheckBox.UseVisualStyleBackColor = true;
            // 
            // ZedGraphPane
            // 
            this.ZedGraphPane.IsShowPointValues = false;
            this.ZedGraphPane.Location = new System.Drawing.Point(4, 23);
            this.ZedGraphPane.Margin = new System.Windows.Forms.Padding(4);
            this.ZedGraphPane.Name = "ZedGraphPane";
            this.ZedGraphPane.PointValueFormat = "G";
            this.ZedGraphPane.Size = new System.Drawing.Size(892, 348);
            this.ZedGraphPane.TabIndex = 0;
            // 
            // RadioGroupBox
            // 
            this.RadioGroupBox.Controls.Add(this.INS_Calibrate_Button);
            this.RadioGroupBox.Controls.Add(this.RadioCalibrateButton);
            this.RadioGroupBox.Controls.Add(this.ThrottleYawRadioPanel);
            this.RadioGroupBox.Controls.Add(this.RollPitchRadioPanel);
            this.RadioGroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.RadioGroupBox.Location = new System.Drawing.Point(407, 4);
            this.RadioGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.RadioGroupBox.Name = "RadioGroupBox";
            this.RadioGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.RadioGroupBox.Size = new System.Drawing.Size(501, 202);
            this.RadioGroupBox.TabIndex = 2;
            this.RadioGroupBox.TabStop = false;
            this.RadioGroupBox.Text = "Radio Setup";
            // 
            // INS_Calibrate_Button
            // 
            this.INS_Calibrate_Button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.INS_Calibrate_Button.Location = new System.Drawing.Point(15, 167);
            this.INS_Calibrate_Button.Margin = new System.Windows.Forms.Padding(4);
            this.INS_Calibrate_Button.Name = "INS_Calibrate_Button";
            this.INS_Calibrate_Button.Size = new System.Drawing.Size(100, 28);
            this.INS_Calibrate_Button.TabIndex = 3;
            this.INS_Calibrate_Button.Text = "ins calibrate";
            this.INS_Calibrate_Button.UseVisualStyleBackColor = true;
            // 
            // RadioCalibrateButton
            // 
            this.RadioCalibrateButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RadioCalibrateButton.Location = new System.Drawing.Point(15, 17);
            this.RadioCalibrateButton.Margin = new System.Windows.Forms.Padding(4);
            this.RadioCalibrateButton.Name = "RadioCalibrateButton";
            this.RadioCalibrateButton.Size = new System.Drawing.Size(100, 28);
            this.RadioCalibrateButton.TabIndex = 2;
            this.RadioCalibrateButton.Text = " calibrate";
            this.RadioCalibrateButton.UseVisualStyleBackColor = true;
            // 
            // ThrottleYawRadioPanel
            // 
            this.ThrottleYawRadioPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ThrottleYawRadioPanel.Controls.Add(this.ThrLabel);
            this.ThrottleYawRadioPanel.Controls.Add(this.YawLabel);
            this.ThrottleYawRadioPanel.Controls.Add(this.label8);
            this.ThrottleYawRadioPanel.Controls.Add(this.label7);
            this.ThrottleYawRadioPanel.Controls.Add(this.YawProgressBar);
            this.ThrottleYawRadioPanel.Location = new System.Drawing.Point(131, 12);
            this.ThrottleYawRadioPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ThrottleYawRadioPanel.Name = "ThrottleYawRadioPanel";
            this.ThrottleYawRadioPanel.Size = new System.Drawing.Size(171, 183);
            this.ThrottleYawRadioPanel.TabIndex = 1;
            // 
            // ThrLabel
            // 
            this.ThrLabel.AutoSize = true;
            this.ThrLabel.Location = new System.Drawing.Point(19, 149);
            this.ThrLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ThrLabel.Name = "ThrLabel";
            this.ThrLabel.Size = new System.Drawing.Size(40, 17);
            this.ThrLabel.TabIndex = 6;
            this.ThrLabel.Text = "1500";
            this.ThrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // YawLabel
            // 
            this.YawLabel.AutoSize = true;
            this.YawLabel.Location = new System.Drawing.Point(19, 37);
            this.YawLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YawLabel.Name = "YawLabel";
            this.YawLabel.Size = new System.Drawing.Size(40, 17);
            this.YawLabel.TabIndex = 5;
            this.YawLabel.Text = "1500";
            this.YawLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(115, 149);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "THR";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(115, 37);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "YAW";
            // 
            // YawProgressBar
            // 
            this.YawProgressBar.Location = new System.Drawing.Point(19, 5);
            this.YawProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.YawProgressBar.MarqueeAnimationSpeed = 50;
            this.YawProgressBar.Maximum = 2250;
            this.YawProgressBar.Minimum = 750;
            this.YawProgressBar.Name = "YawProgressBar";
            this.YawProgressBar.Size = new System.Drawing.Size(133, 28);
            this.YawProgressBar.Step = 50;
            this.YawProgressBar.TabIndex = 0;
            this.YawProgressBar.Value = 1500;
            // 
            // RollPitchRadioPanel
            // 
            this.RollPitchRadioPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RollPitchRadioPanel.Controls.Add(this.PtchLabel);
            this.RollPitchRadioPanel.Controls.Add(this.RollLabel);
            this.RollPitchRadioPanel.Controls.Add(this.label6);
            this.RollPitchRadioPanel.Controls.Add(this.label5);
            this.RollPitchRadioPanel.Controls.Add(this.RollProgressBar);
            this.RollPitchRadioPanel.Location = new System.Drawing.Point(323, 12);
            this.RollPitchRadioPanel.Margin = new System.Windows.Forms.Padding(4);
            this.RollPitchRadioPanel.Name = "RollPitchRadioPanel";
            this.RollPitchRadioPanel.Size = new System.Drawing.Size(171, 183);
            this.RollPitchRadioPanel.TabIndex = 1;
            // 
            // PtchLabel
            // 
            this.PtchLabel.AutoSize = true;
            this.PtchLabel.Location = new System.Drawing.Point(21, 149);
            this.PtchLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PtchLabel.Name = "PtchLabel";
            this.PtchLabel.Size = new System.Drawing.Size(40, 17);
            this.PtchLabel.TabIndex = 5;
            this.PtchLabel.Text = "1500";
            // 
            // RollLabel
            // 
            this.RollLabel.AutoSize = true;
            this.RollLabel.Location = new System.Drawing.Point(21, 37);
            this.RollLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RollLabel.Name = "RollLabel";
            this.RollLabel.Size = new System.Drawing.Size(40, 17);
            this.RollLabel.TabIndex = 4;
            this.RollLabel.Text = "1500";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 149);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "PITCH";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "ROLL";
            // 
            // RollProgressBar
            // 
            this.RollProgressBar.Location = new System.Drawing.Point(17, 5);
            this.RollProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.RollProgressBar.MarqueeAnimationSpeed = 50;
            this.RollProgressBar.Maximum = 2250;
            this.RollProgressBar.Minimum = 750;
            this.RollProgressBar.Name = "RollProgressBar";
            this.RollProgressBar.Size = new System.Drawing.Size(133, 28);
            this.RollProgressBar.Step = 50;
            this.RollProgressBar.TabIndex = 0;
            this.RollProgressBar.Value = 1500;
            // 
            // HilGroupBox
            // 
            this.HilGroupBox.Controls.Add(this.udpconnectButton);
            this.HilGroupBox.Controls.Add(this.startXplaneButton);
            this.HilGroupBox.Controls.Add(this.Hil_Rev_YawCheckBox);
            this.HilGroupBox.Controls.Add(this.Hil_Rev_ThrottleCheckBox);
            this.HilGroupBox.Controls.Add(this.Hil_Rev_PitchCheckBox);
            this.HilGroupBox.Controls.Add(this.Hil_Rev_RollCheckBox);
            this.HilGroupBox.Controls.Add(this.panel4);
            this.HilGroupBox.Controls.Add(this.panel2);
            this.HilGroupBox.Controls.Add(this.panel3);
            this.HilGroupBox.Controls.Add(this.panel1);
            this.HilGroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.HilGroupBox.Location = new System.Drawing.Point(4, 4);
            this.HilGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.HilGroupBox.Name = "HilGroupBox";
            this.HilGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.HilGroupBox.Size = new System.Drawing.Size(393, 203);
            this.HilGroupBox.TabIndex = 1;
            this.HilGroupBox.TabStop = false;
            this.HilGroupBox.Text = "Hardware in Loop";
            // 
            // udpconnectButton
            // 
            this.udpconnectButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.udpconnectButton.Location = new System.Drawing.Point(9, 171);
            this.udpconnectButton.Margin = new System.Windows.Forms.Padding(4);
            this.udpconnectButton.Name = "udpconnectButton";
            this.udpconnectButton.Size = new System.Drawing.Size(129, 28);
            this.udpconnectButton.TabIndex = 4;
            this.udpconnectButton.Text = "udp connect";
            this.udpconnectButton.UseVisualStyleBackColor = true;
            // 
            // startXplaneButton
            // 
            this.startXplaneButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.startXplaneButton.Location = new System.Drawing.Point(141, 171);
            this.startXplaneButton.Margin = new System.Windows.Forms.Padding(4);
            this.startXplaneButton.Name = "startXplaneButton";
            this.startXplaneButton.Size = new System.Drawing.Size(100, 28);
            this.startXplaneButton.TabIndex = 3;
            this.startXplaneButton.Text = "start Xplane";
            this.startXplaneButton.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_YawCheckBox
            // 
            this.Hil_Rev_YawCheckBox.AutoSize = true;
            this.Hil_Rev_YawCheckBox.Location = new System.Drawing.Point(277, 138);
            this.Hil_Rev_YawCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Hil_Rev_YawCheckBox.Name = "Hil_Rev_YawCheckBox";
            this.Hil_Rev_YawCheckBox.Size = new System.Drawing.Size(87, 21);
            this.Hil_Rev_YawCheckBox.TabIndex = 2;
            this.Hil_Rev_YawCheckBox.Text = "Rev_yaw";
            this.Hil_Rev_YawCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_ThrottleCheckBox
            // 
            this.Hil_Rev_ThrottleCheckBox.AutoSize = true;
            this.Hil_Rev_ThrottleCheckBox.Location = new System.Drawing.Point(277, 111);
            this.Hil_Rev_ThrottleCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Hil_Rev_ThrottleCheckBox.Name = "Hil_Rev_ThrottleCheckBox";
            this.Hil_Rev_ThrottleCheckBox.Size = new System.Drawing.Size(107, 21);
            this.Hil_Rev_ThrottleCheckBox.TabIndex = 2;
            this.Hil_Rev_ThrottleCheckBox.Text = "Rev_throttle";
            this.Hil_Rev_ThrottleCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_PitchCheckBox
            // 
            this.Hil_Rev_PitchCheckBox.AutoSize = true;
            this.Hil_Rev_PitchCheckBox.Location = new System.Drawing.Point(277, 55);
            this.Hil_Rev_PitchCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Hil_Rev_PitchCheckBox.Name = "Hil_Rev_PitchCheckBox";
            this.Hil_Rev_PitchCheckBox.Size = new System.Drawing.Size(93, 21);
            this.Hil_Rev_PitchCheckBox.TabIndex = 2;
            this.Hil_Rev_PitchCheckBox.Text = "Rev_pitch";
            this.Hil_Rev_PitchCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_RollCheckBox
            // 
            this.Hil_Rev_RollCheckBox.AutoSize = true;
            this.Hil_Rev_RollCheckBox.Location = new System.Drawing.Point(277, 26);
            this.Hil_Rev_RollCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Hil_Rev_RollCheckBox.Name = "Hil_Rev_RollCheckBox";
            this.Hil_Rev_RollCheckBox.Size = new System.Drawing.Size(82, 21);
            this.Hil_Rev_RollCheckBox.TabIndex = 2;
            this.Hil_Rev_RollCheckBox.Text = "Rev_roll";
            this.Hil_Rev_RollCheckBox.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.sndPortTextBox);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(12, 135);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(222, 29);
            this.panel4.TabIndex = 1;
            // 
            // sndPortTextBox
            // 
            this.sndPortTextBox.Location = new System.Drawing.Point(85, 1);
            this.sndPortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.sndPortTextBox.Name = "sndPortTextBox";
            this.sndPortTextBox.Size = new System.Drawing.Size(132, 22);
            this.sndPortTextBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "sndPort";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.FcsIpTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(12, 105);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(222, 29);
            this.panel2.TabIndex = 1;
            // 
            // FcsIpTextBox
            // 
            this.FcsIpTextBox.Enabled = false;
            this.FcsIpTextBox.Location = new System.Drawing.Point(85, 1);
            this.FcsIpTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.FcsIpTextBox.Name = "FcsIpTextBox";
            this.FcsIpTextBox.Size = new System.Drawing.Size(132, 22);
            this.FcsIpTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "FcsIp";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.recPortTextBox);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(12, 53);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(222, 29);
            this.panel3.TabIndex = 1;
            // 
            // recPortTextBox
            // 
            this.recPortTextBox.Location = new System.Drawing.Point(85, 1);
            this.recPortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.recPortTextBox.Name = "recPortTextBox";
            this.recPortTextBox.Size = new System.Drawing.Size(132, 22);
            this.recPortTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "recPort";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SimIpTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 22);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 29);
            this.panel1.TabIndex = 1;
            // 
            // SimIpTextBox
            // 
            this.SimIpTextBox.Location = new System.Drawing.Point(85, 1);
            this.SimIpTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.SimIpTextBox.Name = "SimIpTextBox";
            this.SimIpTextBox.Size = new System.Drawing.Size(132, 22);
            this.SimIpTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "SimIP";
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(920, 850);
            this.MainPanel.ResumeLayout(false);
            this.GraphGroupBox.ResumeLayout(false);
            this.GraphGroupBox.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.RadioGroupBox.ResumeLayout(false);
            this.ThrottleYawRadioPanel.ResumeLayout(false);
            this.ThrottleYawRadioPanel.PerformLayout();
            this.RollPitchRadioPanel.ResumeLayout(false);
            this.RollPitchRadioPanel.PerformLayout();
            this.HilGroupBox.ResumeLayout(false);
            this.HilGroupBox.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.GroupBox HilGroupBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox Hil_Rev_YawCheckBox;
        private System.Windows.Forms.CheckBox Hil_Rev_ThrottleCheckBox;
        private System.Windows.Forms.CheckBox Hil_Rev_PitchCheckBox;
        private System.Windows.Forms.CheckBox Hil_Rev_RollCheckBox;
        public System.Windows.Forms.Button udpconnectButton;
        public System.Windows.Forms.Button startXplaneButton;
        public System.Windows.Forms.TextBox sndPortTextBox;
        public System.Windows.Forms.TextBox FcsIpTextBox;
        public System.Windows.Forms.TextBox recPortTextBox;
        public System.Windows.Forms.TextBox SimIpTextBox;
        private System.Windows.Forms.GroupBox RadioGroupBox;
        private System.Windows.Forms.ProgressBar RollProgressBar;
        private System.Windows.Forms.Panel RollPitchRadioPanel;
        private System.Windows.Forms.Panel ThrottleYawRadioPanel;
        private System.Windows.Forms.ProgressBar YawProgressBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button RadioCalibrateButton;
        private System.Windows.Forms.Label ThrLabel;
        private System.Windows.Forms.Label YawLabel;
        private System.Windows.Forms.Label PtchLabel;
        private System.Windows.Forms.Label RollLabel;
        private System.Windows.Forms.GroupBox GraphGroupBox;
        private ZedGraph.ZedGraphControl ZedGraphPane;
        private System.Windows.Forms.CheckBox YawRateCheckBox;
        private System.Windows.Forms.CheckBox PitchRateCheckBox;
        private System.Windows.Forms.CheckBox RollRateCheckBox;
        private System.Windows.Forms.CheckBox PitchCheckBox;
        private System.Windows.Forms.CheckBox AllRatesCheckBox;
        private System.Windows.Forms.CheckBox RollCheckBox;
        private System.Windows.Forms.CheckBox YawCheckBox;
        private System.Windows.Forms.CheckBox AhrsCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button INS_Calibrate_Button;
        public System.Windows.Forms.Button metadata_button;
        private System.Windows.Forms.ComboBox camera_comboBox;
        public System.Windows.Forms.Button camera_button;
        private System.Windows.Forms.CheckBox accZ_checkBox;
        private System.Windows.Forms.CheckBox accY_checkBox;
        private System.Windows.Forms.CheckBox accX_checkBox;
        private System.Windows.Forms.CheckBox climbrate_checkBox;
        private System.Windows.Forms.CheckBox altitude_CheckBox;
        private System.Windows.Forms.Button log_button;
        public System.Windows.Forms.Button Parameter_button;
        public System.Windows.Forms.TextBox BatteryVoltageTextBox;
        public System.Windows.Forms.TextBox BatteryGainTextBox;
        public System.Windows.Forms.TextBox BatteryAnalogTextBox;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button Antenna_Connectbutton;
        public System.Windows.Forms.Button DownTiltbutton;
        public System.Windows.Forms.Button RightPanbutton;
        public System.Windows.Forms.Button UpTiltbutton;
        public System.Windows.Forms.Button LeftPanbutton;
        public System.Windows.Forms.ComboBox AntennaComPortcomboBox;
        private System.Windows.Forms.Label millisecondsLabel;
        private System.Windows.Forms.TextBox TimeOffsetTextBox;
    }
}
