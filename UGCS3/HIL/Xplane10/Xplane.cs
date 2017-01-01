using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Sockets;

namespace UGCS3.HIL.Xplane10
{
    public class Xplane:Hil
    {
        Socket receiveClient;
        UdpClient sendClient;
        float[][] DATA = new float[113][];
        public bool READY_XPLANE = false;
        EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
        public Xplane()
        {
           
        }

        public void connect(string add, int recport, int sendport)
        {
            // reception
            // reception port is the port of the simulator pc sending the data
            // we will be listening to any IPaddress but on the reception port -> set the simulator PC port to recport
            IPEndPoint ipend = new IPEndPoint(IPAddress.Any, recport);
            receiveClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (!receiveClient.IsBound)
            {
                receiveClient.Bind(ipend);
                Console.WriteLine("UDP Packets incoming on Port " + recport);
            }

            // sending 
            // For sending data use add Ip address and the sending port
            sendClient = new UdpClient(add,sendport);

            Console.WriteLine("Setting up Xplane10");
            byte[] xplaneSettings = new byte[5 + 4 * 8];
            xplaneSettings[0] = (byte)'D';
            xplaneSettings[1] = (byte)'S';
            xplaneSettings[2] = (byte)'E';
            xplaneSettings[3] = (byte)'L';
            xplaneSettings[4] = 0;

            int pos = 5;

            xplaneSettings[pos] = 0x3; // Speeds
            pos += 4;
            xplaneSettings[pos] = 0x4; // Mach, VVI, G-loads
            pos += 4;
            xplaneSettings[pos] = 0x6; // Atmosphere
            pos += 4;
            xplaneSettings[pos] = 0x10;// Decimal 16 - angular velocities 
            pos += 4;
            xplaneSettings[pos] = 0x11;// Dercimal 17 - pitch, roll, headings
            pos += 4;
            xplaneSettings[pos] = 0x12;// Decimal 18 - AoA, side-slip, paths
            pos += 4;
            xplaneSettings[pos] = 0x14;// Decimal 20 - Lat, Lon, Altitude
            pos += 4;
            xplaneSettings[pos] = 0x15;// Decimal 21 - Loc, Vel, DistTravelled
            pos += 4;

            try
            {
                sendClient.Send(xplaneSettings, xplaneSettings.Length);
                Console.WriteLine("Xplane10 settings sent.");
                READY_XPLANE = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Xplane Class, Sending Settings DSEL error : " + ex.Message);
                throw new Exception("Xplane Failed to Connect :-" + '\n'+ ex.Message);
            }
        }

        
        public void Get_FromSimulator(ref sitl_fdm sitldata)
        {
            if (!receiveClient.IsBound)
                return;
            if (receiveClient.Available > 0)
            {
                byte[] udpdata = new byte[1500];
                int receivebytes = 0;

                while (receiveClient.Available > 0)
                {
                    try
                    {
                        receivebytes = receiveClient.ReceiveFrom(udpdata, ref remote);
                    }
                    catch (Exception ex) { break; }
                }

                // xplane10 sends a 5 byte DATA header, fifth byte is sent internally in xplane and is the Index of the data that is sent
                // Index is equal to whatever was requested in the DSEL message
                if(udpdata[0] == 'D' && udpdata[1] == 'A')
                {
                    int count = 5;
                    while(count < receivebytes)
                    {
                        int index = BitConverter.ToInt32(udpdata, count);
                        DATA[index] = new float[8];

                        DATA[index][0] = BitConverter.ToSingle(udpdata, count + 1 * 4); ;
                        DATA[index][1] = BitConverter.ToSingle(udpdata, count + 2 * 4); ;
                        DATA[index][2] = BitConverter.ToSingle(udpdata, count + 3 * 4); ;
                        DATA[index][3] = BitConverter.ToSingle(udpdata, count + 4 * 4); ;
                        DATA[index][4] = BitConverter.ToSingle(udpdata, count + 5 * 4); ;
                        DATA[index][5] = BitConverter.ToSingle(udpdata, count + 6 * 4); ;
                        DATA[index][6] = BitConverter.ToSingle(udpdata, count + 7 * 4); ;
                        DATA[index][7] = BitConverter.ToSingle(udpdata, count + 8 * 4); ;

                        count         += 36; // 8 * float  
                    }

                    
                    sitldata.pitchDeg  = (DATA[17][0]);
                    sitldata.rollDeg   = (DATA[17][1]);
                    sitldata.yawDeg    = (DATA[17][3]); // True heading - [2] | Mag Heading - [3] RANGE 0 -360
                 
                    if(sitldata.yawDeg > 180)
                    {
                        sitldata.yawDeg = sitldata.yawDeg - 360; // CONVERTS YAW TO +/- 180 DEGREES
                    }

                    Variables.hilrollDeg = (float)sitldata.rollDeg;
                    Variables.hilpitchDeg = (float)sitldata.pitchDeg;
                    Variables.hilyawDeg = (float)sitldata.yawDeg;

                    sitldata.pitchRate = (DATA[16][0] * rad2deg);
                    sitldata.rollRate  = (DATA[16][1] * rad2deg);
                    sitldata.yawRate   = (DATA[16][2] * rad2deg);
                    sitldata.heading   = (DATA[18][2]); // equivalent to HPATH GPS course
                    

                    sitldata.airspeed = ((DATA[3][5] * .44704)); // miles per hour true airspeed to m/s

                    sitldata.latitude  = (DATA[20][0]);
                    sitldata.longitude = (DATA[20][1]);
                    sitldata.altitude  = (DATA[20][3] * ft2m); // 2 - altitude above mean sea level... 3- above the ground level..

                    // North is -ve Z and East is +ve X
                    sitldata.speedN = -DATA[21][5]; // (DATA[3][7] * 0.44704 * Math.Sin(sitldata.heading * deg2rad));
                    sitldata.speedE = DATA[21][3]; // (DATA[3][7] * 0.44704 * Math.Cos(sitldata.heading * deg2rad));


                    // rad = tas^2 / (tan(angle) * G)
                    float turnrad = (float)(((DATA[3][7] * 0.44704) * (DATA[3][7] * 0.44704)) / (float)(9.8f * Math.Tan(sitldata.rollDeg * deg2rad)));

                    float gload = (float)(1 / Math.Cos(sitldata.rollDeg * deg2rad)); // calculated Gs

                    // a = v^2/r
                    float centripaccel = (float)((DATA[3][7] * 0.44704) * (DATA[3][7] * 0.44704)) / turnrad;

                    // NED Axis convention 
                    // acceleration units in gravity
                    sitldata.zAccel = (0-DATA[4][4]); // G's
                    sitldata.xAccel = DATA[4][5]; // G's
                    sitldata.yAccel = DATA[4][6]; // G's
                }
            }
        }

        public void SendToSim(float[] packet)
        {
            // Hil Channels from Main...
            // Roll, Pitch, Throttle, Yaw
            float roll_out     = packet[0];
            float pitch_out    = packet[1];
            float rudder_out   = packet[3];
            float throttle_out = packet[2];

            roll_out = mapConstrain(roll_out, 1000,2000,-1, 1);
            pitch_out = mapConstrain(pitch_out, 1000,2000,-1, 1);
            rudder_out = mapConstrain(rudder_out,1000,2000, -1, 1);
            throttle_out = mapConstrain(throttle_out, 1000,2000,0, 1);
            /*
            roll_out     = Constrain(roll_out,    -1, 1);
            pitch_out    = Constrain(pitch_out,   -1, 1);
            rudder_out   = Constrain(rudder_out,  -1, 1);
            throttle_out = Constrain(throttle_out, 0, 1);
            */
            // sending only 1 packet instead of many
            byte[] Xplane = new byte[5 + 36 + 36];

            // Header packet
            Xplane[0] = (byte)'D';
            Xplane[1] = (byte)'A';
            Xplane[2] = (byte)'T';
            Xplane[3] = (byte)'A';
            Xplane[4] = 0;

            Array.Copy(BitConverter.GetBytes((int)25), 0, Xplane, 5, 4); // packet index

            Array.Copy(BitConverter.GetBytes((float)throttle_out), 0, Xplane, 9, 4); // start data
            Array.Copy(BitConverter.GetBytes((float)throttle_out), 0, Xplane, 13, 4);
            Array.Copy(BitConverter.GetBytes((float)throttle_out), 0, Xplane, 17, 4);
            Array.Copy(BitConverter.GetBytes((float)throttle_out), 0, Xplane, 21, 4);

            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 25, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 29, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 33, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 37, 4);

            // NEXT ONE - control surfaces

            Array.Copy(BitConverter.GetBytes((int)11), 0, Xplane, 41, 4); // packet index

            Array.Copy(BitConverter.GetBytes((float)(pitch_out * REV_pitch)), 0, Xplane, 45, 4); // start data
            Array.Copy(BitConverter.GetBytes((float)(roll_out * REV_roll)), 0, Xplane, 49, 4);
            Array.Copy(BitConverter.GetBytes((float)(rudder_out * REV_yaw)), 0, Xplane, 53, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 57, 4);

            Array.Copy(BitConverter.GetBytes((float)(roll_out * REV_roll * 0.5)), 0, Xplane, 61, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 65, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 69, 4);
            Array.Copy(BitConverter.GetBytes((int)-999), 0, Xplane, 73, 4);

            try
            { 
                sendClient.Send(Xplane, Xplane.Length); 
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        public MAVLink.mavlink_hil_state_t Mavlink_PackHilState(sitl_fdm sitldata)
        {
            TimeSpan gpsspan = DateTime.Now - lastgpsupdate;
            // add gps delay
            if (gpsspan.TotalMilliseconds >= GPS_rate)
            {
                lastgpsupdate = DateTime.Now;
                // save current fix = 3
                sitl_fdmbuffer[gpsbufferindex % sitl_fdmbuffer.Length] = sitldata; // 0 - 5

                // return buffer index + 5 = (3 + 5) = 8 % 6 = 2
                oldgps = sitl_fdmbuffer[(gpsbufferindex + (sitl_fdmbuffer.Length - 1)) % sitl_fdmbuffer.Length];

                gpsbufferindex++;
            }


            MAVLink.mavlink_hil_state_t hilstate = new MAVLink.mavlink_hil_state_t();

            hilstate.time_usec = (UInt64)lastgpsupdate.Ticks; // time in microseconds since last gps update
            //Console.WriteLine("Ticks: " + hilstate.time_usec);
            hilstate.lat = (int)(oldgps.latitude * 1e7); // * 1E7
            hilstate.lon = (int)(oldgps.longitude * 1e7); // * 1E7
            hilstate.alt = (int)(oldgps.altitude * 1000); // mm

            //Console.WriteLine("Altitude: " +hilstate.alt);
            // We are sending in normal mavlink state
            hilstate.pitch = (float)sitldata.pitchDeg * deg2rad;       // (rad)   // has to be -pi/2 to pi/2
            hilstate.pitchspeed = (float)sitldata.pitchRate * deg2rad; // (rad/s)
            hilstate.roll = (float)sitldata.rollDeg * deg2rad;         // (rad)   // has to be -pi to pi 
            hilstate.rollspeed = (float)sitldata.rollRate * deg2rad;   // (rad/s)
            hilstate.yaw = (float)sitldata.yawDeg * deg2rad;           // (rad)   // has to be -pi to pi
            hilstate.yawspeed = (float)sitldata.yawRate * deg2rad;     // (rad/s)

            hilstate.vx = (short)(oldgps.speedN * 100); // m/s * 100
            hilstate.vy = (short)(oldgps.speedE * 100); // m/s * 100
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
            /*
            AASCoGroundControl_V1._mavlinkInterfaceClass.sendPacket(hilstate);

            AASCoGroundControl_V1._mavlinkInterfaceClass.sendPacket(new MAVLink.mavlink_vfr_hud_t()
            {
                airspeed = (float)sitldata.airspeed
            });
            */ 
        }

        public MAVLink.mavlink_vfr_hud_t Mavlink_VfrAirspeed(sitl_fdm sitldata)
        {
            MAVLink.mavlink_vfr_hud_t vfr_t = new MAVLink.mavlink_vfr_hud_t();
            vfr_t.airspeed = (float)sitldata.airspeed;
            vfr_t.heading = (short)sitldata.heading;
            return vfr_t;
        }

        public void disconnect()
        {
            try
            {
                if (receiveClient != null)
                {
                    receiveClient.Close();
                    receiveClient = null;
                }
                if (sendClient != null)
                {
                    sendClient.Close();
                    sendClient = null;
                }

                READY_XPLANE = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Xplane failed to disconnect");
                throw new Exception("Failed to disconnect UDP connection :- "+'\n'+ex.Message);
            }
        }
    }
}
