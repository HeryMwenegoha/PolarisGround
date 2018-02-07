#define SP4
#define READ_PKS_1
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


using System.Windows.Forms.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.ComponentModel;

//using AASCoGroundControl;
//using MavLink;
namespace UGCS3
{
    public delegate void ParameterEventHandler(string s, int index, int count, float p);
    public delegate void MavlinkStatusTextHandler(string P);
    
    public class MavLinkSerialPacketClass
    {
        public event ParameterEventHandler ParameterEvent;
        public event MavlinkStatusTextHandler MavlinkStatusTextEvent;
        private System.IO.Ports.SerialPort _sp;
        private float oldPcksReceived;
        public float PcksLost;
        public float PcksReceived;
        private float oldPcksLost;

        public double[] packetspersecond { get; set; } // Used for Stream rate calculation
        DateTime[] packetspersecondbuild = new DateTime[256];

        internal  byte packetcount = 0;

        internal int SYSTEM_ID           = 1;
        public bool ReadOnly             = false;
        public bool hbreceived           = false;
        volatile object ObjLock          = new object();
        volatile object ReadLock         = new object();
        internal string plaintxtline     = "";
        string buildplaintxtline         = "";

        float synclost;
        internal int recvpacketcount = 0;
        int received_packet = 0;
        internal float packetsLost = 0;
        internal float packets_not_Lost = 0;
        byte[] lastbadPacket = new byte[2];
        int LostPackets;
        DateTime packetLostTimer = DateTime.MinValue;
        public ushort linkquality { get; set; }
        internal int delay = 1;

        int bps1 = 0;
        int bps2 = 0;
        public int bps { get; set; }
        public DateTime bpstime { get; set; }

        System.Windows.Forms.Timer _timerpacketsperSecond;
        
              
        public MavLinkSerialPacketClass(System.IO.Ports.SerialPort Port)
        {
            this._sp                    = Port;

           // this.packetcount            = 0;
            this.SYSTEM_ID              = 1;
            this.ReadLock               = new object();
            this.bpstime                = DateTime.MinValue;
            this.bps2                   = 0;
            this.bps1                   = 0;
            this.bps                    = 0;
            this.packetspersecond       = new double[0x100];
            this.packetspersecondbuild  = new DateTime[0x100];
            this._timerpacketsperSecond = new System.Windows.Forms.Timer();

            ready_for_flags = false;
            ready_plot_gps = false;

           
            // Timer Event...
            // this._timerpacketsperSecond.Tick    += _timerpacketsperSecond_ElapsedEvent;
            // this._timerpacketsperSecond.Interval = 1000;
            // this._timerpacketsperSecond.Start();
            
            Console.WriteLine("Mavlink Initialised...");
        }

        public System.IO.Ports.SerialPort set_sp
        {
            set
            {
                this._sp = value;
            }
        }

        public System.IO.Ports.SerialPort get_sp()
        {
             return this._sp;      
            
        }

        // BPS:-
        private void _timerpacketsperSecond_ElapsedEvent (object sender, EventArgs e)
        {
            // Every Second Compare the Total Packets Received and that Lost...
            oldPcksReceived = PcksReceived;
            PcksReceived    = packets_not_Lost;

            //
            oldPcksLost = PcksLost;
            PcksLost = packetsLost;
        }

    
        public void sendPacket(object indata)
        {
            bool validPacket = false;
            byte a = 0;
           
            foreach(Type ty in MAVLink.MAVLINK_MESSAGE_INFO)
            {
                if(ty == indata.GetType())
                {
                    validPacket = true;
                    generatePacket(a, indata);
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

        /// <summary>
        ///  generates a valid Packet and Writes to the SerialPort
        /// </summary>
        /// <param name="messageType"> type number == MAVLINK_MSG_ID</param>
        /// <param name="indata"> structure of data </param>
        private void generatePacket(byte messageType, object indata)
        {
            if(!_sp.IsOpen)
            {
                return;
            }
            if(ReadOnly)
            {
                // Allow Certain Messages Only... Mostly Request Lists etc..
            }
            lock(ObjLock)
            {
                byte[] data;
                data          = MavlinkUtil.StructureToByteArray(indata);
                byte[] packet = new byte[data.Length + 6 + 2];               // Length =  PayLoad + Headers + Checksum
                packet[0]     = 254;                                         // StartSign
                packet[1]     = (byte)data.Length;                           // PayLoad Length
                packet[2]     = unchecked(packetcount++);                           // Packet Sequence Number
                packet[3]     = 255;                                             // My GCS System ID
                packet[4]     = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;     // COMPONENT_ID IS SUPPOSED TO BE AASCO_ID BuT JUST USE ALL FOR NOW>>
                packet[5]     = messageType;

                int i = 6;
                foreach(byte b in data)
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

                if(_sp.IsOpen)
                {
                    _sp.Write(packet, 0, i);
                }
             }
        }
 
        /// <summary>
        ///  Read Incomming Mavlink Stream
        /// </summary>
        public byte[] readMavPackets()
        {
            byte[] buffer   = new byte[255 + 25];
            int count       = 0;
            int length      = 0;
            int readcount   = 0;
            lastbadPacket   = new byte[2];
            
            if(_sp != null)
                _sp.ReadTimeout = 10000;
            else
            {
                Console.WriteLine("error::Serial object is null, please assign global serial object..");
                return null;
            }


            if (!_sp.IsOpen)
                return null;

            lock (ReadLock)
            {
               
                while (_sp.IsOpen)
                {
                    try
                    {
                        if (readcount > 300)
                        {
                            Console.WriteLine("No Mavlink Packet Received Since Readcount is {0}", readcount);
                            break;
                        }
                        readcount++;
                       
                        DateTime to = DateTime.Now.AddMilliseconds(_sp.ReadTimeout);
                       
                        while (_sp.IsOpen && _sp.BytesToRead <= 0)
                        {
                            if (DateTime.Now > to)
                            {                               
                                Console.WriteLine("MAVLink: 1 wait time out btr {0} len {1}", _sp.BytesToRead, length);
                                throw new TimeoutException("TimeOut");
                            }
                            Thread.Sleep(delay + 2);
                        }
                       

                        if (_sp.IsOpen)
                        {
                            _sp.Read(buffer, count, 1);
                        }
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine("MAvLink ReadPacket Error" + ex.ToString()); 
                        break;
                    }

                    if (buffer[0] != 254 && buffer[0] != 'U' && buffer[0] != 253)
                    {
                        if (buffer[0] >= 0x20 && buffer[0] <= 127 || buffer[0] == '\n' || buffer[0] == '\r')
                        {
                            //check for termination
                            if (buffer[0] == '\r' || buffer[0] == '\n')
                            {
                                // Check for a Valid Line
                                if (buildplaintxtline.Length > 3)
                                    plaintxtline = buildplaintxtline;

                                // Reset Line..
                                buildplaintxtline = "";
                            }
                            Console.Write((char)buffer[0]);
                            buildplaintxtline += (char)buffer[0];
                        }
                        count = 0;
                        lastbadPacket[0] = lastbadPacket[1];
                        lastbadPacket[1] = buffer[0];
                        buffer[1] = 0;
                        continue;  // skip the rest of the following code and go back to the top...
                    }

                    readcount = 0; // buffer[0] == 254 here reset the readcount 


                    // check for a header
                    if (buffer[0] == 254 || buffer[0] == 'U' || buffer[0] == 253)
                    {
                        //Console.WriteLine(buffer[0]);
                        // if Only First Character has been received and there are no other characters
                        if (count == 0)
                        {
                            DateTime to = DateTime.Now.AddMilliseconds(_sp.ReadTimeout);
                            while (_sp.IsOpen && _sp.BytesToRead < 5)
                            {
                                if (DateTime.Now > to)
                                {
                                    
                                    Console.WriteLine("MAVLink: 2 Wait time out btr {0} len {1}", _sp.BytesToRead, length);
                                    //throw new Exception("TimeOut");
                                    // the moment you have 5 or more bytes to read  skip..
                                }
                                Thread.Sleep(delay);
                            }
                            int read = _sp.Read(buffer, 1, 5);
                            count = read;
                        }

                        // Packet Length excluding crc                      
                        length = buffer[1] + 6 + 2 - 2;

                        if (count >= 5)
                        {
                            try
                            {
                                DateTime to = DateTime.Now.AddMilliseconds(_sp.ReadTimeout);
                                // loop here if we still have 1 byte less of the remaining bytes : L-5
                                while (_sp.IsOpen && _sp.BytesToRead < (length - 4))
                                {
                                    // Length- 4 = PayLoad + Checksum ==> If Not Received PayLoad + Checksum then Just loop here.
                                    if (DateTime.Now > to)
                                    {
                                        Debug.WriteLine("MAVLInk : 3 wait time out btr {0} len {1}", _sp.BytesToRead, length);
                                        break;
                                    }
                                    Thread.Sleep(delay);
                                }
                                if (_sp.IsOpen)
                                {
                                    int read = _sp.Read(buffer, 6, length - 4);
                                }
                                count = length + 2;
                            }
                            catch { break; }
                            break;
                        }
                    }

                    count++;
                    if (count == 299)
                        break;
                } // End of Main While Loop : while(_sp.isOpen) -> reading of serial stream occurs here

            }     // End of Main Lock         Lock (ReadLock)

            Array.Resize<byte>(ref buffer, count);

            if (packetLostTimer.AddSeconds(5) < DateTime.Now)
            {
                packetLostTimer  = DateTime.Now;
                packetsLost      = (packetsLost * 0.8f);
                packets_not_Lost = (packets_not_Lost * 0.8f);
            }

            linkquality = (ushort)((packets_not_Lost / (packets_not_Lost + packetsLost))*100);

            if (bpstime.Second != DateTime.Now.Second && _sp.IsOpen)
            {
                //Debug.WriteLine("bps {0} loss {1} left {2} mem {3}      \n", bps1, synclost, _sp.BytesToRead, System.GC.GetTotalMemory(false) / 1024 / 1024.0);
                //Console.WriteLine(bps1);
                bps2 = bps1; // prev sec
                bps1 = 0;    // current sec
                bpstime = DateTime.Now;
            }

            bps1 += buffer.Length;

            bps = (bps1 + bps2) / 2;

            if (buffer.Length == 0)
                return null;

            // Now for the CheckSum...
            ushort crc = MAVLink.MavlinkCRC.crc_calculate(buffer, buffer.Length - 2); // CRC 1
            if (buffer[0] == 254 || buffer[0] == 253)
            {
                crc = MAVLink.MavlinkCRC.crc_accumulate(MAVLink.MAVLINK_MESSAGE_CRCS[buffer[5]], crc);
            }

            if (buffer.Length > 5 && buffer[1] != MAVLink.MAVLINK_MESSAGE_LENGTHS[buffer[5]])
            {
                if (MAVLink.MAVLINK_MESSAGE_LENGTHS[buffer[5]] == 0)
                {
                    // UnKnown packet
                }
                else
                {
                    Console.WriteLine("MAvLink bad Packet: Probably Upgrade");
                    return null;
                }
            }

            if (buffer.Length < 5 || buffer[buffer.Length - 1] != (crc >> 8) || buffer[buffer.Length - 2] != (crc & 0xff))
            {
                int packetNo = -1;
                if (buffer.Length > 5)
                {
                    packetNo = buffer[5];
                }

                if (packetNo != -1 && buffer.Length > 5 && MAVLink.MAVLINK_MESSAGE_INFO[packetNo] != null)
                {
                    Console.WriteLine("MAvlink bad packet (crc fail) len {0} crc {1} ", buffer.Length, crc);        
                }
                return null;
            }
            
            // packet is now verified here
            byte sysid = buffer[3];
            byte compid = buffer[4];

            // gcs packet
            if (buffer.Length >= 5 && (sysid == 255 || sysid == 253)) 
            {
                return buffer;
            }
            try
            {
                if ((buffer[0] == 'U' || buffer[0] == 254 || buffer[0] == 253) && buffer.Length >= buffer[1])
                {
                    if (buffer[3] == '3' && buffer[4] == 'D')
                    {
                        // Console.WriteLine("3DR Packet " + sysid + " " + compid + " SEQ " + buffer[2]); // Packets sent from Radio
                    }
                    else if (buffer[3] == 255 || buffer[3] == 253)
                    {
                        // Console.WriteLine("GSC Packets");
                    }
                    else
                    {
                        byte packetSeqNo = buffer[2];
                        int expectedPacketSeqNo = ((recvpacketcount + 1) % 0x100);
                        int expectedPacketPlus_one = ((expectedPacketSeqNo + 1) % 0x100);
                        int expectedPacketMinus_one = ((recvpacketcount - 1) % 0x100);

                        if ((packetSeqNo != expectedPacketSeqNo) && (packetSeqNo != recvpacketcount) && (packetSeqNo != expectedPacketPlus_one)) // if sequence is not equal to expected as well as previous seqeunce: work around for duplicate packets
                        {
                            synclost++; // actually sync loss's
                            int numLost = 0;
                            if (packetSeqNo < (recvpacketcount + 1)) // if the arrived seq is less than what was expected
                            {
                                numLost = 0x100 - expectedPacketSeqNo + packetSeqNo;
                            }
                            else
                            {
                                numLost = packetSeqNo - expectedPacketSeqNo;
                            }

                            // Put better antenna and lets see the results.
                            Console.WriteLine("SEQ " + packetSeqNo + " ; " + "Expected " + expectedPacketSeqNo + " ; ID " + buffer[5] + " ; Link " + linkquality + "%");

                            packetsLost += numLost;

                            //if (buffer[5] == (byte)MAVLink.MAVLINK_MSG_ID.RADIO)
                            //{
                            //   Console.WriteLine("Radio unaccounted for");
                            //   MAVLink.mavlink_radio_t radio = buffer.ByteArrayToStructure<MAVLink.mavlink_radio_t>(6);
                            //}
                        }

                        //Console.WriteLine("Link " + linkquality + "%");

                        packets_not_Lost++;
                        recvpacketcount = packetSeqNo;
                    }
                    if (double.IsInfinity(packetspersecond[buffer[5]]))
                    {
                        packetspersecond[buffer[5]] = 0;
                    }
                    packetspersecond[buffer[5]] = (((1000 / ((DateTime.Now - packetspersecondbuild[buffer[5]]).TotalMilliseconds) + packetspersecond[buffer[5]]) / 2));
                    packetspersecondbuild[buffer[5]] = DateTime.Now;
                }
                // Instead of decoding messages here, i will decode them on mainview
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void HandParametersToEvent(string s, int index, int count, float p)
        {
            if (ParameterEvent != null)
                ParameterEvent(s,index, count, p);
        }

        private void HandStatusTextToEvent(string P)
        {
            if(MavlinkStatusTextEvent != null)
            {
                MavlinkStatusTextEvent(P);
            }
        }

        public void Send_HeartBeat()
        {
            MAVLink.mavlink_heartbeat_t _hb = new MAVLink.mavlink_heartbeat_t()
            {
                system_status = (byte)MAVLink.MAV_STATE.ACTIVE,
                autopilot = (byte)MAVLink.MAV_AUTOPILOT.GENERIC,
            };
           
            this.sendPacket(_hb);
        }

        public void Send_WayPointCount(int _count)
        {
            MAVLink.mavlink_mission_count_t _mc = new MAVLink.mavlink_mission_count_t()
            {
                target_system = (byte)Variables.UAVID,
                target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL,
                count = (ushort)_count,
            };
            this.sendPacket(_mc);
        }

        public void Send_WayPointItem(object[,] _item, int _sending_index)
        {
            /*
            WP_Item[0, 0] = WPCoordinates.ElementAt(0).Lat; // Lat
            WP_Item[0, 1] = WPCoordinates.ElementAt(0).Lng; // Lon
            WP_Item[0, 2] = 50;                             // Alt
            WP_Item[0, 3] = RadiusNumericUpDown.Value;      // Rad
            WP_Item[0, 4] = 0;                              // Command
             */ 

            MAVLink.mavlink_mission_item_t _mt = new MAVLink.mavlink_mission_item_t();
            
                _mt.target_system = Variables.UAVID;
                _mt.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;
                _mt.x        = (float)_item[_sending_index, 0];
                _mt.y        = (float)_item[_sending_index, 1];
                _mt.z        = float.Parse(_item[_sending_index, 2].ToString());
                _mt.param1   = float.Parse(_item[_sending_index, 3].ToString());
                _mt.command  = Convert.ToUInt16(_item[_sending_index,4]) ;
                _mt.seq      = Convert.ToUInt16(_sending_index);       
            
            this.sendPacket(_mt);
        }

        public void Send_MissionRequestList()
        {
            MAVLink.mavlink_mission_request_list_t mission_list = new MAVLink.mavlink_mission_request_list_t();
            mission_list.target_system = Variables.UAVID;
            mission_list.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;
            this.sendPacket(mission_list);
        }

        public void Send_MissionRequestItem(int seq)
        {
            MAVLink.mavlink_mission_request_t mission_request = new MAVLink.mavlink_mission_request_t();
            mission_request.target_system = Variables.UAVID;
            mission_request.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;
            mission_request.seq = (ushort)seq;
            this.sendPacket(mission_request);
        }

        public void Send_Sequence(int seq)
        {
            MAVLink.mavlink_mission_set_current_t mission_set_current = new MAVLink.mavlink_mission_set_current_t();
            mission_set_current.seq =Convert.ToUInt16(seq);
            mission_set_current.target_system = Variables.UAVID;
            mission_set_current.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_ALL;
            this.sendPacket(mission_set_current);
        }

        public bool ready_plot_gps { get; set; }
        public bool ready_for_flags { get; set; }
        public void Dispose()
        {
            try
            {
                this._sp.Dispose();
                this._timerpacketsperSecond.Dispose();        
            }
            catch { }
        }
    }
}
