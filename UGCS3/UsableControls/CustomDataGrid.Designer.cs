namespace UGCS3.UsableControls
{
    partial class CustomDataGrid
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
            this.BorderPanel = new System.Windows.Forms.Panel();
            this.RowPanel = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ParamNameLabel = new System.Windows.Forms.Label();
            this.BorderPanel.SuspendLayout();
            this.RowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // BorderPanel
            // 
            this.BorderPanel.AutoScroll = true;
            this.BorderPanel.Controls.Add(this.RowPanel);
            this.BorderPanel.Location = new System.Drawing.Point(2, 2);
            this.BorderPanel.Name = "BorderPanel";
            this.BorderPanel.Size = new System.Drawing.Size(231, 144);
            this.BorderPanel.TabIndex = 0;
            // 
            // RowPanel
            // 
            this.RowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RowPanel.Controls.Add(this.numericUpDown1);
            this.RowPanel.Controls.Add(this.ParamNameLabel);
            this.RowPanel.Location = new System.Drawing.Point(1, 3);
            this.RowPanel.Name = "RowPanel";
            this.RowPanel.Size = new System.Drawing.Size(200, 24);
            this.RowPanel.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(125, 1);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(72, 20);
            this.numericUpDown1.TabIndex = 2;
            // 
            // ParamNameLabel
            // 
            this.ParamNameLabel.AutoSize = true;
            this.ParamNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ParamNameLabel.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ParamNameLabel.Location = new System.Drawing.Point(1, 1);
            this.ParamNameLabel.MaximumSize = new System.Drawing.Size(150, 20);
            this.ParamNameLabel.MinimumSize = new System.Drawing.Size(120, 20);
            this.ParamNameLabel.Name = "ParamNameLabel";
            this.ParamNameLabel.Size = new System.Drawing.Size(120, 20);
            this.ParamNameLabel.TabIndex = 0;
            this.ParamNameLabel.Text = "Parameter ID";
            this.ParamNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CustomDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BorderPanel);
            this.Name = "CustomDataGrid";
            this.Size = new System.Drawing.Size(235, 150);
            this.BorderPanel.ResumeLayout(false);
            this.RowPanel.ResumeLayout(false);
            this.RowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label ParamNameLabel;
        public System.Windows.Forms.Panel RowPanel;
        public System.Windows.Forms.Panel BorderPanel;
    }
}
