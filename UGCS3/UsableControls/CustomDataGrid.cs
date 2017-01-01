using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UGCS3.UsableControls
{
    public partial class CustomDataGrid : UserControl
    {
        public CustomDataGrid()
        {
            InitializeComponent();
        }

        public Dictionary<string, NumericUpDown> paramter_dictionary = new Dictionary<string, NumericUpDown>();
        public List<int> changed_indecies_list = new List<int>();
        //List<NumericUpDown> list_numeric = new List<NumericUpDown>();
        public void Add_Rows(ushort index, string paramid, float paramvalue, int decimalplaces, float increment, float min, float max)
        {
            Panel newP = new Panel();
            Label newL = new Label();
            NumericUpDown numeric = new NumericUpDown();

            newP.Size        = RowPanel.Size;
            newP.BorderStyle = RowPanel.BorderStyle;
            newP.Location    = new Point(RowPanel.Location.X, (RowPanel.Location.Y + RowPanel.Size.Height)*(index+1));

            newL.Size = ParamNameLabel.Size;
            newL.BorderStyle = ParamNameLabel.BorderStyle;
            newL.Location = ParamNameLabel.Location;
            newL.Text = paramid;
            newL.TextAlign = ContentAlignment.MiddleLeft;
            newL.ForeColor = ParamNameLabel.ForeColor;
            newP.Controls.Add(newL);

            numeric.Size = numericUpDown1.Size;
            numeric.Location = numericUpDown1.Location;
            numeric.Minimum = (decimal)min;
            numeric.Maximum = (decimal)max;
            numeric.Increment = (decimal)increment;
            numeric.Value = (decimal)paramvalue;
            numeric.DecimalPlaces = decimalplaces;
            newP.Controls.Add(numeric);
                
            // add a list of numerics
            //list_numeric.Add(numeric);

            //list_numeric[index].ValueChanged += CustomDataGrid_ValueChanged;

            //list_numeric[0].BackColor = Color.Green;
                
            // add everything to a list of dictionary
            try
            {
                paramter_dictionary.Add(paramid, numeric);
                //paramter_dictionary.Values.ToList()[paramter_dictionary.Keys(paramid)];
                paramter_dictionary[paramid].ValueChanged += CustomDataGrid_ValueChanged;
                //paramter_dictionary.Values.ToList()[index].ValueChanged += CustomDataGrid_ValueChanged;
                BorderPanel.Controls.Add(newP);
            }
            catch (SystemException ex)
            {
                Console.WriteLine("Parameter Error: " + ex.Message  + " ; " + "Index " + index) ;
            }
           
        }

        
        public void CustomDataGrid_ValueChanged(object sender, EventArgs e)
        {
            // get index of changed value
            NumericUpDown numeric = sender as NumericUpDown;

            //numeric.BackColor = Color.Green;

            int index = paramter_dictionary.Values.ToList().IndexOf(sender as NumericUpDown);

            paramter_dictionary.Values.ToList()[index].BackColor = Color.Green;

            if (!changed_indecies_list.Contains(index))
            {
                changed_indecies_list.Add(index);
            }

            // numeric.BackColor = Color.White;

            // MessageBox.Show(paramter_dictionary.Values.ToList()[index].Value.ToString());
        }



        // upon writing all parameters, the indeces will be cleared.. make sure to take care of user changing parameter while 
        // the parameters are being writen
        // get all changed indeces -> clear_old_indeces -> write all changed parameters
        public void Clear_Indeces()
        {

            foreach(int index in changed_indecies_list)
            {
                paramter_dictionary.Values.ToList()[index].BackColor = Color.White;
            }


            changed_indecies_list.Clear();

            //numeric.BackColor = Color.White;
        }

    }
}
