using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
