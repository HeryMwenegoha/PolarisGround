using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO.Ports;

namespace UGCS3.ComPort
{
    public static class SerialConnection
    {
        public static bool Connect(SerialPort SerialPortObject, Button SerialButton,  ComboBox ComPort_ComboBox,  ComboBox BaudRate_ComboBox)
        {
            if (SerialPortObject.IsOpen)
            {
                return false;
            }

            if (ComPort_ComboBox.SelectedIndex == 0)
            {
                throw new Exception("Please select a valid comPort");
            }

            if (BaudRate_ComboBox.SelectedIndex == 0)
            {
                throw new Exception("Please select a valid baudrate");
            }

            string portname = (string)ComPort_ComboBox.Items[ComPort_ComboBox.SelectedIndex];
            int baudrate    = int.Parse((string)BaudRate_ComboBox.Items[BaudRate_ComboBox.SelectedIndex]);

            try
            {
                SerialPortObject.BaudRate = baudrate;
                SerialPortObject.PortName = portname;
                SerialPortObject.Open();
                SerialButton.Image = Properties.Resources.connection_link_on_64x64_;
                ComPort_ComboBox.Enabled = false;
                BaudRate_ComboBox.Enabled = false;
                SerialButton.Enabled = false;

                System.Threading.Thread.Sleep(1000);

                //while(SerialPortObject.BytesToRead > 0)
                //{
                //   Console.WriteLine("Clearing {0} bytes of Serial data", SerialPortObject.BytesToRead);
                //    SerialPortObject.DiscardInBuffer();
                //    SerialPortObject.DiscardOutBuffer();                  
                //}

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool DisConnect(SerialPort SerialPortObject, Button SerialButton,  ComboBox ComPort_ComboBox,  ComboBox BaudRate_ComboBox)
        {
            if (!SerialPortObject.IsOpen)
            {
                return false;
            }
            try
            {
                SerialPortObject.Close();
                SerialButton.Image = Properties.Resources.connection_link_off_64x64_;
                ComPort_ComboBox.Enabled = true;
                BaudRate_ComboBox.Enabled = true;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
