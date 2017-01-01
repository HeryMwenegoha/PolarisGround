using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UGCS3.UsableForms
{
    public partial class ParameterForm : Form
    {
        public ParameterForm()
        {
            InitializeComponent();
            this.FormClosing += ParameterForm_FormClosing;
        }

        private void ParameterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                //Console.WriteLine("User has tried to Close the Parameter Form");
                e.Cancel = true;
            }
        }

        public void Initialise_Form()
        {
            this.Text = "Parameters";
            this.BackColor = Color.FromArgb(38, 39, 41);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Show();
            this.TopMost = true;
        }


        public void Display_Parameters(ushort param_index, ushort param_count, string param_name, Point form_location, Size form_Size)
        {
            this.Location = new Point(((form_location.X + form_Size.Width / 2) - this.Size.Width / 2), ((form_location.Y + form_Size.Height / 2) - this.Size.Height / 2));
            this.ParameterIdLabel.Text = param_name;
            this.ParameterNumberLabel.Text = (param_index + 1).ToString();
        }
    }
}
