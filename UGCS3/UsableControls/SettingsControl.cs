using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using ZedGraph;
using UGCS3.Common;
using System.Windows.Media.Imaging;

//using AForge;
using AForge;
using AForge.Video.DirectShow;

namespace UGCS3.UsableControls
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            InitializeComponent();

            Load += SettingsControl_Load;
            Disposed += SettingsControl_Disposed;
            
            // Pitch
            PitchProgressBar = new VerticalProgressBar();
            PitchProgressBar.Minimum = 750;
            PitchProgressBar.Maximum = 2250;
            PitchProgressBar.Value = 1500;
            PitchProgressBar.Size = new System.Drawing.Size(23, 100);
            PitchProgressBar.Location = new System.Drawing.Point(RollPitchRadioPanel.Size.Width / 2 - (PitchProgressBar.Size.Width / 2), RollProgressBar.Location.Y + RollProgressBar.Size.Height + 10);

            // Throttle
            ThrottleProgressBar = new VerticalProgressBar();
            ThrottleProgressBar.Minimum = 750;
            ThrottleProgressBar.Maximum = 2250;
            ThrottleProgressBar.Value = 1500;
            ThrottleProgressBar.Size = new Size(23, 100);
            ThrottleProgressBar.Location = new System.Drawing.Point(ThrottleYawRadioPanel.Size.Width / 2 - (ThrottleProgressBar.Size.Width / 2), YawProgressBar.Location.Y + YawProgressBar.Size.Height + 10);

            /*
             * 
             *  Pitch
                System.Windows.Forms.Integration.ElementHost ehost = new System.Windows.Forms.Integration.ElementHost();
                ehost.Size = new System.Drawing.Size(23, 100);
                ehost.BackColor = Color.Green;
                ehost.Location = new Point(RollPitchRadioPanel.Size.Width / 2 - (ehost.Size.Width / 2), RollProgressBar.Location.Y + RollProgressBar.Size.Height + 10);
                ehost.Child = PitchProgressBar;
            
                Throttle
                ThrottleProgressBar = new System.Windows.Controls.ProgressBar();
                ThrottleProgressBar.Orientation = System.Windows.Controls.Orientation.Vertical;
                ThrottleProgressBar.Minimum = 750;
                ThrottleProgressBar.Maximum = 2250;
                ThrottleProgressBar.Value = 1500;
                ThrottleProgressBar.LargeChange = 10;       
                System.Windows.Forms.Integration.ElementHost ehostThrottleProgressBar = new System.Windows.Forms.Integration.ElementHost();
                ehostThrottleProgressBar.Size = new System.Drawing.Size(23, 100);
                ehostThrottleProgressBar.BackColor = Color.Green;
                ehostThrottleProgressBar.Location = new Point(ThrottleYawRadioPanel.Size.Width / 2 - (ehostThrottleProgressBar.Size.Width / 2), YawProgressBar.Location.Y + YawProgressBar.Size.Height + 10);
                ehostThrottleProgressBar.Child = ThrottleProgressBar;
             * 
             * 
                this.ThrottleYawRadioPanel.Controls.Add(ehostThrottleProgressBar);
                this.RollPitchRadioPanel.Controls.Add(ehost);
            */

            this.ThrottleYawRadioPanel.Controls.Add(ThrottleProgressBar);
            this.RollPitchRadioPanel.Controls.Add(PitchProgressBar);

            // graph plane
            pane = ZedGraphPane.GraphPane;
            pane.Title = "Graph Pane";
            pane.YAxis.Title = "Variables";
            pane.YAxis.TitleFontSpec.Size = 16;
            pane.YAxis.StepAuto = true;
            pane.XAxis.Title = "Time(s)";
            pane.XAxis.TitleFontSpec.Size = 16;
            pane.XAxis.ScaleFontSpec.Size = 16;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;
            pane.XAxis.MinorStep = 0.5;
            pane.XAxis.Step = 1;
            pane.Legend.FontSpec.Size = 16;
            pane.FontSpec.Size = 24;
        }
        public VerticalProgressBar PitchProgressBar;
        public VerticalProgressBar ThrottleProgressBar;
        //public System.Windows.Controls.ProgressBar PitchProgressBar;
        //public System.Windows.Controls.ProgressBar ThrottleProgressBar;
        
        GraphPane pane;
        LineItem line_RSSI;
        LineItem line_REMRSSI;
        LineItem line_NOISE;
        LineItem line_REMNOISE;
        LineItem line_ROLL;
        LineItem line_hilROLL;
        LineItem line_PITCH;
        LineItem line_hilPITCH;
        LineItem line_YAW;
        LineItem line_hilYAW;
        LineItem line_ROLLSPEED;
        LineItem line_PITCHSPEED;
        LineItem line_YAWSPEED;

        LineItem line_ALTM;
        LineItem line_CLIMBRATE;
        LineItem line_ACCELX;
        LineItem line_ACCELY;
        LineItem line_ACCELZ;


        FilterInfoCollection video_devices;
        VideoCaptureDevice final_video;
        private void SettingsControl_Load(object sender, EventArgs e)
        {
            this.Resize   += SettingsControl_Resize;
            this.BackColor = Color.Transparent;
            this.MainPanel.BackColor = Color.Transparent;
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            RollRateCheckBox.CheckedChanged +=  RollRateCheckBox_CheckedChanged;
            PitchRateCheckBox.CheckedChanged += PitchRateCheckBox_CheckedChanged;
            YawRateCheckBox.CheckedChanged += YawRateCheckBox_CheckedChanged;
            AllRatesCheckBox.CheckedChanged += AllRatesCheckBox_CheckedChanged;

            RollCheckBox.CheckedChanged += RollCheckBox_CheckedChanged;
            PitchCheckBox.CheckedChanged += PitchCheckBox_CheckedChanged;
            YawCheckBox.CheckedChanged += YawCheckBox_CheckedChanged;
            AhrsCheckBox.CheckedChanged += AhrsCheckBox_CheckedChanged;

            altitude_CheckBox.CheckedChanged  += altcheck_Box_CheckedChanged;
            climbrate_checkBox.CheckedChanged += climbrate_checkBox_CheckedChanged;
            accX_checkBox.CheckedChanged      += accX_checkBox_CheckedChanged;
            accY_checkBox.CheckedChanged      += accY_checkBox_CheckedChanged;
            accZ_checkBox.CheckedChanged      += accZ_checkBox_CheckedChanged;

            ZedGraphPane.MouseWheel += ZedGraphPane_MouseWheel;

            INS_Calibrate_Button.MouseClick += INS_Calibrate_Button_MouseClick;
            RadioCalibrateButton.MouseClick += RadioCalibrateButton_MouseClick;
            metadata_button.Click += metadata_button_Click;
            camera_button.Click += camera_button_Click;
            camera_comboBox.MouseEnter += camera_comboBox_MouseEnter;
            
            //log_button.Click += log_button_Click;

            video_devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            final_video = new VideoCaptureDevice();
        }

        private void camera_comboBox_MouseEnter(object sender, EventArgs e)
        {
            video_devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        
            camera_comboBox.Items.Clear();

            foreach (FilterInfo vf in video_devices)
            {
                camera_comboBox.Items.Add(vf.Name);
            }

            camera_comboBox.SelectedIndex = 0;
        }

        Form camera_form;
        PictureBox camera_pictureBox;
        private void camera_button_Click(object sender, EventArgs e)
        {
            if (camera_button.Text == "Camera On")
            {
                if (!final_video.IsRunning)
                {
                    try
                    {
                        final_video = new VideoCaptureDevice(video_devices[camera_comboBox.SelectedIndex].MonikerString);
                        final_video.NewFrame += new AForge.Video.NewFrameEventHandler(finalvideoSource_NewFrame);
                        final_video.Start();
                        camera_button.Text = "Camera Off";
                        camera_comboBox.Enabled = false;
                        
                        camera_form             = new Form();
                        camera_form.ClientSize = new System.Drawing.Size(640, 480);
                        camera_form.MaximumSize = new System.Drawing.Size(640, 480);
                        //camera_form.MinimumSize = new System.Drawing.Size(650, 650);
                        camera_form.FormClosing += camera_form_FormClosing;
                        camera_form.Resize += camera_form_Resize;

                        camera_form.Show();
                        camera_form.TopMost = true;

                        camera_pictureBox       = new PictureBox();
                        camera_pictureBox.Size  = camera_form.Size;
                       // camera_pictureBox.BackgroundImage.Size = camera_form.ClientSize;
                                               
                        camera_form.Controls.Add(camera_pictureBox);
                        camera_pictureBox.BackgroundImageLayout = ImageLayout.Stretch;

                    }
                    catch
                    {
                        Console.WriteLine("Some Camera Failure");
                    }         
                }
            }
            else
            {
                try
                {
                    camera_button.Text = "Camera On";
                    final_video.SignalToStop();
                    final_video.WaitForStop();
                    final_video.Stop();
                    camera_comboBox.Enabled = true;

                    if (camera_form != null)
                    {
                        camera_form.Hide();
                        camera_form = null;
                    }

                    if (camera_pictureBox != null)
                    {
                        camera_pictureBox = null;
                    }
                }
                catch
                {
                    Console.WriteLine("camera stopping error");
                }
            }
        }


        private void camera_form_Resize(object sender, EventArgs e)
        {
            camera_pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
        }


        private void camera_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            camera_button_Click(sender, e);
        }

      
        private void finalvideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            try
            {             
                Bitmap video = (Bitmap)e.Frame.Clone();
                camera_pictureBox.BackgroundImage = video;
            }
            catch
            {
                Console.WriteLine("finalVideo_Soure_NewFrame");
            }
        }


        Int32 count        = 0;
        /// <summary>
        /// Opens up the log file and creates a string array of time, lat,lon and a count on the number of log data available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string[,] log_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFld = new OpenFileDialog();
            string[,] log_data     = new string[10000, 4];          // more than 30 minutes worth of data.
            string filepath        = Application.StartupPath + Path.DirectorySeparatorChar + "Logs";
            if (!Directory.Exists(filepath))
            {
                filepath = Environment.CurrentDirectory;
            }
            openFld.InitialDirectory = filepath;
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                string file = openFld.FileName;
                if (file.EndsWith(".dat") || file.EndsWith(".txt"))
                {

                }
                else
                {
                    return null;
                }

                using (StreamReader sr = new StreamReader(openFld.OpenFile()))
                {
                    string line;
                    bool read = false;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (read == true)
                        {
                            count++;
                        }
                        read = true;
                    }

                    Console.WriteLine("Log Length: " + count);
                    if (count > 10000)
                    {
                        return null;
                    }
                    count = 0;
                    read  = false;
                    while((line = sr.ReadLine()) != null)
                    {
                        if (read == true)   
                        {
                            string[] data = line.Split('\t');
                            log_data[count, 0] = data[0]; // date
                            log_data[count, 1] = data[1]; // lat
                            log_data[count, 2] = data[2]; // lon
                            log_data[count, 3] = "FALSE"; // Inidicated Data hasn't been read
                            count++;
                        }
                        read = true;   
                    }
                }

            }
            return log_data;
        }
     

        /// <summary>
        /// Read all images, get the image index and 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metadata_button_Click(object sender, EventArgs e)
        {
            // load images get dates and times
            // get log data 
            string[,] log_data = log_button_Click( sender,  e);

            if (log_data == null){
                MessageBox.Show("Data is either above the specified limits or corrupted", "Alert");
                return;
            }

            FolderBrowserDialog openFld = new FolderBrowserDialog();
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                if (DialogResult.Cancel == MessageBox.Show("This action will process the metadata information in your images", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
                { return; }
                string[,] pic_data  = new string[1000, 2];
                string path         = openFld.SelectedPath;
                string [] files     = Directory.GetFiles(path);
                int len             = files.Length;
                for (int ic         = 0; ic < files.Length; ic++)
                {   
                    pic_data[ic, 0] = files[ic]; // unique path to image file 
                    pic_data[ic, 1] = "FALSE";   // means not geottages
                }   
                   
#if !APP1             
                DateTime[] logTime      = new DateTime[10000];
                for (int r = 0; r < count; r++)
                {
                    logTime[r] = Convert.ToDateTime(log_data[r, 0]);
                }   
#endif           
              
                for (int i = 0; i < len; i++)
                { 
                   if (pic_data[i, 0].EndsWith(".jpg") || pic_data[i, 0].EndsWith(".JPG") || pic_data[i, 0].EndsWith(".png") || pic_data[i, 0].EndsWith(".PNG")) 
                   {
                        using (FileStream fs = new FileStream(pic_data[i, 0], FileMode.Open, FileAccess.ReadWrite))
                        {
                            Image image = new Bitmap(fs);
                            if (image.PropertyIdList.Contains(36867) && image.PropertyIdList.Contains(1) && image.PropertyIdList.Contains(2))
                            {
                                PropertyItem propItem_ref = image.GetPropertyItem(1);
                                PropertyItem propItem_lat = image.GetPropertyItem(2);
                                PropertyItem propTime     = image.GetPropertyItem(36867);

                                string datetime = ASCIIEncoding.ASCII.GetString(propTime.Value);
                                string[] dates  = datetime.Split(' ');
                                string date     = dates[0];
                                string time     = dates[1];
                                //DateTime myDate = DateTime.ParseExact(dates[0], "yyyy:MM:dd", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime picTime = Convert.ToDateTime(time);
                              
#if !APP1                       
                                // For every log time go through this picture:
                                for (int index = 0; index < count; index++)
                                {
                                    if (log_data[index, 3] == "TRUE")
                                        continue;

                                    if (picTime.Hour == logTime[index].Hour && picTime.Minute == logTime[index].Minute && picTime.Second == logTime[index].Second)
                                    {

                                        float latitude = 0, longitude = 0;
                                        if (!float.TryParse(log_data[index, 1], out latitude) || !float.TryParse(log_data[index, 2], out longitude))
                                        {
                                            Console.WriteLine("SettingsControl: Log_data either longitude or latitude can't be converted to float");
                                            break; 
                                        }

                                        log_data[index, 3] = "TRUE";
                                        
                                        var refLatitude    = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        refLatitude.Id     = 0x0001;
                                        refLatitude.Type   = 2;
                                        refLatitude.Len    = 2;
                                        refLatitude.Value  = refLat(latitude);                                      
                                        image.SetPropertyItem(refLatitude);


                                        var gpsLatitude  = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        gpsLatitude.Id   = 0x0002;
                                        gpsLatitude.Type = 5;
                                        gpsLatitude.Len  = 3;
                                        image.SetPropertyItem(gpsLatitude);

                                        string GeotaggedFile = Environment.CurrentDirectory + Path.DirectorySeparatorChar +  "Geottaged" + Path.DirectorySeparatorChar + "Geo_" + index;
                                        image.Save(GeotaggedFile);
                                        //image.Save();
                                        break;
                                    }
                                }
#endif
                                //Console.WriteLine(dates[1]);
                                //double gps_lat = ExifGpsToDouble(propItem_ref, propItem_lat);
                                //string Time    = ExifTimeToDouble(propTime);
                                //Console.WriteLine(Time);
                                //Console.WriteLine(gps_lat);
                            }
                        }
                    }
                }
            }
        }


        private byte[] refLat(float lat)
        {
            string _ref = "";
            if (lat < 0)
                _ref = "S";
            else
            _ref = "N";

            _ref = _ref + "\0";

            return Encoding.ASCII.GetBytes(_ref);
        }

        private byte[] refLon(float lon)
        {
            string _ref = "";
            if (lon < 0)
                _ref = "W";
            else
                _ref = "E";

            _ref = _ref + "\0";

            return Encoding.ASCII.GetBytes(_ref);
        }

        private byte[] ddmmss(float angle)
        {
           // string _ref = "";

            if (angle < -180.0)
                angle += 360.0F;

            if (angle > 180.0)
                angle -= 360.0F;

            if (angle < 0)
                angle = Math.Abs(angle);

            int degrees = (int)Math.Floor(angle);

            var delta   = angle - degrees;

            var seconds = (int)Math.Floor(3600.0 * delta);

            int second  = seconds % 60;

            int minute  = (int)Math.Floor(seconds / 60.0) % 60;

            byte[] _ref = new byte[3];

            //Convert.ToByte(_ref,2);

            //Convert.ToByte(degrees, 2);

            // degrees = degrees + ;

            _ref[0] = Convert.ToByte(degrees);

            BitConverter.GetBytes((UInt32)degrees);

           // BitArray b = new BitArray();


            BitmapMetadata mtd = new BitmapMetadata("png");
           // mtd.Location

           // Bitmap img = new Bitmap(new Image());
          
            return _ref;
        }

        private static double ExifGpsToDouble(PropertyItem propItemRef, PropertyItem propItem)
        {
            double degreesNumerator = BitConverter.ToUInt32(propItem.Value, 0);
            double degreesDenominator = BitConverter.ToUInt32(propItem.Value, 4);
            double degrees = degreesNumerator / (double)degreesDenominator;

            double minutesNumerator = BitConverter.ToUInt32(propItem.Value, 8);
            double minutesDenominator = BitConverter.ToUInt32(propItem.Value, 12);
            double minutes = minutesNumerator / (double)minutesDenominator;

            double secondsNumerator = BitConverter.ToUInt32(propItem.Value, 16);
            double secondsDenominator = BitConverter.ToUInt32(propItem.Value, 20);
            double seconds = secondsNumerator / (double)secondsDenominator;


            double coorditate = degrees + (minutes / 60d) + (seconds / 3600d);
            string gpsRef = System.Text.Encoding.ASCII.GetString(new byte[1] { propItemRef.Value[0] }); //N, S, E, or W
            if (gpsRef == "S" || gpsRef == "W")
                coorditate = coorditate * -1;
            return coorditate;
        }

        private static string ExifTimeToDouble(PropertyItem propItem)
        {
            double Hour     = BitConverter.ToUInt32(propItem.Value, 0);
            double Minute   = BitConverter.ToUInt32(propItem.Value, 8);
            double Second   = BitConverter.ToUInt32(propItem.Value, 16);
            string time     = Hour.ToString() + ":" +Minute.ToString() + ":"+ Second.ToString();

            return time;
        }

        private void INS_Calibrate_Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Are you sure you want to calibrate the accelerometer ?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
            {
                INS_Calibration.AccelCalibration AccelCalibration_Class = new INS_Calibration.AccelCalibration();
                AccelCalibration_Class.Sample_Processing();
            }
        }



        private void RadioCalibrateButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Are you sure you want to calibrate the radio"+'\n'+"if so, move controls to their extreme after pressing ok for 5 seconds ?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
            {
                int iterations = 50;
                ushort max_chan1, max_chan2, max_chan3, max_chan4, max_chan5;
                ushort min_chan1, min_chan2, min_chan3, min_chan4, min_chan5;
                
                max_chan1 = max_chan2 = max_chan3 = max_chan4 = max_chan5 = 1500;
                min_chan1 = min_chan2 = min_chan3 = min_chan4 = min_chan5 = 1500;

                while (iterations-- > 0)
                {
                    max_chan1 = Math.Max(max_chan1, Variables.chan1);
                    max_chan2 = Math.Max(max_chan2, Variables.chan2);
                    max_chan3 = Math.Max(max_chan3, Variables.chan3);
                    max_chan4 = Math.Max(max_chan4, Variables.chan4);
                    max_chan5 = Math.Max(max_chan5, Variables.chan5);

                    min_chan1 = Math.Min(min_chan1, Variables.chan1);
                    min_chan2 = Math.Min(min_chan2, Variables.chan2);
                    min_chan3 = Math.Min(min_chan3, Variables.chan3);
                    min_chan4 = Math.Min(min_chan4, Variables.chan4);
                    min_chan5 = Math.Min(min_chan5, Variables.chan5);   

                    System.Threading.Thread.Sleep(100);
                }

                MessageBox.Show(
               "Radio Calibration Sucessful" 
                +
                '\n'
                +
                "Roll:        " + max_chan1 + "  **  " + min_chan1
                +
                '\n'
                +
                "Pitch:     " + max_chan2 + "  **  " + min_chan2
                +
                '\n'
                +
                "Throttle:" + max_chan3 + "  **  " + min_chan3
                +
                '\n'
                +
                "Yaw:       " + max_chan4 + "  **  " + min_chan4, "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk
                );
            }
        }


        private void ZedGraphPane_MouseWheel(object sender, MouseEventArgs e)
        {
            int mousemove = e.Delta / 120;
            if (mousemove > 0)
            {
                if (pane.YAxis.Max > 0.5)
                    pane.YAxis.Max -= 1;
                if (pane.YAxis.Min < -0.5)
                    pane.YAxis.Min += 1;
            }
            else
            {
                pane.YAxis.Max += 1;
                pane.YAxis.Min -= 1;
            }
        }

        Int64 x = -1;
        Int64 y = -1;


        private void AllRatesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RollRateCheckBox.Checked = PitchRateCheckBox.Checked = YawRateCheckBox.Checked = AllRatesCheckBox.Checked;
        }

        private void AhrsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RollCheckBox.Checked = PitchCheckBox.Checked = YawCheckBox.Checked = AhrsCheckBox.Checked;
        }

        private void altcheck_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (!altitude_CheckBox.Checked)
            {
                line_ALTM.Clear();
                pane.CurveList.Remove(line_ALTM);
            }
            else
            {
                //ControlCollection cntrl_collection = new ControlCollection();
                foreach (Control cntrl in GraphGroupBox.Controls)
                {
                    if (cntrl.GetType() == typeof(CheckBox))
                    {
                        if ((cntrl  as CheckBox)!= (sender as CheckBox))
                            (cntrl as CheckBox).Checked = false;
                    }
                }
                //pane.CurveList.Clear();
                line_ALTM = pane.AddCurve("Altitude (m)", new PointPairList(), Color.Green, SymbolType.None);  
            }
        }

        private void climbrate_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!climbrate_checkBox.Checked)
            {
                line_CLIMBRATE.Clear();
                pane.CurveList.Remove(line_CLIMBRATE);
            }
            else
            {
                foreach (Control cntrl in GraphGroupBox.Controls)
                {
                    if (cntrl.GetType() == typeof(CheckBox))
                    {
                        if ((cntrl as CheckBox) != (sender as CheckBox))
                            (cntrl as CheckBox).Checked = false;
                    }
                }
                //pane.CurveList.Clear();
                line_CLIMBRATE = pane.AddCurve("ClimbRate (cm/s)", new PointPairList(), Color.Red, SymbolType.None);
            }
        }

        private void accX_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!accX_checkBox.Checked)
            {
                line_ACCELX.Clear();
                pane.CurveList.Remove(line_ACCELX);
            }
            else
            {
                if (accY_checkBox.Checked || accZ_checkBox.Checked)
                {
                    // Just add without clearing
                    line_ACCELX = pane.AddCurve("ACCELX (m/s2)", new PointPairList(), Color.Black, SymbolType.None);
                }
                else
                {
                    // clear fist
                    // pane.CurveList.Clear();
                    line_ACCELX = pane.AddCurve("ACCELX (m/s2)", new PointPairList(), Color.Black, SymbolType.None);
                }

                foreach (Control cntrl in GraphGroupBox.Controls)
                {
                    if (cntrl.GetType() == typeof(CheckBox))
                    {
                        if ((cntrl as CheckBox) != (sender as CheckBox))
                            (cntrl as CheckBox).Checked = false;
                    }
                }
            }
        }

        private void accY_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!accY_checkBox.Checked)
            {
                line_ACCELY.Clear();
                pane.CurveList.Remove(line_ACCELY);
            }
            else
            {
                if (accX_checkBox.Checked || accZ_checkBox.Checked)
                {
                    // Just add without clearing
                    line_ACCELY = pane.AddCurve("ACCELY (m/s2)", new PointPairList(), Color.Blue, SymbolType.None);
                }
                else
                {
                    // clear fist
                    //pane.CurveList.Clear();
                    line_ACCELY = pane.AddCurve("ACCELY (m/s2)", new PointPairList(), Color.Blue, SymbolType.None);
                }

                foreach (Control cntrl in GraphGroupBox.Controls)
                {
                    if (cntrl.GetType() == typeof(CheckBox))
                    {
                        if ((cntrl as CheckBox) != (sender as CheckBox))
                            (cntrl as CheckBox).Checked = false;
                    }
                }
            }
        }

        private void accZ_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!accZ_checkBox.Checked)
            {
                line_ACCELZ.Clear();
                pane.CurveList.Remove(line_ACCELZ);
            }
            else
            {
                if (accX_checkBox.Checked || accY_checkBox.Checked)
                {
                    // Just add without clearing
                    line_ACCELZ = pane.AddCurve("ACCELZ (m/s2)", new PointPairList(), Color.Brown, SymbolType.None);
                }
                else
                {
                    // clear fist
                    //pane.CurveList.Clear();
                    line_ACCELZ = pane.AddCurve("ACCELZ (m/s2)", new PointPairList(), Color.Brown, SymbolType.None);
                }

                foreach (Control cntrl in GraphGroupBox.Controls)
                {
                    if (cntrl.GetType() == typeof(CheckBox))
                    {
                        if ((cntrl as CheckBox) != (sender as CheckBox))
                            (cntrl as CheckBox).Checked = false;
                    }
                }
            }
        }  

        private void RollCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!RollCheckBox.Checked)
            {
                line_ROLL.Clear();
                line_hilROLL.Clear();

                pane.CurveList.Remove(line_ROLL);
                pane.CurveList.Remove(line_hilROLL);
                return;
            }

            if (PitchCheckBox.Checked || YawCheckBox.Checked)
            {
                line_ROLL = pane.AddCurve("ROLL", new PointPairList(), Color.Red, SymbolType.None);
                line_hilROLL = pane.AddCurve("hil ROLL", new PointPairList(), Color.Green, SymbolType.None);
            }
            else
            {
                //pane.CurveList.Clear();
                line_ROLL    = pane.AddCurve("ROLL", new PointPairList(), Color.Red, SymbolType.None);
                line_hilROLL = pane.AddCurve("hil ROLL", new PointPairList(), Color.Green, SymbolType.None);
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 3.14192;
            pane.YAxis.Min = -3.14192;
        }


        private void PitchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!PitchCheckBox.Checked)
            {
                line_PITCH.Clear();
                pane.CurveList.Remove(line_PITCH);

                line_hilPITCH.Clear();
                pane.CurveList.Remove(line_hilPITCH);
                return;
            }
            if (RollCheckBox.Checked || YawCheckBox.Checked)
            {
                line_PITCH = pane.AddCurve("PITCH", new PointPairList(), Color.Brown, SymbolType.None);
                line_hilPITCH = pane.AddCurve("hil PITCH", new PointPairList(), Color.Orange, SymbolType.None);
            }
            else
            {
                //pane.CurveList.Clear();
                line_PITCH = pane.AddCurve("PITCH", new PointPairList(), Color.Brown, SymbolType.None);
                line_hilPITCH = pane.AddCurve("hil PITCH", new PointPairList(), Color.Orange, SymbolType.None);
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 3.14192;
            pane.YAxis.Min = -3.14192;
        }


        private void YawCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!YawCheckBox.Checked)
            {
                line_YAW.Clear();
                pane.CurveList.Remove(line_YAW);
                                line_hilYAW.Clear();
                pane.CurveList.Remove(line_hilYAW);
                return;
            }

            if (RollCheckBox.Checked || PitchCheckBox.Checked)
            {
                line_YAW = pane.AddCurve("YAW", new PointPairList(), Color.Yellow, SymbolType.None);
                 line_hilYAW = pane.AddCurve("hil YAW", new PointPairList(), Color.Black, SymbolType.None);
            }
            else
            {
                //pane.CurveList.Clear();
                line_YAW = pane.AddCurve("YAW", new PointPairList(), Color.Yellow, SymbolType.None);
                line_hilYAW = pane.AddCurve("hil YAW", new PointPairList(), Color.Black, SymbolType.None);
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 3.14192;
            pane.YAxis.Min = -3.14192;
        }


        private void RollRateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!RollRateCheckBox.Checked)
            {
                line_ROLLSPEED.Clear();
                pane.CurveList.Remove(line_ROLLSPEED);
                return;
            }

            if(PitchRateCheckBox.Checked || YawRateCheckBox.Checked)
            {
                line_ROLLSPEED = pane.AddCurve("ROLL RATE", new PointPairList(), Color.Green, SymbolType.None);         
            }
            else
            {
                //pane.CurveList.Clear();
                line_ROLLSPEED = pane.AddCurve("ROLL RATE", new PointPairList(), Color.Green, SymbolType.None);         
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 6.29;
            pane.YAxis.Min = -6.29;
        }

        private void PitchRateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!PitchRateCheckBox.Checked)
            {
                line_PITCHSPEED.Clear();
                pane.CurveList.Remove(line_PITCHSPEED);
                return;
            }
            if (RollRateCheckBox.Checked || YawRateCheckBox.Checked)
            {
                line_PITCHSPEED = pane.AddCurve("PITCH RATE", new PointPairList(), Color.Blue, SymbolType.None);
            }
            else
            {
                //pane.CurveList.Clear();
                line_PITCHSPEED = pane.AddCurve("PITCH RATE", new PointPairList(), Color.Blue, SymbolType.None);
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 6.29;
            pane.YAxis.Min = -6.29;

        }

        private void YawRateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!YawRateCheckBox.Checked)
            {
                line_YAWSPEED.Clear();
                pane.CurveList.Remove(line_YAWSPEED);
                return;
            }

            if (RollRateCheckBox.Checked || PitchRateCheckBox.Checked)
            {
                line_YAWSPEED = pane.AddCurve("YAW RATE", new PointPairList(), Color.Black, SymbolType.None);
            }
            else
            {
                //pane.CurveList.Clear();
                line_YAWSPEED = pane.AddCurve("YAW RATE", new PointPairList(), Color.Black, SymbolType.None);
            }

            foreach (Control cntrl in GraphGroupBox.Controls)
            {
                if (cntrl.GetType() == typeof(CheckBox))
                {
                    if ((cntrl as CheckBox) != (sender as CheckBox))
                        (cntrl as CheckBox).Checked = false;
                }
            }

            x = 0;
            pane.XAxis.Min = 0;
            pane.XAxis.Max = 10;

            pane.YAxis.MaxAuto = false;
            pane.YAxis.MinAuto = false;
            pane.YAxis.Max = 6.29;
            pane.YAxis.Min = -6.29;
        }


        public void GraphPane_Draw(float[] values)
        {
            x++;
            y++;

            double time = (double)(x / 20.0);
            if (time > pane.XAxis.Max)
            {
                pane.XAxis.Max = time + pane.XAxis.Step;
                pane.XAxis.Min = pane.XAxis.Max - 10;
            }

            // AHRS ROLLRAD
            if (RollCheckBox.Checked)
            {
                if (line_ROLL.Points.Count > 210)
                    line_ROLL.Points.Remove(0);
                line_ROLL.AddPoint(time, values[3]);

                if (line_hilROLL.Points.Count > 210)
                    line_hilROLL.Points.Remove(0);
                line_hilROLL.AddPoint(time, values[6]);
            }

            // AHRS PITCHRAD
            if(PitchCheckBox.Checked)
            {
                if (line_PITCH.Points.Count > 210)
                    line_PITCH.Points.Remove(0);
                line_PITCH.AddPoint(time, values[4]);

                if (line_hilPITCH.Points.Count > 210)
                    line_hilPITCH.Points.Remove(0);
                line_hilPITCH.AddPoint(time, values[7]);
            }

            // AHRS YAWRAD
            if(YawCheckBox.Checked)
            {
                if (line_YAW.Points.Count > 210)
                    line_YAW.Points.Remove(0);
                line_YAW.AddPoint(time, values[5]);

                if (line_hilYAW.Points.Count > 210)
                    line_hilYAW.Points.Remove(0);
                line_hilYAW.AddPoint(time, values[8]);
            }
            


            // AHRS ROLLRATE RPS
            if (RollRateCheckBox.Checked)
            {
                if (line_ROLLSPEED.Points.Count > 210)
                    line_ROLLSPEED.Points.Remove(0);
                line_ROLLSPEED.AddPoint(time, values[0]);
            }

            // AHRS PITCHRATE RPS
            if (PitchRateCheckBox.Checked)
            {
                if (line_PITCHSPEED.Points.Count > 210)
                    line_PITCHSPEED.Points.Remove(0);
                line_PITCHSPEED.AddPoint(time, values[1]);
            }

            // AHRS YAWRATE RPS
            if (YawRateCheckBox.Checked)
            {
                if (line_YAWSPEED.Points.Count > 210)
                    line_YAWSPEED.Points.Remove(0);
                line_YAWSPEED.AddPoint(time, values[2]);
            }          

            // AHRS ALTITUDE
            if (altitude_CheckBox.Checked)
            {
                if (line_ALTM.Points.Count > 210)
                    line_ALTM.Points.Remove(0);
                line_ALTM.AddPoint(time, values[9]);
            }

            // AHRS CLIMBRATE
            if (climbrate_checkBox.Checked)
            {
                if (line_CLIMBRATE.Points.Count > 210)
                    line_CLIMBRATE.Points.Remove(0);
                line_CLIMBRATE.AddPoint(time, values[10]);
            }

            // AHRS XACCELERATION
            if (accX_checkBox.Checked)
            {
                if (line_ACCELX.Points.Count > 210)
                    line_ACCELX.Points.Remove(0);
                line_ACCELX.AddPoint(time, values[11]);
            }

            // AHRS YACCELERATION
            if (accY_checkBox.Checked)
            {
                if (line_ACCELY.Points.Count > 210)
                    line_ACCELY.Points.Remove(0);
                line_ACCELY.AddPoint(time, values[12]);
            }

            // AHRS ZACCELERATION
            if (accZ_checkBox.Checked)
            {
                if (line_ACCELZ.Points.Count > 210)
                    line_ACCELZ.Points.Remove(0);
                line_ACCELZ.AddPoint(time, values[13]);
            }

            // invalidate GraphPane
            ZedGraphPane.AxisChange();
            ZedGraphPane.Invalidate();
        }

        public void ServoOutputs_Draw(ushort chan1, ushort chan2, ushort chan3, ushort chan4)
        {
            if (chan1 > 750 && chan1 < 2250)
                RollProgressBar.Value = chan1;
            else
                RollProgressBar.Value = 750;

            if (chan2 > 750 && chan2 < 2250)
                PitchProgressBar.Value = chan2;
            else
                PitchProgressBar.Value = 750;

            if (chan3 > 750 && chan3 < 2250)
                ThrottleProgressBar.Value = chan3;
            else
                ThrottleProgressBar.Value = 750;

            if (chan4 > 750 && chan4 < 2250)
                YawProgressBar.Value = chan4;
            else
                YawProgressBar.Value = 750;

            RollLabel.Text = chan1.ToString();
            PtchLabel.Text = chan2.ToString();
            ThrLabel.Text = chan3.ToString();
            YawLabel.Text = chan4.ToString();
        }

        private void SettingsControl_Resize(object sender, EventArgs e)
        {          
            MainPanel.Size = new Size(Size.Width - 1, Size.Height - 1);         
        }

        private void SettingsControl_Disposed(object sender, EventArgs e)
        {
            if (final_video != null)
            {

                if (final_video.IsRunning)
                {
                    final_video.SignalToStop();
                    final_video.WaitForStop();
                    final_video.Stop();
                    camera_pictureBox = null;
                    camera_form       = null;
                }
            }
            // check boxex
            RollRateCheckBox.Dispose();
            PitchRateCheckBox.Dispose();
            YawRateCheckBox.Dispose();


            if(line_ROLL != null)
            {
                line_ROLL.Clear();
                line_ROLL = null;
            }

            if (line_hilROLL != null)
            {
                line_hilROLL.Clear();
                line_hilROLL = null;
            }

            if (line_PITCH != null)
            {
                line_PITCH.Clear();
                line_PITCH = null;
            }

            if (line_hilPITCH != null)
            {
                line_hilPITCH.Clear();
                line_hilPITCH = null;
            }


            if(line_YAW != null)
            {
                line_YAW.Clear();
                line_YAW = null;
            }

            if (line_hilYAW != null)
            {
                line_hilYAW.Clear();
                line_hilYAW = null;
            }

            if (line_ROLLSPEED != null)
            {
                line_ROLLSPEED.Clear();
                line_ROLLSPEED = null;
            }

            if (line_YAWSPEED != null)
            {
                line_YAWSPEED.Clear();
                line_YAWSPEED = null;
            }

            if (line_PITCHSPEED != null)
            {
                line_PITCHSPEED.Clear();
                line_PITCHSPEED = null;
            }

            if (line_ALTM != null)
            {
                line_ALTM.Clear();
                line_ALTM = null;
            }

            if (line_CLIMBRATE != null)
            {
                line_CLIMBRATE.Clear();
                line_CLIMBRATE = null;
            }

            if (line_ACCELX != null)
            {
                line_ACCELX.Clear();
                line_ACCELX = null;
            }

            if (line_ACCELY != null)
            {
                line_ACCELY.Clear();
                line_ACCELY = null;
            }

            if (line_ACCELZ != null)
            {
                line_ACCELZ.Clear();
                line_ACCELZ = null;
            }

            ZedGraphPane.Dispose();
            ZedGraphPane = null;

            GraphGroupBox.Dispose();
            GraphGroupBox = null;

            RadioGroupBox.Dispose();
            RadioGroupBox = null;

            HilGroupBox.Dispose();
            HilGroupBox = null;

            MainPanel.Dispose();
            MainPanel = null;

            PitchProgressBar = null;
            YawProgressBar      = null;
            ThrottleProgressBar = null;
            RollProgressBar = null;
        }
    }
}
