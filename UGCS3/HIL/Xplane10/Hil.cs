using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGCS3.HIL.Xplane10
{
    public abstract class Hil
    {
        public struct sitl_fdm
        {
            // packets sent from xplan simulator will be decoded to the following data - little endian
            // rates in degrees per second
            // accelerations in m/s2 in body frame
            public double latitude, longitude, altitude, heading, speedN, speedE, speedD, xAccel, yAccel, zAccel, rollRate, pitchRate, yawRate, rollDeg, pitchDeg, yawDeg, airspeed, elevator, aileron, rudder, throttle, inaileron, inelevator, inrudder, inthrottle;
            public UInt32 magic; // 0x4c56414e
        };

        internal DateTime lastgpsupdate = DateTime.Now;
        internal sitl_fdm[] sitl_fdmbuffer = new sitl_fdm[5];
        internal sitl_fdm oldgps = new sitl_fdm();
        internal int gpsbufferindex = 0;
        internal int GPS_rate = 200;

        public float rad2deg = (float)(180/Math.PI);
        public float ft2m    = 0.3048F;
        public float deg2rad = (float)Math.PI / 180;

        public int REV_pitch = 1, REV_roll = 1, REV_yaw = 1, REV_throttle = 1;

        public float Constrain(float val , float min, float max)
        {
            if (val > max)
                val = max;
            else if (val < min)
                val = min;

            return val;
        }

        public float mapConstrain(float val, float xmin, float xmax, float min, float max)
        {
            float ops = ((val - xmin)/(xmax - xmin))*(max - min) + min;
            return ops;
        }

    }
}
