using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO;
namespace UGCS3.Log
{
    public static class Log
    {
        public static string _file_path  = "";
        public static string _param_path = "";

        public static void log_console()
        {

        }

        public static void start()
        {         
            string filepath = Application.StartupPath;
            // Folder
            string folder   = filepath + Path.DirectorySeparatorChar + "Logs";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string[] date   = DateTime.Now.ToString().Split('/', ':', '-', ',', ' ');
            int len         = date.Length;

            string filename = "";
            string file     = "";

            for (int i = 1; i < 65355; i++)
            {
                    filename = i.ToString();
                    file     =  folder + Path.DirectorySeparatorChar + "Log" + filename + ".txt";

                    if (!File.Exists(file))
                    {
                        File.Create(file).Close();
                        break;
                    }
            }


            _file_path = file;

            if (!File.Exists(_file_path))
            {
                return;
            }

            string temp = _file_path;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine("Date" + '\t' + "Latitude(deg) " + '\t' + "Longitude(deg)" + '\t' + "hamsl(m)" + '\t' + "hafl(m)" + '\t' +"climbrate(cm/s)"+'\t'+ "GroundSpeed(m/s)" + '\t' + "Airspeed(m/s)" + '\t' + "Mav_Link(%)" + '\t'+ "Rssi_Link(%)" +'\t'+ "Throttle" +'\t'+"Roll(deg)" +'\t'+"Pitch(deg)"+'\t'+"Yaw(deg)");
            }
        }

        static bool log_started = false;
        public static void logwrite(float lat, float lon, float hasl, float altitude, float climbrate, float gspeed, float airspeed, float mav_link, float rssi_link,  float throttle,  float roll, float pitch, float yaw)
        {
            if (log_started == false)
            {
                start();                // start the log directory
                log_started = true;     // only do once
            }

            if (!File.Exists(_file_path))
            {
                return;
            }

            string temp = _file_path;
            using (StreamWriter sw = new StreamWriter(temp,true))
            {
                sw.WriteLine(DateTime.Now.ToLongTimeString() + '\t' + lat + '\t' + lon + '\t' + hasl + '\t' + altitude + '\t' +climbrate+'\t'+ gspeed + '\t' + airspeed + '\t' + mav_link + '\t' + rssi_link + '\t' + throttle + '\t' + roll +'\t' + pitch+'\t' +yaw+'\t');
            }
        }

        static bool param_written = false;
        public static void logparameter(string name, ushort index, float value, int uavid)
        {
            if (param_written == false)
            {
                // Parameter Folder
                string filename = "";
                string file = "";
                string uavtype = "";

                if (uavid >= 0 && uavid < 100)
                {
                    uavtype = "Plane";
                }
                else if (uavid >= 100 && uavid < 200)
                {
                    uavtype = "Copter";
                }
                else if (uavid >= 200 && uavid < 255)
                {
                    uavtype = "Rover";
                }

                string filepath      = Application.StartupPath;
                string _param_folder = filepath + Path.DirectorySeparatorChar + "Parameters" + Path.DirectorySeparatorChar + uavtype;

                if (!Directory.Exists(_param_folder))
                {
                    Directory.CreateDirectory(_param_folder);
                }

                for (int i = 1; i < 65355; i++)
                {
                    filename = i.ToString();
                    file     = _param_folder + Path.DirectorySeparatorChar +"Param_" + filename + ".txt";

                    if (!File.Exists(file))
                    {
                        File.Create(file).Close();
                        break;
                    }
                }

                _param_path = file;

                if (!File.Exists(_param_path))
                {
                    return;
                }

                param_written = true;
            }

            if (!File.Exists(_param_path))
            {
                return;
            }

            string temp = _param_path;
            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine(name + '\t' +index+'\t'+ value);
            }         
        }
    }
}
