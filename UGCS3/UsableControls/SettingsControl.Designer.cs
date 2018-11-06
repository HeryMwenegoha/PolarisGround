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
            this.components = new System.ComponentModel.Container();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.GraphGroupBox = new System.Windows.Forms.GroupBox();
            this.flickerFreePanel1 = new UGCS3.Common.FlickerFreePanel();
            this.label12 = new System.Windows.Forms.Label();
            this.BatteryVoltageTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.BatteryGainTextBox = new System.Windows.Forms.TextBox();
            this.BatteryAnalogTextBox = new System.Windows.Forms.TextBox();
            this.plotLatLng_button = new System.Windows.Forms.Button();
            this.millisecondsLabel = new System.Windows.Forms.Label();
            this.TimeOffsetTextBox = new System.Windows.Forms.TextBox();
            this.Parameter_button = new System.Windows.Forms.Button();
            this.log_button = new System.Windows.Forms.Button();
            this.camera_comboBox = new System.Windows.Forms.ComboBox();
            this.camera_button = new System.Windows.Forms.Button();
            this.metadata_button = new System.Windows.Forms.Button();
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
            this.buttonLog = new System.Windows.Forms.Button();
            this.MainPanel.SuspendLayout();
            this.GraphGroupBox.SuspendLayout();
            this.flickerFreePanel1.SuspendLayout();
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
            this.MainPanel.Location = new System.Drawing.Point(3, 3);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(684, 612);
            this.MainPanel.TabIndex = 0;
            // 
            // GraphGroupBox
            // 
            this.GraphGroupBox.Controls.Add(this.flickerFreePanel1);
            this.GraphGroupBox.Controls.Add(this.plotLatLng_button);
            this.GraphGroupBox.Controls.Add(this.millisecondsLabel);
            this.GraphGroupBox.Controls.Add(this.TimeOffsetTextBox);
            this.GraphGroupBox.Controls.Add(this.Parameter_button);
            this.GraphGroupBox.Controls.Add(this.log_button);
            this.GraphGroupBox.Controls.Add(this.camera_comboBox);
            this.GraphGroupBox.Controls.Add(this.camera_button);
            this.GraphGroupBox.Controls.Add(this.metadata_button);
            this.GraphGroupBox.Controls.Add(this.ZedGraphPane);
            this.GraphGroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.GraphGroupBox.Location = new System.Drawing.Point(3, 171);
            this.GraphGroupBox.Name = "GraphGroupBox";
            this.GraphGroupBox.Size = new System.Drawing.Size(678, 403);
            this.GraphGroupBox.TabIndex = 3;
            this.GraphGroupBox.TabStop = false;
            this.GraphGroupBox.Text = "Graph Plane";
            // 
            // flickerFreePanel1
            // 
            this.flickerFreePanel1.Controls.Add(this.label12);
            this.flickerFreePanel1.Controls.Add(this.BatteryVoltageTextBox);
            this.flickerFreePanel1.Controls.Add(this.label9);
            this.flickerFreePanel1.Controls.Add(this.label11);
            this.flickerFreePanel1.Controls.Add(this.label10);
            this.flickerFreePanel1.Controls.Add(this.BatteryGainTextBox);
            this.flickerFreePanel1.Controls.Add(this.BatteryAnalogTextBox);
            this.flickerFreePanel1.Location = new System.Drawing.Point(468, 311);
            this.flickerFreePanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flickerFreePanel1.Name = "flickerFreePanel1";
            this.flickerFreePanel1.Size = new System.Drawing.Size(204, 81);
            this.flickerFreePanel1.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(61, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Power Module";
            // 
            // BatteryVoltageTextBox
            // 
            this.BatteryVoltageTextBox.Location = new System.Drawing.Point(146, 24);
            this.BatteryVoltageTextBox.Name = "BatteryVoltageTextBox";
            this.BatteryVoltageTextBox.Size = new System.Drawing.Size(47, 20);
            this.BatteryVoltageTextBox.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Incoming";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(125, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "V";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Gain";
            // 
            // BatteryGainTextBox
            // 
            this.BatteryGainTextBox.Location = new System.Drawing.Point(65, 55);
            this.BatteryGainTextBox.Name = "BatteryGainTextBox";
            this.BatteryGainTextBox.Size = new System.Drawing.Size(47, 20);
            this.BatteryGainTextBox.TabIndex = 1;
            // 
            // BatteryAnalogTextBox
            // 
            this.BatteryAnalogTextBox.Location = new System.Drawing.Point(66, 24);
            this.BatteryAnalogTextBox.Name = "BatteryAnalogTextBox";
            this.BatteryAnalogTextBox.Size = new System.Drawing.Size(47, 20);
            this.BatteryAnalogTextBox.TabIndex = 1;
            // 
            // plotLatLng_button
            // 
            this.plotLatLng_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.plotLatLng_button.Location = new System.Drawing.Point(4, 362);
            this.plotLatLng_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.plotLatLng_button.Name = "plotLatLng_button";
            this.plotLatLng_button.Size = new System.Drawing.Size(76, 30);
            this.plotLatLng_button.TabIndex = 16;
            this.plotLatLng_button.Text = "Plot LatLng";
            this.plotLatLng_button.UseVisualStyleBackColor = true;
            // 
            // millisecondsLabel
            // 
            this.millisecondsLabel.AutoSize = true;
            this.millisecondsLabel.Location = new System.Drawing.Point(400, 318);
            this.millisecondsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.millisecondsLabel.Name = "millisecondsLabel";
            this.millisecondsLabel.Size = new System.Drawing.Size(63, 13);
            this.millisecondsLabel.TabIndex = 15;
            this.millisecondsLabel.Text = "milliseconds";
            // 
            // TimeOffsetTextBox
            // 
            this.TimeOffsetTextBox.Location = new System.Drawing.Point(339, 317);
            this.TimeOffsetTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TimeOffsetTextBox.Name = "TimeOffsetTextBox";
            this.TimeOffsetTextBox.Size = new System.Drawing.Size(50, 20);
            this.TimeOffsetTextBox.TabIndex = 14;
            this.TimeOffsetTextBox.Text = "0 ";
            this.TimeOffsetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Parameter_button
            // 
            this.Parameter_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Parameter_button.Location = new System.Drawing.Point(85, 362);
            this.Parameter_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Parameter_button.Name = "Parameter_button";
            this.Parameter_button.Size = new System.Drawing.Size(113, 30);
            this.Parameter_button.TabIndex = 12;
            this.Parameter_button.Text = "Parameter";
            this.Parameter_button.UseVisualStyleBackColor = true;
            // 
            // log_button
            // 
            this.log_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.log_button.Location = new System.Drawing.Point(245, 362);
            this.log_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.log_button.Name = "log_button";
            this.log_button.Size = new System.Drawing.Size(76, 28);
            this.log_button.TabIndex = 11;
            this.log_button.Text = "log";
            this.log_button.UseVisualStyleBackColor = true;
            // 
            // camera_comboBox
            // 
            this.camera_comboBox.FormattingEnabled = true;
            this.camera_comboBox.Location = new System.Drawing.Point(83, 314);
            this.camera_comboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.camera_comboBox.Name = "camera_comboBox";
            this.camera_comboBox.Size = new System.Drawing.Size(114, 21);
            this.camera_comboBox.TabIndex = 5;
            // 
            // camera_button
            // 
            this.camera_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.camera_button.Location = new System.Drawing.Point(4, 314);
            this.camera_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.camera_button.Name = "camera_button";
            this.camera_button.Size = new System.Drawing.Size(74, 27);
            this.camera_button.TabIndex = 4;
            this.camera_button.Text = "Camera On";
            this.camera_button.UseVisualStyleBackColor = true;
            // 
            // metadata_button
            // 
            this.metadata_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.metadata_button.Location = new System.Drawing.Point(245, 314);
            this.metadata_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.metadata_button.Name = "metadata_button";
            this.metadata_button.Size = new System.Drawing.Size(79, 23);
            this.metadata_button.TabIndex = 3;
            this.metadata_button.Text = "Metadata";
            this.metadata_button.UseVisualStyleBackColor = true;
            // 
            // ZedGraphPane
            // 
            this.ZedGraphPane.Location = new System.Drawing.Point(3, 19);
            this.ZedGraphPane.Name = "ZedGraphPane";
            this.ZedGraphPane.ScrollGrace = 0D;
            this.ZedGraphPane.ScrollMaxX = 0D;
            this.ZedGraphPane.ScrollMaxY = 0D;
            this.ZedGraphPane.ScrollMaxY2 = 0D;
            this.ZedGraphPane.ScrollMinX = 0D;
            this.ZedGraphPane.ScrollMinY = 0D;
            this.ZedGraphPane.ScrollMinY2 = 0D;
            this.ZedGraphPane.Size = new System.Drawing.Size(669, 283);
            this.ZedGraphPane.TabIndex = 0;
            this.ZedGraphPane.UseExtendedPrintDialog = true;
            // 
            // RadioGroupBox
            // 
            this.RadioGroupBox.Controls.Add(this.INS_Calibrate_Button);
            this.RadioGroupBox.Controls.Add(this.RadioCalibrateButton);
            this.RadioGroupBox.Controls.Add(this.ThrottleYawRadioPanel);
            this.RadioGroupBox.Controls.Add(this.RollPitchRadioPanel);
            this.RadioGroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.RadioGroupBox.Location = new System.Drawing.Point(305, 3);
            this.RadioGroupBox.Name = "RadioGroupBox";
            this.RadioGroupBox.Size = new System.Drawing.Size(376, 164);
            this.RadioGroupBox.TabIndex = 2;
            this.RadioGroupBox.TabStop = false;
            this.RadioGroupBox.Text = "Radio Setup";
            // 
            // INS_Calibrate_Button
            // 
            this.INS_Calibrate_Button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.INS_Calibrate_Button.Location = new System.Drawing.Point(11, 136);
            this.INS_Calibrate_Button.Name = "INS_Calibrate_Button";
            this.INS_Calibrate_Button.Size = new System.Drawing.Size(75, 23);
            this.INS_Calibrate_Button.TabIndex = 3;
            this.INS_Calibrate_Button.Text = "ins calibrate";
            this.INS_Calibrate_Button.UseVisualStyleBackColor = true;
            // 
            // RadioCalibrateButton
            // 
            this.RadioCalibrateButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RadioCalibrateButton.Location = new System.Drawing.Point(11, 14);
            this.RadioCalibrateButton.Name = "RadioCalibrateButton";
            this.RadioCalibrateButton.Size = new System.Drawing.Size(75, 23);
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
            this.ThrottleYawRadioPanel.Location = new System.Drawing.Point(98, 10);
            this.ThrottleYawRadioPanel.Name = "ThrottleYawRadioPanel";
            this.ThrottleYawRadioPanel.Size = new System.Drawing.Size(129, 149);
            this.ThrottleYawRadioPanel.TabIndex = 1;
            // 
            // ThrLabel
            // 
            this.ThrLabel.AutoSize = true;
            this.ThrLabel.Location = new System.Drawing.Point(10, 121);
            this.ThrLabel.Name = "ThrLabel";
            this.ThrLabel.Size = new System.Drawing.Size(31, 13);
            this.ThrLabel.TabIndex = 6;
            this.ThrLabel.Text = "1500";
            this.ThrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // YawLabel
            // 
            this.YawLabel.AutoSize = true;
            this.YawLabel.Location = new System.Drawing.Point(10, 30);
            this.YawLabel.Name = "YawLabel";
            this.YawLabel.Size = new System.Drawing.Size(31, 13);
            this.YawLabel.TabIndex = 5;
            this.YawLabel.Text = "1500";
            this.YawLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(86, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "THR";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(86, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "YAW";
            // 
            // YawProgressBar
            // 
            this.YawProgressBar.Location = new System.Drawing.Point(14, 4);
            this.YawProgressBar.MarqueeAnimationSpeed = 50;
            this.YawProgressBar.Maximum = 2250;
            this.YawProgressBar.Minimum = 750;
            this.YawProgressBar.Name = "YawProgressBar";
            this.YawProgressBar.Size = new System.Drawing.Size(100, 23);
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
            this.RollPitchRadioPanel.Location = new System.Drawing.Point(242, 10);
            this.RollPitchRadioPanel.Name = "RollPitchRadioPanel";
            this.RollPitchRadioPanel.Size = new System.Drawing.Size(129, 149);
            this.RollPitchRadioPanel.TabIndex = 1;
            // 
            // PtchLabel
            // 
            this.PtchLabel.AutoSize = true;
            this.PtchLabel.Location = new System.Drawing.Point(12, 121);
            this.PtchLabel.Name = "PtchLabel";
            this.PtchLabel.Size = new System.Drawing.Size(31, 13);
            this.PtchLabel.TabIndex = 5;
            this.PtchLabel.Text = "1500";
            // 
            // RollLabel
            // 
            this.RollLabel.AutoSize = true;
            this.RollLabel.Location = new System.Drawing.Point(12, 30);
            this.RollLabel.Name = "RollLabel";
            this.RollLabel.Size = new System.Drawing.Size(31, 13);
            this.RollLabel.TabIndex = 4;
            this.RollLabel.Text = "1500";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "PITCH";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "ROLL";
            // 
            // RollProgressBar
            // 
            this.RollProgressBar.Location = new System.Drawing.Point(13, 4);
            this.RollProgressBar.MarqueeAnimationSpeed = 50;
            this.RollProgressBar.Maximum = 2250;
            this.RollProgressBar.Minimum = 750;
            this.RollProgressBar.Name = "RollProgressBar";
            this.RollProgressBar.Size = new System.Drawing.Size(100, 23);
            this.RollProgressBar.Step = 50;
            this.RollProgressBar.TabIndex = 0;
            this.RollProgressBar.Value = 1500;
            // 
            // HilGroupBox
            // 
            this.HilGroupBox.Controls.Add(this.buttonLog);
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
            this.HilGroupBox.Location = new System.Drawing.Point(3, 3);
            this.HilGroupBox.Name = "HilGroupBox";
            this.HilGroupBox.Size = new System.Drawing.Size(295, 165);
            this.HilGroupBox.TabIndex = 1;
            this.HilGroupBox.TabStop = false;
            this.HilGroupBox.Text = "Hardware in Loop";
            // 
            // udpconnectButton
            // 
            this.udpconnectButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.udpconnectButton.Location = new System.Drawing.Point(7, 139);
            this.udpconnectButton.Name = "udpconnectButton";
            this.udpconnectButton.Size = new System.Drawing.Size(97, 23);
            this.udpconnectButton.TabIndex = 4;
            this.udpconnectButton.Text = "udp connect";
            this.udpconnectButton.UseVisualStyleBackColor = true;
            // 
            // startXplaneButton
            // 
            this.startXplaneButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.startXplaneButton.Location = new System.Drawing.Point(106, 139);
            this.startXplaneButton.Name = "startXplaneButton";
            this.startXplaneButton.Size = new System.Drawing.Size(75, 23);
            this.startXplaneButton.TabIndex = 3;
            this.startXplaneButton.Text = "start Xplane";
            this.startXplaneButton.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_YawCheckBox
            // 
            this.Hil_Rev_YawCheckBox.AutoSize = true;
            this.Hil_Rev_YawCheckBox.Location = new System.Drawing.Point(208, 112);
            this.Hil_Rev_YawCheckBox.Name = "Hil_Rev_YawCheckBox";
            this.Hil_Rev_YawCheckBox.Size = new System.Drawing.Size(71, 17);
            this.Hil_Rev_YawCheckBox.TabIndex = 2;
            this.Hil_Rev_YawCheckBox.Text = "Rev_yaw";
            this.Hil_Rev_YawCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_ThrottleCheckBox
            // 
            this.Hil_Rev_ThrottleCheckBox.AutoSize = true;
            this.Hil_Rev_ThrottleCheckBox.Location = new System.Drawing.Point(208, 90);
            this.Hil_Rev_ThrottleCheckBox.Name = "Hil_Rev_ThrottleCheckBox";
            this.Hil_Rev_ThrottleCheckBox.Size = new System.Drawing.Size(84, 17);
            this.Hil_Rev_ThrottleCheckBox.TabIndex = 2;
            this.Hil_Rev_ThrottleCheckBox.Text = "Rev_throttle";
            this.Hil_Rev_ThrottleCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_PitchCheckBox
            // 
            this.Hil_Rev_PitchCheckBox.AutoSize = true;
            this.Hil_Rev_PitchCheckBox.Location = new System.Drawing.Point(208, 45);
            this.Hil_Rev_PitchCheckBox.Name = "Hil_Rev_PitchCheckBox";
            this.Hil_Rev_PitchCheckBox.Size = new System.Drawing.Size(75, 17);
            this.Hil_Rev_PitchCheckBox.TabIndex = 2;
            this.Hil_Rev_PitchCheckBox.Text = "Rev_pitch";
            this.Hil_Rev_PitchCheckBox.UseVisualStyleBackColor = true;
            // 
            // Hil_Rev_RollCheckBox
            // 
            this.Hil_Rev_RollCheckBox.AutoSize = true;
            this.Hil_Rev_RollCheckBox.Location = new System.Drawing.Point(208, 21);
            this.Hil_Rev_RollCheckBox.Name = "Hil_Rev_RollCheckBox";
            this.Hil_Rev_RollCheckBox.Size = new System.Drawing.Size(65, 17);
            this.Hil_Rev_RollCheckBox.TabIndex = 2;
            this.Hil_Rev_RollCheckBox.Text = "Rev_roll";
            this.Hil_Rev_RollCheckBox.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.sndPortTextBox);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(9, 110);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(167, 24);
            this.panel4.TabIndex = 1;
            // 
            // sndPortTextBox
            // 
            this.sndPortTextBox.Location = new System.Drawing.Point(64, 1);
            this.sndPortTextBox.Name = "sndPortTextBox";
            this.sndPortTextBox.Size = new System.Drawing.Size(100, 20);
            this.sndPortTextBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "sndPort";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.FcsIpTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(9, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(167, 24);
            this.panel2.TabIndex = 1;
            // 
            // FcsIpTextBox
            // 
            this.FcsIpTextBox.Enabled = false;
            this.FcsIpTextBox.Location = new System.Drawing.Point(64, 1);
            this.FcsIpTextBox.Name = "FcsIpTextBox";
            this.FcsIpTextBox.Size = new System.Drawing.Size(100, 20);
            this.FcsIpTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "FcsIp";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.recPortTextBox);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(9, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(167, 24);
            this.panel3.TabIndex = 1;
            // 
            // recPortTextBox
            // 
            this.recPortTextBox.Location = new System.Drawing.Point(64, 1);
            this.recPortTextBox.Name = "recPortTextBox";
            this.recPortTextBox.Size = new System.Drawing.Size(100, 20);
            this.recPortTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "recPort";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SimIpTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 24);
            this.panel1.TabIndex = 1;
            // 
            // SimIpTextBox
            // 
            this.SimIpTextBox.Location = new System.Drawing.Point(64, 1);
            this.SimIpTextBox.Name = "SimIpTextBox";
            this.SimIpTextBox.Size = new System.Drawing.Size(100, 20);
            this.SimIpTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SimIP";
            // 
            // buttonLog
            // 
            this.buttonLog.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonLog.Location = new System.Drawing.Point(208, 139);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(75, 23);
            this.buttonLog.TabIndex = 5;
            this.buttonLog.Text = "startHilLog";
            this.buttonLog.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainPanel);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(690, 576);
            this.MainPanel.ResumeLayout(false);
            this.GraphGroupBox.ResumeLayout(false);
            this.GraphGroupBox.PerformLayout();
            this.flickerFreePanel1.ResumeLayout(false);
            this.flickerFreePanel1.PerformLayout();
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button INS_Calibrate_Button;
        public System.Windows.Forms.Button metadata_button;
        private System.Windows.Forms.ComboBox camera_comboBox;
        public System.Windows.Forms.Button camera_button;
        private System.Windows.Forms.Button log_button;
        public System.Windows.Forms.Button Parameter_button;
        public System.Windows.Forms.TextBox BatteryVoltageTextBox;
        public System.Windows.Forms.TextBox BatteryGainTextBox;
        public System.Windows.Forms.TextBox BatteryAnalogTextBox;
        private System.Windows.Forms.Label millisecondsLabel;
        private System.Windows.Forms.TextBox TimeOffsetTextBox;
        public System.Windows.Forms.Button plotLatLng_button;
        private Common.FlickerFreePanel flickerFreePanel1;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Button buttonLog;
    }
}
