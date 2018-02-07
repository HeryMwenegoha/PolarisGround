namespace UGCS3
{
    partial class MainView
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
            this.HomeButton = new System.Windows.Forms.Button();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.SerialButton = new System.Windows.Forms.Button();
            this.StatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.BaudRate_ComboBox = new System.Windows.Forms.ComboBox();
            this.ComPort_ComboBox = new System.Windows.Forms.ComboBox();
            this.UploadButton = new System.Windows.Forms.Button();
            this.Flight_Plan_Button = new System.Windows.Forms.Button();
            this.StatusPanel = new UGCS3.Common.FlickerFreePanel();
            this.RadiusNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Do_Action_Button = new System.Windows.Forms.Button();
            this.WriteWayPointsButton = new System.Windows.Forms.Button();
            this.ReadWayPointsButton = new System.Windows.Forms.Button();
            this.WPSeq_ComboBox = new System.Windows.Forms.ComboBox();
            this.ComportStatuslabel = new System.Windows.Forms.Label();
            this.StatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadiusNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // HomeButton
            // 
            this.HomeButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.HomeButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.HomeButton.Location = new System.Drawing.Point(2, 3);
            this.HomeButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(80, 80);
            this.HomeButton.TabIndex = 0;
            this.HomeButton.Text = "Home";
            this.HomeButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.HomeButton.UseVisualStyleBackColor = true;
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SettingsButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.SettingsButton.Location = new System.Drawing.Point(83, 3);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(80, 80);
            this.SettingsButton.TabIndex = 0;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SettingsButton.UseVisualStyleBackColor = true;
            // 
            // SerialButton
            // 
            this.SerialButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SerialButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.SerialButton.Location = new System.Drawing.Point(901, 2);
            this.SerialButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.SerialButton.Name = "SerialButton";
            this.SerialButton.Size = new System.Drawing.Size(80, 80);
            this.SerialButton.TabIndex = 0;
            this.SerialButton.Text = "ComPort";
            this.SerialButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SerialButton.UseVisualStyleBackColor = true;
            // 
            // StatusProgressBar
            // 
            this.StatusProgressBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StatusProgressBar.Location = new System.Drawing.Point(771, 33);
            this.StatusProgressBar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.StatusProgressBar.Name = "StatusProgressBar";
            this.StatusProgressBar.Size = new System.Drawing.Size(120, 21);
            this.StatusProgressBar.TabIndex = 1;
            // 
            // BaudRate_ComboBox
            // 
            this.BaudRate_ComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BaudRate_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaudRate_ComboBox.FormattingEnabled = true;
            this.BaudRate_ComboBox.Location = new System.Drawing.Point(771, 56);
            this.BaudRate_ComboBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.BaudRate_ComboBox.Name = "BaudRate_ComboBox";
            this.BaudRate_ComboBox.Size = new System.Drawing.Size(121, 24);
            this.BaudRate_ComboBox.TabIndex = 0;
            // 
            // ComPort_ComboBox
            // 
            this.ComPort_ComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComPort_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComPort_ComboBox.FormattingEnabled = true;
            this.ComPort_ComboBox.Location = new System.Drawing.Point(771, 7);
            this.ComPort_ComboBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComPort_ComboBox.Name = "ComPort_ComboBox";
            this.ComPort_ComboBox.Size = new System.Drawing.Size(120, 24);
            this.ComPort_ComboBox.TabIndex = 0;
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UploadButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.UploadButton.Location = new System.Drawing.Point(164, 3);
            this.UploadButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(80, 80);
            this.UploadButton.TabIndex = 0;
            this.UploadButton.Text = "Upload";
            this.UploadButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.UploadButton.UseVisualStyleBackColor = true;
            // 
            // Flight_Plan_Button
            // 
            this.Flight_Plan_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Flight_Plan_Button.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Flight_Plan_Button.Location = new System.Drawing.Point(245, 3);
            this.Flight_Plan_Button.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Flight_Plan_Button.Name = "Flight_Plan_Button";
            this.Flight_Plan_Button.Size = new System.Drawing.Size(80, 80);
            this.Flight_Plan_Button.TabIndex = 4;
            this.Flight_Plan_Button.Text = "Flight Plan";
            this.Flight_Plan_Button.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Flight_Plan_Button.UseVisualStyleBackColor = true;
            // 
            // StatusPanel
            // 
            this.StatusPanel.Controls.Add(this.RadiusNumericUpDown);
            this.StatusPanel.Controls.Add(this.label1);
            this.StatusPanel.Controls.Add(this.StatusLabel);
            this.StatusPanel.Controls.Add(this.Do_Action_Button);
            this.StatusPanel.Controls.Add(this.WriteWayPointsButton);
            this.StatusPanel.Controls.Add(this.ReadWayPointsButton);
            this.StatusPanel.Controls.Add(this.WPSeq_ComboBox);
            this.StatusPanel.Controls.Add(this.ComportStatuslabel);
            this.StatusPanel.Location = new System.Drawing.Point(457, 3);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(306, 80);
            this.StatusPanel.TabIndex = 5;
            // 
            // RadiusNumericUpDown
            // 
            this.RadiusNumericUpDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RadiusNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RadiusNumericUpDown.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.RadiusNumericUpDown.Location = new System.Drawing.Point(91, 2);
            this.RadiusNumericUpDown.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.RadiusNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.RadiusNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RadiusNumericUpDown.Name = "RadiusNumericUpDown";
            this.RadiusNumericUpDown.Size = new System.Drawing.Size(91, 22);
            this.RadiusNumericUpDown.TabIndex = 1;
            this.RadiusNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RadiusNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(6, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Radius (m)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.StatusLabel.Location = new System.Drawing.Point(6, 25);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(176, 21);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "Status";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Do_Action_Button
            // 
            this.Do_Action_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Do_Action_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Do_Action_Button.Location = new System.Drawing.Point(203, 51);
            this.Do_Action_Button.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Do_Action_Button.Name = "Do_Action_Button";
            this.Do_Action_Button.Size = new System.Drawing.Size(100, 25);
            this.Do_Action_Button.TabIndex = 4;
            this.Do_Action_Button.Text = "Do Action";
            this.Do_Action_Button.UseVisualStyleBackColor = true;
            // 
            // WriteWayPointsButton
            // 
            this.WriteWayPointsButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.WriteWayPointsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WriteWayPointsButton.Location = new System.Drawing.Point(91, 51);
            this.WriteWayPointsButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.WriteWayPointsButton.Name = "WriteWayPointsButton";
            this.WriteWayPointsButton.Size = new System.Drawing.Size(91, 26);
            this.WriteWayPointsButton.TabIndex = 2;
            this.WriteWayPointsButton.Text = "WriteWP";
            this.WriteWayPointsButton.UseVisualStyleBackColor = true;
            // 
            // ReadWayPointsButton
            // 
            this.ReadWayPointsButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ReadWayPointsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ReadWayPointsButton.Location = new System.Drawing.Point(6, 51);
            this.ReadWayPointsButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ReadWayPointsButton.Name = "ReadWayPointsButton";
            this.ReadWayPointsButton.Size = new System.Drawing.Size(81, 26);
            this.ReadWayPointsButton.TabIndex = 2;
            this.ReadWayPointsButton.Text = "ReadWp";
            this.ReadWayPointsButton.UseVisualStyleBackColor = true;
            // 
            // WPSeq_ComboBox
            // 
            this.WPSeq_ComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.WPSeq_ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WPSeq_ComboBox.FormattingEnabled = true;
            this.WPSeq_ComboBox.Location = new System.Drawing.Point(203, 2);
            this.WPSeq_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.WPSeq_ComboBox.Name = "WPSeq_ComboBox";
            this.WPSeq_ComboBox.Size = new System.Drawing.Size(100, 24);
            this.WPSeq_ComboBox.TabIndex = 3;
            this.WPSeq_ComboBox.Text = "Command";
            // 
            // ComportStatuslabel
            // 
            this.ComportStatuslabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComportStatuslabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ComportStatuslabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ComportStatuslabel.Location = new System.Drawing.Point(203, 28);
            this.ComportStatuslabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ComportStatuslabel.Name = "ComportStatuslabel";
            this.ComportStatuslabel.Size = new System.Drawing.Size(100, 20);
            this.ComportStatuslabel.TabIndex = 0;
            this.ComportStatuslabel.Text = "Port Status";
            this.ComportStatuslabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(982, 953);
            this.Controls.Add(this.StatusPanel);
            this.Controls.Add(this.StatusProgressBar);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.BaudRate_ComboBox);
            this.Controls.Add(this.Flight_Plan_Button);
            this.Controls.Add(this.ComPort_ComboBox);
            this.Controls.Add(this.HomeButton);
            this.Controls.Add(this.SerialButton);
            this.Controls.Add(this.UploadButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.StatusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RadiusNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button HomeButton;
        private System.Windows.Forms.Button SerialButton;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.ComboBox BaudRate_ComboBox;
        private System.Windows.Forms.ComboBox ComPort_ComboBox;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.ProgressBar StatusProgressBar;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.NumericUpDown RadiusNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WriteWayPointsButton;
        private System.Windows.Forms.Button ReadWayPointsButton;
        private System.Windows.Forms.Button Flight_Plan_Button;
        private System.Windows.Forms.Button Do_Action_Button;
        private System.Windows.Forms.ComboBox WPSeq_ComboBox;
        private System.Windows.Forms.Label ComportStatuslabel;
        private Common.FlickerFreePanel StatusPanel;
    }
}

