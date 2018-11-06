using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGCS3
{
    static class Variables
    {
        public static bool WAITING_FOR_PARAM_LIST = true; // this parameter after receipt of all parameters is set to false so that the gcs doesnt start showing the param_form and other stuff
        public static float link_rssi;
        public static float link_mavlink;
        public static int request_missionitem_target_system = 0;
        public static int requested_missionitem_seq = 0;
        public static byte mission_ack = (byte)MAVLink.MAV_MISSION_RESULT.MAV_MISSION_DENIED;
        
        public static int mission_count = 0;
        public static int mission_seq   = -1;
        public static float[,] RecWP = new float[25, 5];

        public static float auto_latitude = 0;
        public static float auto_longitude = 0;
        public static float auto_altitude = 0;
        public static float auto_radius = 0;

        public static bool ack_message_received = false;
        public static bool gps_command_sent = false;
        public static bool mission_start_sent = false;

        public static bool start_hil_log = false;

        public struct WP_Item
        {
            public float lat;
            public float lng;
            public float alt;
            public float radius;
            public ushort command;
            public ushort sequence;
            public ushort count;
        };

        public static WP_Item[] WP = new WP_Item[100];


        public static Int16 gyroX, gyroY, gyroZ, accelX, accelY, accelZ, magX, magY, magZ;
        public static float rollDeg, pitchDeg, yawDeg, hilrollDeg, hilpitchDeg, hilyawDeg, imu_altitude, imu_climbrate;
        public static float rollrateDeg, pitchrateDeg, yawrateDeg, rollraterad, pitchraterad, yawraterad;
        public static ushort chan1 = 1500, chan2 = 1500, chan3 = 1500, chan4 = 1500, chan5 = 1500, chan6 = 1500, chan7 = 1500, chan8 = 1500;

        public static float batteryVolts;

        public static void reset_variables()
        {
            gyroX = gyroY  = gyroZ=accelX= accelY= accelZ= magX= magY= magZ=0;
            rollrateDeg    = pitchrateDeg = yawrateDeg = rollDeg = pitchDeg = yawDeg = 0;
            hilrollDeg     =  hilpitchDeg = hilyawDeg = imu_altitude = imu_climbrate = 0;
            chan1 =  chan2 = chan3 =  chan4 =  chan5 =  chan6 =  chan7 =  chan8 = 1500;

        }

        public static float latitude, longitude, hMSL, gSpeed, heading, airspeed, throttle, cog, vfr_groundspeed, gps_altitude, gprs_latitude, gprs_longitude;

        private static byte UAV_ID = 1;

        public static byte  Signal;
        
        public static byte  fix_type = 0, numSatellites = 0;

        private static byte FLIGHT_MODE = 0;

        public  enum FLIGHT_MODES
        {
            PREFLIGHT,
            MANUAL,
            STABILISE, 
            AUTO,
            IGNORE
        };

        /// <summary>
        ///  gets or sets the UAV_ID
        /// </summary>
        public static byte UAVID
        {
            set
            {
                UAV_ID = value;
            }

            get
            {
                return UAV_ID;
            }
        }

        /// <summary>
        /// sets the flight mode
        /// </summary>
        /// <param name="mode"></param>
        public static void set_FLIGHTMODE(FLIGHT_MODES mode)
        {
            FLIGHT_MODE = (byte)mode;
        }

        /// <summary>
        ///  gets the flight mode
        /// </summary>
        public static byte get_FLIGHTMODE
        {
            get
            {
                return FLIGHT_MODE;
            }
        }

        public static string get_FLIGHTMODEString
        {
            get
            {
                switch (FLIGHT_MODE)
                {
                    case (byte)FLIGHT_MODES.PREFLIGHT:
                        return "PREFLIGHT";

                    case (byte)FLIGHT_MODES.MANUAL:
                        return "MANUAL";

                    case (byte)FLIGHT_MODES.STABILISE:
                        return "STABILISE";

                    case (byte) FLIGHT_MODES.AUTO:
                        return "AUTO";

                    case (byte)FLIGHT_MODES.IGNORE:
                        return "IGNORE";

                    default:
                        return "IGNORE";
                }
            }
        }

        public static UInt16 error2 = 0, error1=0, error3 = 0;
        public static string GPS_ERROR()
        {
            if (error2 == 0)
                return "BAD G P S HEALTH";
            else
                return "";
        }

        public static string INS_ERROR()
        {
            if (error1 == 0)
                return "BAD I N S HEALTH";
            else
                return "GOOD I N S HEALTH";
        }

        public static string BARO_ERROR()
        {
            if (error3 == 0)
                return "BAD BARO HEALTH";
            else
                return "";
        }

        public static string fix_status
        {
            get
            {
                switch (fix_type)
                {
                    case 0:
                        return "NoFix";

                    case 1:
                        return "DR";

                    case 2:
                        return "2DFix";

                    case 3:
                        return "3DFix";

                    case 4:
                        return "3D+";

                    default:
                        return "UNK";
                }
            }
        }


        public static float ToDeg(float x)
        {
           return( (float)(x * 180 / Math.PI));
        }

        public static float ToDeg(double x)
        {
            return ((float)(x * 180 / Math.PI));
        }

        public static float ToDeg(int x)
        {
            return ((int)(x * 180 / Math.PI));
        }

        public static float ToRad(double x)
        {
            return ((float)(x * Math.PI/180));
        }

        public static float ToRad(float x)
        {
            return ((float)(x * Math.PI / 180));
        }
    }
}
