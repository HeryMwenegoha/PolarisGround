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
using System.IO.Ports;
using ZedGraph;
using UGCS3.Common;
using System.Windows.Media.Imaging;

using AForge;
using AForge.Video.DirectShow;


namespace UGCS3.UsableControls
{
    public partial class SettingsControl : UserControl
    {
        public VerticalProgressBar PitchProgressBar;
        public VerticalProgressBar ThrottleProgressBar;
        GraphPane pane;
        LineItem[] Line;
        FilterInfoCollection video_devices;
        VideoCaptureDevice final_video;
        ComboBox zedComboBox;
        public bool loaded = false;
        Form camera_form;
        PictureBox camera_pictureBox;

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

            // graph plane
            pane                        = ZedGraphPane.GraphPane;
            pane.YAxis.Title.Text       = "Variables";
            pane.XAxis.Type             = AxisType.Date;
            pane.XAxis.Title.Text       = "Time (HH:mm:ss)";
            pane.XAxis.Scale.Format     = "HH:mm:ss";
            pane.XAxis.Scale.MajorUnit  = DateUnit.Second;
            pane.XAxis.Scale.MinorUnit  = DateUnit.Millisecond;
            pane.XAxis.Scale.Min        = DateTime.Now.ToOADate();//.Subtract(new TimeSpan(0, 0, 10, 0, 0)).ToOADate();
            pane.XAxis.Scale.Max        = DateTime.Now.Add(new TimeSpan(0, 0, 0,0,30)).ToOADate();
            pane.Legend.FontSpec.Size   = 16;

            this.ThrottleYawRadioPanel.Controls.Add(ThrottleProgressBar);
            this.RollPitchRadioPanel.Controls.Add(PitchProgressBar);
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            this.Resize   += SettingsControl_Resize;
            this.BackColor = Color.Transparent;
            this.MainPanel.BackColor = Color.Transparent;
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            object[] zeditems = new object[]
            {
                "Euler(rad)",
                "Roll(rad)",
                "Pitch(rad)",
                "Yaw(rad)",
                "EulerRates(rad/sec)",
                "RollRate(rad/sec)",
                "PitchRate(rad/sec)",
                "YawRate(rad/sec)",
                "Altitude(m)",
                "Climbrate(cm/s)",
                "Speed(m/s)",
                "Airspeed(m/s)",
                "Groundspeed(m/s)",
                "Accel(m/s/s)",
                "Xacc(m/s/s)",
                "Yacc(m/s/s)",
                "Zacc(m/s/s)",
            };

            zedComboBox = new ComboBox();
            ZedGraphPane.Controls.Add(zedComboBox);
            ZedGraphPane.MouseWheel += ZedGraphPane_MouseWheel;
            zedComboBox.Items.AddRange(zeditems);
            zedComboBox.SelectedIndex = 0;
            zedComboBox.SelectedIndexChanged += ZedComboBox_SelectedIndexChanged;
            ZedGraphPane.GraphPane.Title.Text = zedComboBox.Items[0].ToString();
            Line    = new LineItem[3];
            Line[0] = pane.AddCurve("Variable1", new PointPairList(), Color.Red, SymbolType.None);
            Line[1] = pane.AddCurve("Variable2", new PointPairList(), Color.Blue, SymbolType.None);
            Line[2] = pane.AddCurve("Variable3", new PointPairList(), Color.Green, SymbolType.None);

            INS_Calibrate_Button.MouseClick += INS_Calibrate_Button_MouseClick;
            RadioCalibrateButton.MouseClick += RadioCalibrateButton_MouseClick;
            metadata_button.Click += metadata_button_Click;
            camera_button.Click += camera_button_Click;
            camera_comboBox.MouseEnter += camera_comboBox_MouseEnter;
            
            log_button.Click += log_button_Click;

            video_devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            final_video = new VideoCaptureDevice();

            loaded = true;
        }

        private void ZedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // combobox item
            ComboBox cmb = sender as ComboBox;

            // setup lines
            switch (cmb.Items[cmb.SelectedIndex].ToString())
            {
                case "Euler(rad)":
                case "EulerRates(rad/sec)":
                case "Accel(m/s/s)":
                    Line[0].Label.Text = "X";
                    Line[1].Label.Text = "Y";
                    Line[2].Label.Text = "Z";
                    break;

                case "Speed(m/s)":
                    Line[0].Label.Text = "Air";
                    Line[1].Label.Text = "Ground";
                    break;

                default:
                    Line[0].Label.Text = cmb.Items[cmb.SelectedIndex].ToString();
                    break;
            }

            // xetup axis
            switch(cmb.Items[cmb.SelectedIndex].ToString())
            {
                case "Euler(rad)":
                case "EulerRates(rad/sec)":
                case "RollRate(rad/sec)":
                case "PitchRate(rad/sec)":
                case "YawRate(rad/sec)":
                case "Roll(rad)":
                case "Pitch(rad)":
                case "Yaw(rad)":
                     pane.YAxis.Scale.Max = 3.14192;
                     pane.YAxis.Scale.Min = -3.14192;      
                    break;

                default:
                    pane.YAxis.Scale.MaxAuto = true;
                    pane.YAxis.Scale.MinAuto = true;
                    break;
            }

            // house keeping       
            ZedGraphPane.GraphPane.Title.Text = cmb.Items[cmb.SelectedIndex].ToString();
            ZedGraphPane.Invalidate();
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

        private void log_button_Click(object sender, EventArgs e)
        {
#if TEST1
            FolderBrowserDialog openFld = new FolderBrowserDialog();
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                if (DialogResult.Cancel == MessageBox.Show("This action will process the metadata information in your images", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
                { return; }
                string[,] pic_data = new string[1000, 2];
                string path = openFld.SelectedPath;
                string[] files = Directory.GetFiles(path);
                int len = files.Length;
                for (int ic = 0; ic < files.Length; ic++)
                {
                    pic_data[ic, 0] = files[ic]; // unique path to image file 
                    pic_data[ic, 1] = "FALSE";   // means not geottages
                }


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
                                PropertyItem propTime = image.GetPropertyItem(36867);

                                string datetime = ASCIIEncoding.ASCII.GetString(propTime.Value);
                                string[] dates = datetime.Split(' ');
                                string date = dates[0];
                                string time = dates[1];
                                //DateTime myDate = DateTime.ParseExact(dates[0], "yyyy:MM:dd", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime picTime = Convert.ToDateTime(time);

                                Console.WriteLine(picTime);

                            }
                        }
                    }
                }
            }
#endif

#if !TEST2
            FolderBrowserDialog openFld = new FolderBrowserDialog();
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                if (DialogResult.Cancel == MessageBox.Show("This action will process the metadata information in your images", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
                { return; }
                string[,] pic_data = new string[1000, 2];
                string path = openFld.SelectedPath;
                string[] files = Directory.GetFiles(path);
                int len = files.Length;
                for (int ic = 0; ic < files.Length; ic++)
                {
                    pic_data[ic, 0] = files[ic]; // unique path to image file 
                    pic_data[ic, 1] = "FALSE";   // means not geottages
                }

                for (int i = 0; i < len; i++)
                {
                    if (pic_data[i, 0].EndsWith(".jpg") || pic_data[i, 0].EndsWith(".JPG") || pic_data[i, 0].EndsWith(".png") || pic_data[i, 0].EndsWith(".PNG"))
                    {

                        using (FileStream fs = new FileStream(pic_data[i, 0], FileMode.Open, FileAccess.ReadWrite))
                        {
                            //Image image = new Bitmap(fs);
                            using (Image image = Image.FromStream(fs))
                            {
                                if (image.PropertyIdList.Contains(36867) && image.PropertyIdList.Contains(1) && image.PropertyIdList.Contains(2))
                                {
                                    //Console.WriteLine("Processing..." + pic_data[i, 0]);

                                    PropertyItem propItem_ref = image.GetPropertyItem(1);
                                    PropertyItem propItem_lat = image.GetPropertyItem(2);
                                    PropertyItem propTime = image.GetPropertyItem(36867);


                                    double gps_lat = ExifGpsToDouble(propItem_ref, propItem_lat);
                                    //double gps_lat = ExifGpsToDouble(propItem_ref, propItem_lat);
                                    string Time    = ExifTimeToDouble(propTime);
                                    Console.WriteLine(gps_lat);
                                }
                            }
                        }
                    }
                }



            }
#endif
        }


        Int32 count        = 0;
        /// <summary>
        /// Opens up the log file and creates a string array of time, lat,lon and a count on the number of log data available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        /// FORMAT: DATE    LATITUDE    LONGITUDE   HAMSL  HAFL CLMRATE GSPEED  ASPEED  MAV%    RSSI%   THROTTLE    ROLLDEG PITCHDEG    YAWDEG
        string logname = "";
        private string[,] log_information(object sender, EventArgs e)
        {
            OpenFileDialog openFld = new OpenFileDialog();
            string[,] log_data     = new string[30000, 5];          // more than 30 minutes worth of data.
            string filepath        = Application.StartupPath + Path.DirectorySeparatorChar + "Logs";
            if (!Directory.Exists(filepath))
            {
                filepath = Environment.CurrentDirectory;
            }
            openFld.InitialDirectory = filepath;
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                string file = openFld.FileName;
                logname     = openFld.SafeFileName; // does  not include path but has the extension
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

                    if (count > 30000)
                    {
                        return null;
                    }
                }


                using (StreamReader sr = new StreamReader(openFld.OpenFile()))
                {
                    string line;
                    bool read = false;
                    count     = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (read == true)
                        {
                            string[] data = line.Split('\t');
                            log_data[count, 0] = data[0]; // date
                            log_data[count, 1] = data[1]; // lat
                            log_data[count, 2] = data[2]; // lon
                            log_data[count, 3] = "FALSE"; // Inidicated Data hasn't been read
                            log_data[count, 4] = data[3]; // Inidicated Data hasn't been read
                            count++;
                        }
                        read = true;
                    }
                }



            }

            Console.WriteLine("Log data read successfully "+count);
            return log_data;
        }

        
        public List<GMap.NET.PointLatLng> plotLatLng_button_Clicked(object sender, EventArgs e)
        {
            string[,] log_data = log_information(sender, e);

            if (log_data == null)
            {
                MessageBox.Show("Data is either above the specified limits or corrupted", "Alert");
                return null;
            }

            List<GMap.NET.PointLatLng> points = new List<GMap.NET.PointLatLng>();

           
            for (Int32 i = 0; i < count; i++)
            {
                points.Add(new GMap.NET.PointLatLng(Convert.ToDouble(log_data[i, 1]), Convert.ToDouble(log_data[i, 2])));
            }

            return points;
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
            string[,] log_data = log_information(sender, e);

            if (log_data == null){
                MessageBox.Show("Data is either above the specified limits or corrupted", "Alert");
                return;
            }

            FolderBrowserDialog openFld = new FolderBrowserDialog();
            if (openFld.ShowDialog() == DialogResult.OK)
            {
                if (DialogResult.Cancel == MessageBox.Show("This action will process the metadata information in your images", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk))
                { return; }
                string[,] pic_data  = new string[30000, 2];
                string path         = openFld.SelectedPath;
                string [] files     = Directory.GetFiles(path);
                int len             = files.Length;


                for (int ic         = 0; ic < files.Length; ic++)
                {   
                    pic_data[ic, 0] = files[ic]; // unique path to image file 
                    pic_data[ic, 1] = "FALSE";   // means not geottages
                }   
                   
#if !APP1             
                DateTime[] logTime      = new DateTime[30000];
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
                            //Image image = new Bitmap(fs);
                            using(Image image = Image.FromStream(fs))
                            {
                            if (image.PropertyIdList.Contains(36867))// && image.PropertyIdList.Contains(1) && image.PropertyIdList.Contains(2))
                            {
                                //PropertyItem propItem_ref = image.GetPropertyItem(1);
                                //PropertyItem propItem_lat = image.GetPropertyItem(2);
                                PropertyItem propTime     = image.GetPropertyItem(36867);

                                string datetime = ASCIIEncoding.ASCII.GetString(propTime.Value);
                                string[] dates  = datetime.Split(' ');
                                string date = "", time = "";

                                if (dates.Length > 0)
                                    date = dates[0];
                                if(dates.Length > 1)
                                    time  = dates[1];
                                
                                if(string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                                    continue;
                             
                                // DateTime myDate = DateTime.ParseExact(dates[0], "yyyy:MM:dd", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime picTime = Convert.ToDateTime(time);

                                Int32 offset_millis = 0;
                                Int32.TryParse(TimeOffsetTextBox.Text, out offset_millis);
                                picTime = picTime.AddMilliseconds(offset_millis);

                                // picTime = picTime.AddMinutes(-235 + 6);

                                Console.Write("Processing..." + (i +1)+ "/" + len + "       ");// + pic_data[i, 0]);
                                Console.WriteLine(datetime + "      " + date + "      " + picTime.ToLongTimeString());  
#if !APP1                       
                                // For every log time go through this picture:
                                for (int index = 0; index < count; index++)
                                {
                                    if (log_data[index, 3] == "TRUE")
                                        continue;

                                 
                                    if (picTime.Hour == logTime[index].Hour && picTime.Minute == logTime[index].Minute && picTime.Second == logTime[index].Second)
                                    {
                                        Console.WriteLine("Processing..." + "       " + picTime + "      " + logTime[index]);

                                        float latitude = 0, longitude = 0, altitude = 0;
                                        if (!float.TryParse(log_data[index, 1], out latitude) || !float.TryParse(log_data[index, 2], out longitude) || !float.TryParse(log_data[index, 4], out altitude))
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

                                        var gpsLatitude    = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        gpsLatitude.Id     = 0x0002;
                                        gpsLatitude.Type   = 5;
                                        gpsLatitude.Len    = (int)ddmmss(latitude).LongLength;
                                        gpsLatitude.Value  = ddmmss(latitude);
                                        image.SetPropertyItem(gpsLatitude);

                                        var refLongitude = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        refLongitude.Id = 0x0003;
                                        refLongitude.Type = 2;
                                        refLongitude.Len = 2;
                                        refLongitude.Value = refLon(longitude);
                                        image.SetPropertyItem(refLongitude);

                                        var gpsLongitude = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        gpsLongitude.Id = 0x0004;
                                        gpsLongitude.Type = 5;
                                        gpsLongitude.Len = (int)ddmmss(longitude).LongLength;
                                        gpsLongitude.Value = ddmmss(longitude);
                                        image.SetPropertyItem(gpsLongitude);

                                        // Reference altitude
                                        var refaltitude   = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        refaltitude.Id    = 0x0005;
                                        refaltitude.Type  = 1;
                                        refaltitude.Len   = Alt_Buffer(Settings.GPSSettings.Default.Home_Altitude).Length;
                                        refaltitude.Value = Alt_Buffer(Settings.GPSSettings.Default.Home_Altitude);
                                        image.SetPropertyItem(refaltitude);

                                        // Actual altitude
                                        var gpsaltitude   = (PropertyItem)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                                        gpsaltitude.Id    = 0x0006;
                                        gpsaltitude.Type  = 10;
                                        gpsaltitude.Len   = Alt_GPS(altitude).Length;
                                        gpsaltitude.Value = Alt_GPS(altitude);
                                        image.SetPropertyItem(gpsaltitude);

                                        string geottaged_Dir      = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Geottaged";

                                        string geottaged_Dir_desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "Geottaged";

                                        string GeotaggedFile = geottaged_Dir+ Path.DirectorySeparatorChar + "Geo_" + index + ".JPG";
                                        string GeotaggedFile_Desk = geottaged_Dir_desk + Path.DirectorySeparatorChar + "Geo_" + index + ".JPG";

                                        if (!Directory.Exists(geottaged_Dir_desk))
                                        {
                                            Directory.CreateDirectory(geottaged_Dir_desk);
                                        }

                                        if(!Directory.Exists(GeotaggedFile))
                                        {
                                            Directory.CreateDirectory(geottaged_Dir);
                                        }
                                        image.Save(GeotaggedFile);
                                        image.Save(GeotaggedFile_Desk);

                                        Console.WriteLine("Successful Geottagging: " + GeotaggedFile);
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
        }

        
        private byte[] Alt_Buffer(float altitude){

            Int32 alt = Convert.ToInt32(altitude);

            Int32[] buffer_int = new Int32[] { alt };

            byte[] buffer = new byte[buffer_int.Length * sizeof(Int32)];

            Buffer.BlockCopy(buffer_int, 0, buffer, 0, buffer_int.Length * sizeof(Int32));

            return buffer;
        }


        private byte[] Alt_GPS(float altitude)
        {

            Int32 alt = Convert.ToInt32(altitude);

            Int32[] buffer_int = new Int32[] { alt , 1};

            byte[] buffer = new byte[buffer_int.Length * sizeof(Int32)];

            Buffer.BlockCopy(buffer_int, 0, buffer, 0, buffer_int.Length * sizeof(Int32));

            return buffer;
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

#if OLD
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


            int denoms = 1;
            int[] _ref_ui = new[] { degrees, denoms, minute, denoms, second, denoms };
            byte[] _ref = new byte[_ref_ui.Length * sizeof(UInt32)];
            Buffer.BlockCopy(_ref_ui, 0, _ref, 0, _ref_ui.Length * sizeof(UInt32));

            return _ref;
#else
            var d = (UInt32)Math.Abs(angle);
            var m = Math.Abs((angle % 1) * 60);
            var s = (m % 1) * 60;

            //Console.WriteLine(d +"      "+ m + "    "+s);

            var denoms       = (UInt32)1;
            UInt32[] _ref_ui =  new[] { d, denoms, (UInt32)m ,(UInt32)(denoms), (UInt32)(s*1e7), (UInt32)(denoms * 1e7)};
            byte[] _ref      = new byte[_ref_ui.Length * sizeof(UInt32)];
            Buffer.BlockCopy(_ref_ui, 0, _ref, 0, _ref_ui.Length * sizeof(UInt32));

            return _ref;
#endif
            //byte[] //BitConverter.GetBytes((UInt32)degrees);
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
         
                if (pane.YAxis.Scale.Max > 0.5)
                    pane.YAxis.Scale.Max -= 1;
                if (pane.YAxis.Scale.Min < -0.5)
                   pane.YAxis.Scale.Min += 1;
            }
            else
            {
                pane.YAxis.Scale.Max += 1;
                pane.YAxis.Scale.Min -= 1;
            }
        }


        public void GraphPane_Draw(float[] values)
        {
            double time  = DateTime.Now.ToOADate();

            if (time > pane.XAxis.Scale.Max)
            {
                pane.XAxis.Scale.Max = DateTime.Now.Add(new TimeSpan(0, 0, 0, 0, 30)).ToOADate();
                pane.XAxis.Scale.Min = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 0, 9970)).ToOADate();
            }

            for (int i = 0; i < 3; i++)
            {
                if (Line[i].Points.Count > 1500)
                    Line[i].RemovePoint(0);
            }


            switch (zedComboBox.Items[zedComboBox.SelectedIndex].ToString())
            {
                case "EulerRates(rad/sec)":
                    Line[0].AddPoint(time, values[0]);
                    Line[1].AddPoint(time, values[1]);
                    Line[2].AddPoint(time, values[2]);
                    break;

                case "Euler(rad)":
                    Line[0].AddPoint(time, values[3]);
                    Line[1].AddPoint(time, values[4]);
                    Line[2].AddPoint(time, values[5]);
                    break;

                case "RollRate(rad/sec)":
                    Line[0].AddPoint(time, values[0]);
                    break;

                case "PitchRate(rad/sec)":
                    Line[0].AddPoint(time, values[1]);
                    break;

                case "YawRate(rad/sec)":
                    Line[0].AddPoint(time, values[2]);
                    break;

                case "Roll(rad)":
                    Line[0].AddPoint(time, values[3]);
                    break;

                case "Pitch(rad)":
                    Line[0].AddPoint(time, values[4]);
                    break;

                case "Yaw(rad)":
                    Line[0].AddPoint(time, values[5]);
                    break;

                case "Accel(m/s/s)":
                    Line[0].AddPoint(time, values[11]);
                    Line[1].AddPoint(time, values[12]);
                    Line[2].AddPoint(time, values[13]);
                    break;

                case "Xacc(m/s/s)":
                    Line[0].AddPoint(time, values[11]);
                    break;

                case "Yacc(m/s/s)":
                    Line[0].AddPoint(time, values[12]);
                    break;

                case "Zacc(m/s/s)":
                    Line[0].AddPoint(time, values[13]);
                    break;

                case "Altitude(m)":
                    Line[0].AddPoint(time, values[9]);
                    break;

                case "Climbrate(cm/s)":
                    Line[0].AddPoint(time, values[10]);
                    break;

                case "Speed(m/s)":
                    Line[0].AddPoint(time, values[6]);
                    Line[1].AddPoint(time, values[7]);
                    break;

                case "Airspeed(m/s)":
                    Line[0].AddPoint(time, values[6]);
                    break;

                case "Groundspeed(m/s)":
                    Line[0].AddPoint(time, values[7]);
                    break;
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
