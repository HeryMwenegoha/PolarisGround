namespace UGCS3.UsableForms
{
    partial class ParameterForm
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
            this.ParameterIdLabel = new System.Windows.Forms.Label();
            this.ParameterNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ParameterIdLabel
            // 
            this.ParameterIdLabel.AutoSize = true;
            this.ParameterIdLabel.BackColor = System.Drawing.Color.Transparent;
            this.ParameterIdLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ParameterIdLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ParameterIdLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ParameterIdLabel.Location = new System.Drawing.Point(99, 31);
            this.ParameterIdLabel.MaximumSize = new System.Drawing.Size(200, 100);
            this.ParameterIdLabel.MinimumSize = new System.Drawing.Size(150, 20);
            this.ParameterIdLabel.Name = "ParameterIdLabel";
            this.ParameterIdLabel.Size = new System.Drawing.Size(150, 20);
            this.ParameterIdLabel.TabIndex = 0;
            this.ParameterIdLabel.Text = "Parameter Id";
            this.ParameterIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParameterNumberLabel
            // 
            this.ParameterNumberLabel.AutoSize = true;
            this.ParameterNumberLabel.BackColor = System.Drawing.Color.Transparent;
            this.ParameterNumberLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ParameterNumberLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ParameterNumberLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ParameterNumberLabel.Location = new System.Drawing.Point(143, 62);
            this.ParameterNumberLabel.MaximumSize = new System.Drawing.Size(200, 100);
            this.ParameterNumberLabel.MinimumSize = new System.Drawing.Size(60, 20);
            this.ParameterNumberLabel.Name = "ParameterNumberLabel";
            this.ParameterNumberLabel.Size = new System.Drawing.Size(60, 20);
            this.ParameterNumberLabel.TabIndex = 0;
            this.ParameterNumberLabel.Text = "Index";
            this.ParameterNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 106);
            this.Controls.Add(this.ParameterNumberLabel);
            this.Controls.Add(this.ParameterIdLabel);
            this.Name = "ParameterForm";
            this.Text = "ParameterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label ParameterIdLabel;
        public System.Windows.Forms.Label ParameterNumberLabel;

    }
}