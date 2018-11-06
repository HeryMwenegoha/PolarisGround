using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UGCS3;
using UGCS3.HIL.Xplane10;
using UGCS3.Common;
namespace UGCS3.MavlinkProtocol
{
    static class MavlinkExecution 
    {

        /// <summary>
        ///  pack a parameter list request message
        /// </summary>
        /// <param name="UAV_ID"></param>
        /// <returns></returns>
        public static MAVLink.mavlink_param_request_list_t ParameterRequestList(byte UAV_ID)
        {
            MAVLink.mavlink_param_request_list_t request_parameters_t = new MAVLink.mavlink_param_request_list_t()
            {
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL,
                target_system = UAV_ID,
            };
            return request_parameters_t;
        }


        public static MAVLink.mavlink_param_set_t ParameterSet(Dictionary<string, System.Windows.Forms.NumericUpDown> dic, List<int> changed_indecies, int mask_index)
        {
            int actual_index = changed_indecies[mask_index];

            byte[] temp = ASCIIEncoding.ASCII.GetBytes(dic.Keys.ToList()[actual_index]);

            Array.Resize(ref temp, 16);

            MAVLink.mavlink_param_set_t param_set_t = new MAVLink.mavlink_param_set_t()
            {
                
                param_id = temp,
                param_value = (float)dic.Values.ToList()[actual_index].Value,
                param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32,
                target_system = Variables.UAVID,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL,
            };

            return param_set_t;
        }


        public static MAVLink.mavlink_vfr_hud_t Mavlink_VfrAirspeed(UGCS3.HIL.Xplane10.Hil.sitl_fdm sitldata)
        {
            MAVLink.mavlink_vfr_hud_t vfr_t = new MAVLink.mavlink_vfr_hud_t();
            vfr_t.airspeed = (float)sitldata.airspeed;
            vfr_t.heading = (short)sitldata.heading;
            return vfr_t;
        }

        public static MAVLink.mavlink_hil_state_t Mavlink_PackHilState (Xplane xplane, Hil.sitl_fdm sitldata)
        {
            TimeSpan gpsspan = DateTime.Now - xplane.lastgpsupdate;
            // add gps delay
            if (gpsspan.TotalMilliseconds >= xplane.GPS_rate)
            {
                xplane.lastgpsupdate = DateTime.Now;
                // save current fix = 3
                xplane.sitl_fdmbuffer[xplane.gpsbufferindex % xplane.sitl_fdmbuffer.Length] = sitldata; // 0 - 5

                // return buffer index + 5 = (3 + 5) = 8 % 6 = 2
                xplane.oldgps = xplane.sitl_fdmbuffer[(xplane.gpsbufferindex + (xplane.sitl_fdmbuffer.Length - 1)) % xplane.sitl_fdmbuffer.Length];

                xplane.gpsbufferindex++;
            }

            MAVLink.mavlink_hil_state_t hilstate = new MAVLink.mavlink_hil_state_t();

            hilstate.time_usec = (UInt64)xplane.lastgpsupdate.Ticks; // time in microseconds since last gps update
            //Console.WriteLine("Ticks: " + hilstate.time_usec);
            hilstate.lat = (int)(xplane.oldgps.latitude * 1e7); // * 1E7
            hilstate.lon = (int)(xplane.oldgps.longitude * 1e7); // * 1E7
            hilstate.alt = (int)(xplane.oldgps.altitude * 1000); // mm

            //Console.WriteLine("Altitude: " +hilstate.alt);
            // We are sending in normal mavlink state
            hilstate.pitch = (float)sitldata.pitchDeg * common.deg2rad;       // (rad)   // has to be -pi/2 to pi/2
            hilstate.pitchspeed = (float)sitldata.pitchRate * common.deg2rad; // (rad/s)
            hilstate.roll = (float)sitldata.rollDeg * common.deg2rad;         // (rad)   // has to be -pi to pi 
            hilstate.rollspeed = (float)sitldata.rollRate * common.deg2rad;   // (rad/s)
            hilstate.yaw = (float)sitldata.yawDeg * common.deg2rad;           // (rad)   // has to be -pi to pi
            hilstate.yawspeed = (float)sitldata.yawRate * common.deg2rad;     // (rad/s)

            hilstate.vx = (short)(xplane.oldgps.speedN * 100); // m/s * 100
            hilstate.vy = (short)(xplane.oldgps.speedE * 100); // m/s * 100
            hilstate.vz = 0; // m/s * 100

            // EUS is the opengpl convention used defined  in the following manner
            // X+ - East
            // Y+ - UP
            // Z+ - South


            // This have already been converted to NED
            hilstate.xacc = (short)(sitldata.xAccel * 1000); // (mg)
            hilstate.yacc = (short)(sitldata.yAccel * 1000); // (mg)
            hilstate.zacc = (short)(sitldata.zAccel * 1000); // (mg)

            return hilstate;
        }

        /*
        public static void Send_Mavlink(object indata, System.IO.Ports.SerialPort _sp)
        {
            bool validPacket = false;
            byte a = 0;
            foreach(Type ty in MAVLink.MAVLINK_MESSAGE_INFO)
            {
                if(ty == indata.GetType())
                {
                    validPacket = true;
                    generatePacket(a, indata, _sp);
                    return;
                }
                a++;
            }
            if(!validPacket)
            {
                System.Diagnostics.Debug.WriteLine("MAVLink: Not Valid Packet In sendPacket() of Type {0} ", indata.GetType().ToString());
                //("MAVLink: Not Valid Packet In sendPacket() of Type {0} ", indata.GetType().ToString());
            }  
        }

        private static void generatePacket(byte messageType, object indata, System.IO.Ports.SerialPort _sp)
        {
            if (!_sp.IsOpen)
            {
                return;
            }

            //if (ReadOnly)
            //{
            //    // Allow Certain Messages Only... Mostly Request Lists etc..
            //}

            lock (ObjLock)
            {
                byte[] data;
                data = MavlinkUtil.StructureToByteArray(indata);
                byte[] packet = new byte[data.Length + 6 + 2];               // Length =  PayLoad + Headers + Checksum
                packet[0] = 254;                                         // StartSign
                packet[1] = (byte)data.Length;                           // PayLoad Length
                packet[2] = (byte)packetcount;                           // Packet Sequence Number
                packetcount++;

                packet[3] = 255;                                             // My GCS System ID
                packet[4] = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;     // COMPONENT_ID IS SUPPOSED TO BE AASCO_ID BuT JUST USE ALL FOR NOW>>
                packet[5] = messageType;

                int i = 6;
                foreach (byte b in data)
                {
                    packet[i] = b;
                    i++;
                }

                ushort checksum = MAVLink.MavlinkCRC.crc_calculate(packet, packet[1] + 6);
                checksum = MAVLink.MavlinkCRC.crc_accumulate(MAVLink.MAVLINK_MESSAGE_CRCS[messageType], checksum);
                byte ck_a = (byte)(checksum & 0xFF);  // HIGH BYTE
                byte ck_b = (byte)(checksum >> 8);    // LOW  BYTE

                packet[i] = ck_a;
                i += 1;
                packet[i] = ck_b;
                i += 1;

                if (_sp.IsOpen)
                {
                    _sp.Write(packet, 0, i);
                }
            }
        }
        */
    }
}
