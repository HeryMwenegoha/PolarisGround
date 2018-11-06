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

        /*        
        1.      "Time(hh:mm:ss:mmm)" + '\t' + 
        2.      "Latitude(deg)" + '\t' + 
        3.      "Longitude(deg)" + '\t' + 
        4.      "hamsl(m)" + '\t' + 
        5.      "hafl(m)" + '\t' + 
        6.      "climbrate(cm/s)" + '\t' + 
        7.      "GroundSpeed(m/s)" + '\t' + 
        8.      "Airspeed(m/s)" + '\t' + 
        9.      "Mav_Link(%)" + '\t' + 
        10.     "Rssi_Link(%)" + '\t' + 
        11.     "Throttle" + '\t' + 
        12.     "Roll(deg)" + '\t' + 
        13.     "Pitch(deg)" + '\t' + 
        14.     "Yaw(deg)" + '\t' + 
        15.     "Rollrate(dps)" + '\t' + 
        16.     "Pitchrate(dps)" +'\t' + 
        17.     "Yawrate(dps)" +'\t' + 
        18.     "accelX(m/s/s)" + '\t' + 
        19.     "accelY(m/s/s)" + '\t' + 
        20.     "accelZ(m/s/s)" + '\t' + 
        21.     "vgN(mps)" + '\t' + 
        22.     "vgE(mps)" + '\t' + 
        23.     "vgD(mps)" +'\t' + 
        24.     "Aileron(unit)" + '\t' + 
        25.     "Elevator(unit)" + '\t' + 
        26.     "Rudder(unit)" + '\t' + 
        27.     "Throttle(unit)");
         */

        public static string _file_path  = "";
        public static string _mat_file_path = "";
        public static string _param_path = "";

        public static string _hil_file_path = "";
        public static string _hil_mat_file_path = "";

        public static void log_console()
        {

        }

        public static void start()
        {         
            string filepath = Application.StartupPath;
            
            // Folder
            string folder   = filepath + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + "Flight";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // date
            string[] date = DateTime.Now.ToShortDateString().Split('/', ':', '-', ',', ' ');
            string _date = "";
            for (int i = 0; i < date.Length; i++)
            {
                _date += date[i] + "_";
            }


            // create file and matfile
            string filename = "";
            string file     = "";
            string matfile = "";
            for (int i = 1; i < 65355; i++)
            {
                filename = i.ToString();
                file = folder + Path.DirectorySeparatorChar + _date + "Log" + filename + ".dat";
                matfile = folder + Path.DirectorySeparatorChar + _date + "mlog" + filename + ".dat";

                if (!File.Exists(file))
                {
                    File.Create(file).Close();
                    break;
                }

                System.Threading.Thread.Sleep(10);

                if (!File.Exists(matfile))
                {
                    File.Create(matfile).Close();
                    break;
                }
            }


            _file_path = file;
            _mat_file_path = matfile;

            if (!File.Exists(_file_path))
            {
                return;
            }

            if (!File.Exists(_mat_file_path))
            {
                return;
            }

            string temp = _file_path;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine("Time(hh:mm:ss:mmm)" + '\t' + "Latitude(deg) " + '\t' + "Longitude(deg)" + '\t' + "hamsl(m)" + '\t' + "hafl(m)" + '\t' +"climbrate(cm/s)"+'\t'+ "GroundSpeed(m/s)" + '\t' + "Airspeed(m/s)" + '\t' + "Mav_Link(%)" + '\t'+ "Rssi_Link(%)" +'\t'+ "Throttle" +'\t'+"Roll(deg)" +'\t'+"Pitch(deg)"+'\t'+"Yaw(deg)");
            }
        }


        public static void startHil(uint folderType)
        {
            string filepath = Application.StartupPath;
            // Folder
            string fold = "";
            if (folderType == 1)
                fold = "Flight";
            else
                fold = "Hil";

            string folder = filepath + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + fold;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string[] date = DateTime.Now.ToShortDateString().Split('/', ':', '-', ',', ' ');
            string _date = "";
            //_date = date[0] + "_" + date[1] + "_" + date[2];

            for(int i = 0; i < date.Length; i++)
            {
                _date += date[i] + "_";
            }


            // essentially the program creates 2 logs a normal log and a matlab specific log.
            string filename = "";
            string file = "";
            string matfile = "";

            for (int i = 1; i < 65355; i++)
            {
                filename = i.ToString();
                file     = folder + Path.DirectorySeparatorChar + _date + "log" + filename + ".dat";
                matfile  = folder + Path.DirectorySeparatorChar + _date + "mlog" + filename + ".dat";

                if (!File.Exists(file))
                {
                    File.Create(file).Close(); // create file
                    break;
                }

                if (!File.Exists(matfile))
                {
                    File.Create(matfile).Close(); // create matfile
                    break;
                }
            }

            // update hil file paths
            _hil_file_path = file;
            _hil_mat_file_path = matfile;
 

            if (!File.Exists(file))
            {
                return; // make sure files exist
            }

            if(!File.Exists(matfile))
            {
                return; // make sure matfile exists.
            }

            string temp = file;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine("Time(hh:mm:ss:mmm)" + '\t' + 
                             "Latitude(deg)" + '\t' + 
                             "Longitude(deg)" + '\t' + 
                             "hamsl(m)" + '\t' + 
                             "hafl(m)" + '\t' + 
                             "climbrate(cm/s)" + '\t' + 
                             "GroundSpeed(m/s)" + '\t' + 
                             "Airspeed(m/s)" + '\t' + 
                             "Mav_Link(%)" + '\t' + 
                             "Rssi_Link(%)" + '\t' + 
                             "Throttle" + '\t' + 
                             "Roll(deg)" + '\t' + 
                             "Pitch(deg)" + '\t' + 
                             "Yaw(deg)" + '\t' + 
                             "Rollrate(dps)" + '\t' + 
                             "Pitchrate(dps)" +'\t' + 
                             "Yawrate(dps)" +'\t' + 
                             "accelX(m/s/s)" + '\t' + 
                             "accelY(m/s/s)" + '\t' + 
                             "accelZ(m/s/s)" + '\t' + 
                             "vgN(mps)" + '\t' + 
                             "vgE(mps)" + '\t' + 
                             "vgD(mps)" +'\t' + 
                             "Aileron(unit)" + '\t' + 
                             "Elevator(unit)" + '\t' + 
                             "Rudder(unit)" + '\t' + 
                             "Throttle(unit)");
            }
        }



        static bool log_started = false; // need to be reset when comport disconnects
        public static void logwrite(float lat, float lon, float hasl, float altitude, float climbrate, float gspeed, float airspeed, float mav_link, float rssi_link,  float throttle,  float roll, float pitch, float yaw)
        {
            if (log_started == false)
            {
                start();                // start the log directory
                log_started = true;     // only do once
            }

            // Log File 
            if (!File.Exists(_file_path))
            {
                return;
            }

            string temp = _file_path;

            using (StreamWriter sw = new StreamWriter(temp,true))
            {
                sw.WriteLine(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond + '\t' + lat + '\t' + lon + '\t' + hasl + '\t' + altitude + '\t' +climbrate+'\t'+ gspeed + '\t' + airspeed + '\t' + mav_link + '\t' + rssi_link + '\t' + throttle + '\t' + roll +'\t' + pitch+'\t' +yaw+'\t');
            }

            // Matfile
            if (!File.Exists(_mat_file_path))
            {
                return;
            }

            temp = _mat_file_path;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond + '\t' + lat + '\t' + lon + '\t' + hasl + '\t' + altitude + '\t' + climbrate + '\t' + gspeed + '\t' + airspeed + '\t' + mav_link + '\t' + rssi_link + '\t' + throttle + '\t' + roll + '\t' + pitch + '\t' + yaw + '\t');
            }
        }

        static bool hil_log_started = false;

        public static void logwritehil(
                    float lat,
                    float lon,
                    float hasl,
                    float altitude,
                    float climbrate,
                    float gspeed,
                    float airspeed,
                    float mav_link,
                    float rssi_link,
                    float throttle,
                    float roll,
                    float pitch,
                    float yaw,
                    float rollrate,
                    float pitchrate,
                    float yawrate,
                    float accelX,
                    float accelY,
                    float accelZ,
                    float vgN,
                    float vgE,
                    float vgD,
                    float daileron,
                    float delevator,
                    float drudder,
                    float dthrottle
                    )
        {
            if (hil_log_started == false)
            {
                startHil(2);                // start the log directory
                hil_log_started = true;     // only do once --> reset when serial port disconnects
            }

            if (!File.Exists(_hil_file_path))
            {
                return;
            }


            string temp = _hil_file_path;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine(
                    DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond + '\t' + 
                    lat + '\t' + 
                    lon + '\t' + 
                    hasl + '\t' + 
                    altitude + '\t' + 
                    climbrate + '\t' + 
                    gspeed + '\t' + 
                    airspeed + '\t' + 
                    mav_link + '\t' + 
                    rssi_link + '\t' + 
                    throttle + '\t' + 
                    roll + '\t' + 
                    pitch + '\t' + 
                    yaw + '\t' +
                    rollrate + '\t' +
                    pitchrate + '\t' +
                    yawrate + '\t' +
                    accelX + '\t' +
                    accelY + '\t' +
                    accelZ + '\t' +
                    vgN + '\t' +
                    vgE + '\t' +
                    vgD + '\t' +
                    daileron   + '\t' +
                    delevator  + '\t' +
                    drudder    + '\t' +
                    dthrottle  + '\t'
                    );
            }

            // Mat file

            if (!File.Exists(_hil_mat_file_path))
            {
                return;
            }

            temp = _hil_mat_file_path;

            using (StreamWriter sw = new StreamWriter(temp, true))
            {
                sw.WriteLine(
                    DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond + '\t' +
                    lat + '\t' +
                    lon + '\t' +
                    hasl + '\t' +
                    altitude + '\t' +
                    climbrate + '\t' +
                    gspeed + '\t' +
                    airspeed + '\t' +
                    mav_link + '\t' +
                    rssi_link + '\t' +
                    throttle + '\t' +
                    roll + '\t' +
                    pitch + '\t' +
                    yaw + '\t' +
                    rollrate + '\t' +
                    pitchrate + '\t' +
                    yawrate + '\t' +
                    accelX + '\t' +
                    accelY + '\t' +
                    accelZ + '\t' +
                    vgN + '\t' +
                    vgE + '\t' +
                    vgD + '\t' +
                    daileron + '\t' +
                    delevator + '\t' +
                    drudder + '\t' +
                    dthrottle + '\t'
                    );
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


        /// <summary>
        /// call this on every new serial connection
        /// </summary>
        public static void Reset()
        {
            log_started = false;
            hil_log_started = false;
        }
    }
}
