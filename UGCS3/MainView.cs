#define VS2015
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Speech.Synthesis;

//using System.Drawing.Drawing2D;
using System.IO.Ports;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET;

using UGCS3.MavlinkProtocol;
using UGCS3.UsableForms;
using UGCS3.ComPort;
using UGCS3.UsableControls;
using UGCS3.Map;

using UGCS3.HIL.Xplane10;
using UGCS3.Settings;
using UGCS3.Log;
using UGCS3.Common;

using System.Web;

using System.Net;

namespace UGCS3
{
    public partial class MainView:Form
    {
        // Custom mavlink Here...
        // GoogleMaps API
        // AIzaSyD4BkAWzWCMyt4L8tSWOP9G4q2QFAbWyxQ
        /*
         * Remove Global.cs Form if the form doesnt cause any problems after operating a couple of times.
         */
        // MainForm is used to handle waypoint data grid view as well as all other waypointing functions
        Form Main_Form;


        public MainView()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            Load        += MainView_Load;         
            Shown       += MainView_Shown;
            FormClosing += MainView_FormClosing;
          
            Main_Form = new Form();
            Main_Form.StartPosition = FormStartPosition.CenterScreen;
            Main_Form.Text   = "Welcome";
            Main_Form.FormClosing += Main_Form_FormClosing;
            Main_Form.Show();
            Main_Form.Hide();
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {           
            e.Cancel = true;
            waypoint_grid_button_Click(sender, e);
        }

        #region GLOBAL VARIABLES
        public FlickerFreePanel comportPanel;

        public Bitmap AI_background;
        public Bitmap AI_Wing;
        public Point AI_imageSize;
        public Label AirspeedLabel;
        public Label ThrottleLabel;
        public Label HeadingLabel;
        public Label BatteryLabel;
        public Label ModeLabel;
        public Label GPSLabel;
        public Label GroundSpeedLablel;
        public Label SATSLabel;
        public Label SignalLabel;
        public ComboBox MapBox;

       // private GMapControl gMapControl;
        private PictureBox Attitude_Indicator;
        private Timer DoUI_Timer;
        private Xplane xplane10;
        private SettingsControl SettingsCntrl;
        private CustomDataGrid CustomDataGridCntrl;
        private ParameterForm Parameter_Form;
        private BackgroundWorker main_backgroundWorker;
        private MavLinkSerialPacketClass Mavlink_Protocol;
        private Timer System_Timer;
        private SerialPort SerialPortObject;
        private GMapDirectionMarker _GmapDirectionalMarker;

        private GMapWayPointMarker GPRS_Marker;
        private GMapRoute GPRS_Route;

        private GmapTargetMarker target_marker;
        private GMapRoute _GmapRoute;
        private GMapWayPointMarker _HomeMarker;
        private GMapOverlay HeaderOverlay;
        private GMapOverlay TargetOverlay;
        private GMapOverlay WayPointOverlay;
        private GMapOverlay SurveyOverlay;
        private GMapOverlay PlaybackOverlay;

        private BackgroundWorker gprs_backgroundworker;
      
        /*
        private DateTime curTime;
        private DateTime prevTime;
        private int medium_5Hz_counter = 0;
        */ 

        private int slow_1Hz_counter = 0;
        private float fraction_GmapSize = 1;

        private Button waypoint_grid_button;
        private DataGridView WayPoint_DataGridView;
        private DataGridViewTextBoxColumn DGVTextBoxColumn;
        private DataGridViewButtonColumn   DGVButtonColumn;
        private DataGridViewComboBoxColumn DGVComboBoxColumn;

        private Rectangle MapControl_Rectangle;
        private Rectangle MapControl_Rectangle_Mini;
        private Point DirectionMarkerPoints;
        private Point AI_imagelocation;
        private Point AI_rotationPoint;
        private Point AI_WingPoint;
        private TrackBar htrackBar;
        private Button Update_Home_Button;
        private Button Clear_Map_Button;
        private int MapZoomLevel = 0;
        private int size_grip_button = 1;
        private int grid_button_width = 600;
        private System.Speech.Synthesis.SpeechSynthesizer Speech;
        private ToolStripDropDown _toolStripDropDown;

        BackgroundWorker _bwWayPoints; // consider making this a dynamic object instead of global.
        #endregion

        // Unsued Methods
        // gMapControl.Manager.PrimaryCache.DeleteOlderThan(DateTime.Now, GoogleHybridMapProvider.Instance.DbId);


        FlickerFreeGmapControl gMapControl;
        #region MAIN VIEW REGION
        /// <summary>
        ///  primary load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_Load(object sender, EventArgs e)
        {
            // Maximize Form
            WindowState = FormWindowState.Maximized;
            
            // Setup Stuff
            BackColor     = Color.FromArgb(38, 39, 41);
            MinimizeBox   = true;
            MapControl_Rectangle      = new Rectangle();
            MapControl_Rectangle_Mini = new Rectangle();
            DirectionMarkerPoints     = new Point();
            Text = "UGCS 3";

            // Initialise classes
            gMapControl      = new FlickerFreeGmapControl();
            SerialPortObject = new SerialPort();
            Mavlink_Protocol = new MavLinkSerialPacketClass(SerialPortObject);
            xplane10         = new Xplane();
            Speech           = new SpeechSynthesizer();
            DoUI_Timer      = new Timer();
            _bwWayPoints = new BackgroundWorker();

            // Place any objects properly          
            Setup_Controls();           
                        
            // add resize event
            SettingsButton.Click += SettingsButton_Click;
            DoUI_Timer.Tick += DoUI_Timer_Tick;
            _bwWayPoints.DoWork += _bwWayPoints_DoWork;
            _bwWayPoints.RunWorkerCompleted += _bwWayPoints_RunWorkerCompleted;
            RadiusNumericUpDown.ValueChanged += RadiusNumericUpDown_ValueChanged;
            ReadWayPointsButton.Click += ReadWayPointsButton_Click;
            WriteWayPointsButton.Click += WriteWayPointsButton_Click;
            Do_Action_Button.Click += Do_Action_Button_Click;
            Resize += MainView_Resize;
            Click += MainView_Click;

            // Setups            
            DoUI_Timer.Interval = 20;
            DoUI_Timer.Start();           
            _bwWayPoints.WorkerSupportsCancellation = true;
            _bwWayPoints.WorkerReportsProgress      = true;

            gprs_backgroundworker = new BackgroundWorker();
            gprs_backgroundworker.WorkerReportsProgress = true;
            gprs_backgroundworker.WorkerSupportsCancellation = true;
            gprs_backgroundworker.DoWork += Gprs_backgroundworker_DoWork;
            if (gprs_backgroundworker.IsBusy == false)
                gprs_backgroundworker.RunWorkerAsync();

            Console.WriteLine("Application Loaded: " + DateTime.Now);          
            //Console.WriteLine(MAVLink.MAVLINK_MESSAGE_CRCS[27]);
        }


        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        private void Gprs_backgroundworker_DoWork(object sender, DoWorkEventArgs e)
        {
          
            while(e.Cancel == false)
            {
                if(gprs_backgroundworker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // GET STUFF
                    var url = "http://herybotics.com/wp-uav.php/?u=G";

                    try
                    {
                        System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                        System.Net.WebResponse response = request.GetResponse();
                        System.IO.Stream datastream = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(datastream);
                        string myread = reader.ReadToEnd();
                        reader.Close();
                        response.Close();
                        Console.WriteLine(myread);

                        string[] mystring = myread.Split(';');

                        float lat =(float)(Convert.ToDouble(mystring[2]) * 1e-7);
                        float lon = (float)(Convert.ToDouble(mystring[3]) * 1e-7);

                        Variables.gprs_latitude  = lat;
                        Variables.gprs_longitude = lon;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    System.Threading.Thread.Sleep(2000);
                }
            }
        }


        /// <summary>
        ///  main view show event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_Shown(object sender, EventArgs e)
        {
            WPSeq_ComboBox.Items.Clear();
            
            WPSeq_ComboBox.Items.Add("Restart Mission");
        }


        /// <summary>
        ///  main view mouse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_Click(object sender, EventArgs e)
        {
            if (fraction_GmapSize != 1)
            {
                if (SettingsCntrl != null)
                {
                    Point clicked_pts = (e as MouseEventArgs).Location;

                    if (clicked_pts.X >= gMapControl.Size.Width && clicked_pts.X <= SettingsCntrl.Location.X)
                    {
                        // MessageBox.Show("WHat");
                    }
                }
            }
        }

        /// <summary>
        ///  Main View ResizeEvents, takes care of resizing other controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_Resize(object sender, EventArgs e)
        {          
             Resize_Controls();
        }

        /// <summary>
        ///  setting up MainView Controls
        /// </summary>
        private void Setup_Controls()
        {
            // Global Size of Buttons
            int size_of_buttons = 80;

            // HomeButton
            this.HomeButton.BackColor = Color.Transparent;
            this.HomeButton.Size = new Size(size_of_buttons, size_of_buttons);
            this.HomeButton.FlatStyle = FlatStyle.Popup;
            this.HomeButton.Image    = Properties.Resources.home_button_main_64x64__fw;
            this.HomeButton.Location = new Point(0, 0);

            // SettingsButton
            this.SettingsButton.BackColor = Color.Transparent;
            this.SettingsButton.Size = new Size(size_of_buttons, size_of_buttons);
            this.SettingsButton.FlatStyle = FlatStyle.Popup;
            this.SettingsButton.Image = Properties.Resources.configuration_and_settings_button;
            this.SettingsButton.Location = new Point(size_of_buttons + 2, 0);
            SettingsButton.MouseEnter += SettingsButton_MouseEnter;

            // UploadButton
            this.UploadButton.BackColor = Color.Transparent;
            this.UploadButton.Size = new Size(size_of_buttons, size_of_buttons);
            this.UploadButton.FlatStyle = FlatStyle.Popup;
            this.UploadButton.Image = Properties.Resources.connection_arrow_not_connected_64x64__fw1;
            this.UploadButton.Location = new Point(SettingsButton.Location.X + size_of_buttons + 2, 0);
            UploadButton.Click += UploadButton_Click;
            UploadButton.MouseEnter += UploadButton_MouseEnter;

            // FlightPlanButton
            this.Flight_Plan_Button.BackColor = Color.Transparent;
            this.Flight_Plan_Button.Size = new Size(size_of_buttons, size_of_buttons);
            this.Flight_Plan_Button.FlatStyle = FlatStyle.Popup;
            this.Flight_Plan_Button.Image = Properties.Resources.plan_mission_status_on_50x50_;
            this.Flight_Plan_Button.Location = new Point(UploadButton.Location.X + size_of_buttons + 2, 0);
            Flight_Plan_Button.Click += Flight_Plan_Button_Click;
            Flight_Plan_Button.MouseEnter += Flight_Plan_Button_MouseEnter;
            
            // SerialButton
            this.SerialButton.BackColor = Color.Transparent;
            this.SerialButton.Size = new Size(size_of_buttons, size_of_buttons);
            this.SerialButton.FlatStyle = FlatStyle.Popup;
            this.SerialButton.Image = Properties.Resources.connection_link_off_64x64_;
            this.SerialButton.Location = new Point(this.ClientSize.Width - 80, 0);
            this.SerialButton.Click += SerialButton_Click;

            // COMPORT PANEL
            comportPanel = new FlickerFreePanel();
            comportPanel.BackColor = Color.Transparent;
            comportPanel.Size = new Size(133,80);
            comportPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(comportPanel);
            string[] Ports = new string[] { "select comport" };
            string[] Baudrates = new string[] { "select baudrate", "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };     
            this.BaudRate_ComboBox.Items.AddRange(Baudrates);
            this.ComPort_ComboBox.Items.AddRange(Ports);
            this.BaudRate_ComboBox.SelectedIndex = 0;
            this.ComPort_ComboBox.SelectedIndex = 0;
            this.ComPort_ComboBox.Text = "select comport";
            comportPanel.Location = new Point(SerialButton.Location.X - comportPanel.Size.Width - 2, SerialButton.Location.Y);
            this.ComPort_ComboBox.MouseEnter += ComPort_ComboBox_MouseEnter;
            ComPort_ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            BaudRate_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comportPanel.Controls.AddRange(new Control[] { ComPort_ComboBox, StatusProgressBar, BaudRate_ComboBox });
            ComPort_ComboBox.Location  = new Point(2, 2);
            StatusProgressBar.Location = new Point(ComPort_ComboBox.Location.X, ComPort_ComboBox.Location.Y + ComPort_ComboBox.Size.Height + 3);
            BaudRate_ComboBox.Location = new Point(ComPort_ComboBox.Location.X, StatusProgressBar.Location.Y + StatusProgressBar.Size.Height + 3);

            this.StatusPanel.BackColor = Color.Transparent;
            StatusPanel.BorderStyle = BorderStyle.FixedSingle;
            this.StatusPanel.Location = new Point(comportPanel.Location.X - StatusPanel.Size.Width - 2, comportPanel.Location.Y);

            // GmapControl       
            gMapControl.Location     =  new Point(0, size_of_buttons + 2);
            gMapControl.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - (size_of_buttons + 2));
            gMapControl.Manager.Mode = AccessMode.ServerAndCache;
            gMapControl.Manager.UseMemoryCache = true;
            gMapControl.Manager.CacheOnIdleRead = true;
            gMapControl.Manager.BoostCacheEngine = true;
            gMapControl.MaxZoom = 20;
            gMapControl.MinZoom = 3;
            gMapControl.Zoom    = 5;
            gMapControl.BorderStyle = BorderStyle.FixedSingle;
            gMapControl.MapProvider = GoogleHybridMapProvider.Instance;
            gMapControl.Refresh();
            gMapControl.MouseClick    += gMapControl_MouseClick;
            gMapControl.MouseDown     += gMapControl_MouseDown;
            gMapControl.MouseMove     += gMapControl_MouseMove;
            gMapControl.MouseUp       += gMapControl_MouseUp;
            gMapControl.MouseHover    += gMapControl_MouseHover;
            gMapControl.OnMarkerClick += gMapControl_OnMarkerClick;
            this.Controls.Add(gMapControl);

            // Overlays and Markers
            HeaderOverlay          = new GMapOverlay("Main Overlay");
            TargetOverlay          = new GMapOverlay("Target Overlay");
            SurveyOverlay          = new GMapOverlay("Survey Overlay");
            PlaybackOverlay        = new GMapOverlay("Playback Overlay");
            _GmapDirectionalMarker = new GMapDirectionMarker(new GMap.NET.PointLatLng(Settings.GPSSettings.Default.Home_Latitude, Settings.GPSSettings.Default.Home_Longitude), 0, 0, Properties.Resources.locohale_map_asset_main_red_64x36__fw);

            GPRS_Marker = new GMapWayPointMarker(new GMap.NET.PointLatLng(Settings.GPSSettings.Default.Home_Latitude, Settings.GPSSettings.Default.Home_Longitude), Properties.Resources.generic_waypoint_50x58_, 20);
            GPRS_Route  = new GMapRoute("GPRS Route");
            GPRS_Route.Stroke = routePen;

            target_marker          = new GmapTargetMarker(new GMap.NET.PointLatLng(Settings.GPSSettings.Default.Home_Latitude, Settings.GPSSettings.Default.Home_Longitude), 20);
            _HomeMarker            = new GMapWayPointMarker(new GMap.NET.PointLatLng(Settings.GPSSettings.Default.Home_Latitude, Settings.GPSSettings.Default.Home_Longitude), Properties.Resources.homewaypoint_new_60x70_, 20);
            _GmapRoute             = new GMapRoute("Main Route");
            _GmapRoute.Stroke      = routePen;

            HeaderOverlay.Markers.Add(GPRS_Marker);
            HeaderOverlay.Routes.Add(GPRS_Route);

            HeaderOverlay.Markers.Add(_GmapDirectionalMarker);

            TargetOverlay.Markers.Add(target_marker);
            HeaderOverlay.Routes.Add(_GmapRoute);
            gMapControl.Overlays.Add(HeaderOverlay);
            gMapControl.Overlays.Add(TargetOverlay);
            gMapControl.Overlays.Add(SurveyOverlay);
            gMapControl.Overlays.Add(PlaybackOverlay);
            gMapControl.ZoomAndCenterMarkers("Main Overlay");
            gMapControl.Zoom = Settings.GPSSettings.Default.Map_Zoom;
            
            this.WPCoordinates.Add(_HomeMarker.Position); 
            this.WayPointOverlay = new GMapOverlay("WayPoint Overlay");
            this.WayPointOverlay.Markers.Add(_HomeMarker);
            gMapControl.Overlays.Add(WayPointOverlay);         
            
            // Trackbar
            htrackBar = new TrackBar();
            htrackBar.TickFrequency = 1;
            htrackBar.Minimum = gMapControl.MinZoom - 1;
            htrackBar.Maximum = gMapControl.MaxZoom;
            htrackBar.Value   = (int)gMapControl.Zoom;
            MapZoomLevel      = htrackBar.Value;
            htrackBar.Size    = new Size(200, 40);
            htrackBar.TickStyle = TickStyle.Both;
            htrackBar.Orientation = Orientation.Horizontal;
            htrackBar.Location = new Point(gMapControl.ClientSize.Width - htrackBar.ClientSize.Width, gMapControl.ClientSize.Height - htrackBar.ClientSize.Height);
            htrackBar.ValueChanged += htrackBar_ValueChanged;
            gMapControl.Controls.Add(htrackBar);           
               
            // UpdateHome        
            Update_Home_Button = new Button();
            Update_Home_Button.Size = new Size(60, 46);
            Update_Home_Button.Location = new Point(htrackBar.Location.X - Update_Home_Button.ClientSize.Width-5, gMapControl.ClientSize.Height - Update_Home_Button.Height);
            Update_Home_Button.Text = "Update Home";
            Update_Home_Button.ForeColor = Color.White;
            Update_Home_Button.BackColor = Color.FromArgb(38, 39, 41);
            Update_Home_Button.FlatStyle = FlatStyle.Popup;
            Update_Home_Button.Click += Update_Home_Button_Click;
            gMapControl.Controls.Add(Update_Home_Button);
            
            // ClearMap
            Clear_Map_Button = new Button();
            Clear_Map_Button.Size = new Size(60, 46);
            Clear_Map_Button.Location = new Point(Update_Home_Button.Location.X - Clear_Map_Button.ClientSize.Width - 4, gMapControl.ClientSize.Height - Update_Home_Button.Height);
            Clear_Map_Button.Text = "Clear Map";
            Clear_Map_Button.ForeColor = Color.White;
            Clear_Map_Button.BackColor = Color.FromArgb(38, 39, 41);
            Clear_Map_Button.FlatStyle = FlatStyle.Popup;
            Clear_Map_Button.Click += Clear_Map_Button_Click;
            gMapControl.Controls.Add(Clear_Map_Button);

            // Attitude Indicator Setup
            Attitude_Indicator = new PictureBox();
            Attitude_Indicator.Location = new Point(0, 0);
            Attitude_Indicator.Size = new Size((int)(gMapControl.Size.Width * 0.25), (int)(gMapControl.Size.Height * 0.50));
            Attitude_Indicator.BackColor = Color.Green;
            Attitude_Indicator.BorderStyle = BorderStyle.FixedSingle;
            Attitude_Indicator.Paint += Attitude_Indicator_Paint;
            gMapControl.Controls.Add(Attitude_Indicator);

            // Bitmaps
            AI_background   = Properties.Resources.attitude_background_930x1593;
            AI_Wing         = Properties.Resources.attitude_indicator_wing;
            AI_imageSize    = new Point(AI_background.Size.Width, AI_background.Size.Height);
            AI_imagelocation  = new Point((Attitude_Indicator.Size.Width / 2) - (AI_imageSize.X / 2), (Attitude_Indicator.Size.Height / 2) - (AI_imageSize.Y / 2));
            AI_rotationPoint  = new Point((Attitude_Indicator.Size.Width / 2), (Attitude_Indicator.Size.Height / 2));
            AI_WingPoint      = new Point((Attitude_Indicator.Size.Width / 2) - (AI_Wing.Size.Width / 2), (Attitude_Indicator.Size.Height / 2) - (AI_Wing.Size.Height / 2));

            // Airspeed label
            AirspeedLabel = new Label();
            AirspeedLabel.Size = new System.Drawing.Size(54, 25);
            AirspeedLabel.BackColor = Color.Transparent;
            AirspeedLabel.TextAlign = ContentAlignment.MiddleCenter;
            AirspeedLabel.ForeColor = Color.Green;
            AirspeedLabel.BorderStyle = BorderStyle.FixedSingle;
            AirspeedLabel.Text = "AS (m/s)";
            AirspeedLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - AirspeedLabel.Size.Width-2, Attitude_Indicator.ClientSize.Height - AirspeedLabel.Size.Height - 2);
            this.Attitude_Indicator.Controls.Add(AirspeedLabel);
            
            // GPS Label      
            GPSLabel = new Label();
            GPSLabel.Size = new System.Drawing.Size(54, 25);
            GPSLabel.BackColor = Color.Transparent;
            GPSLabel.TextAlign = ContentAlignment.MiddleCenter;
            GPSLabel.ForeColor = Color.Green;
            GPSLabel.BorderStyle = BorderStyle.FixedSingle;
            GPSLabel.Text = "GPS Fix";
            GPSLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - GPSLabel.Size.Width - 2, Attitude_Indicator.ClientSize.Height - 3*GPSLabel.Size.Height - 2);
            this.Attitude_Indicator.Controls.Add(GPSLabel);          
            
            // Satellites Label
            SATSLabel = new Label();
            SATSLabel.Size = new System.Drawing.Size(54, 25);
            SATSLabel.BackColor = Color.Transparent;
            SATSLabel.TextAlign = ContentAlignment.MiddleCenter;
            SATSLabel.ForeColor = Color.Green;
            SATSLabel.BorderStyle = BorderStyle.FixedSingle;
            SATSLabel.Text = "## sats";
            SATSLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - SATSLabel.Size.Width - 2, Attitude_Indicator.ClientSize.Height - 2 * SATSLabel.Size.Height - 2);
            this.Attitude_Indicator.Controls.Add(SATSLabel);


            // Throttle label
            ThrottleLabel = new Label();
            ThrottleLabel.Size = new System.Drawing.Size(54, 25);
            ThrottleLabel.BackColor = Color.Transparent;
            ThrottleLabel.TextAlign = ContentAlignment.MiddleCenter;
            ThrottleLabel.ForeColor = Color.DarkBlue;
            ThrottleLabel.BorderStyle = BorderStyle.FixedSingle;
            ThrottleLabel.Text = "T: 100%";
            ThrottleLabel.Location = new Point(2, Attitude_Indicator.ClientSize.Height - ThrottleLabel.Size.Height - 2);
            this.Attitude_Indicator.Controls.Add(ThrottleLabel);

            // Mode Label
            ModeLabel = new Label();
            ModeLabel.Size = new System.Drawing.Size(54, 25);
            ModeLabel.BackColor = Color.Transparent;
            ModeLabel.TextAlign = ContentAlignment.MiddleCenter;
            ModeLabel.ForeColor = Color.Black;
            ModeLabel.BorderStyle = BorderStyle.FixedSingle;
            ModeLabel.Text = "Mode";
            ModeLabel.Location = new Point(2, Attitude_Indicator.ClientSize.Height - (3 * ModeLabel.Size.Height) - 2);
            this.Attitude_Indicator.Controls.Add(ModeLabel);

            // GroundSpeed      
            GroundSpeedLablel = new Label();
            GroundSpeedLablel.Size = new System.Drawing.Size(54, 25);
            GroundSpeedLablel.BackColor = Color.Transparent;
            GroundSpeedLablel.TextAlign = ContentAlignment.MiddleCenter;
            GroundSpeedLablel.ForeColor = Color.Black;
            GroundSpeedLablel.BorderStyle = BorderStyle.FixedSingle;
            GroundSpeedLablel.Text = "GS (m/s)";
            GroundSpeedLablel.Location = new Point(2, Attitude_Indicator.ClientSize.Height - (2 * GroundSpeedLablel.Size.Height) - 2);
            this.Attitude_Indicator.Controls.Add(GroundSpeedLablel);

            // HeadingLabel
            HeadingLabel = new Label();
            HeadingLabel.Size = new System.Drawing.Size(50, 25);
            HeadingLabel.BackColor = Color.Transparent;
            HeadingLabel.TextAlign = ContentAlignment.MiddleCenter;
            HeadingLabel.ForeColor = Color.Black;
            HeadingLabel.BorderStyle = BorderStyle.FixedSingle;
            HeadingLabel.Text = "Hdn";
            HeadingLabel.Location = new Point((Attitude_Indicator.ClientSize.Width / 2) - (HeadingLabel.Width / 2), 0);
            this.Attitude_Indicator.Controls.Add(HeadingLabel);

            // SignalLabel           
            SignalLabel = new Label();
            SignalLabel.Size = new System.Drawing.Size(46, 25);
            SignalLabel.BackColor = Color.Transparent;
            SignalLabel.TextAlign = ContentAlignment.MiddleCenter;
            SignalLabel.ForeColor = Color.Black;
            SignalLabel.BorderStyle = BorderStyle.FixedSingle;
            SignalLabel.Text = "S:100%";
            SignalLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - SignalLabel.Size.Width - 2, 0);
            this.Attitude_Indicator.Controls.Add(SignalLabel);

            // BatteryLabel
            BatteryLabel = new Label();
            BatteryLabel.Size = new System.Drawing.Size(60, 25);
            BatteryLabel.BackColor = Color.Transparent;
            BatteryLabel.TextAlign = ContentAlignment.MiddleCenter;
            BatteryLabel.ForeColor = Color.Black;
            BatteryLabel.BorderStyle = BorderStyle.FixedSingle;
            BatteryLabel.Text = "BV:12.5 V";
            BatteryLabel.Location = new Point(0, 0);
            this.Attitude_Indicator.Controls.Add(BatteryLabel);

            // Maps ComboBox
            this.MapBox = new ComboBox();
            this.MapBox.Size = new System.Drawing.Size(100, 25);
            this.MapBox.MaximumSize = new System.Drawing.Size(100, 30);   
                    
            /*
            Console.WriteLine(GoogleHybridMapProvider.Instance.RefererUrl);
            Console.WriteLine(GoogleHybridMapProvider.Instance.SecureWord);
            Console.WriteLine(GoogleHybridMapProvider.Instance.Server);
            Console.WriteLine(GoogleHybridMapProvider.Instance.ServerAPIs);
            Console.WriteLine(GoogleHybridMapProvider.IsSocksProxy);
            Console.WriteLine(GoogleHybridMapProvider.TimeoutMs);
            Console.WriteLine(GoogleHybridMapProvider.UserAgent);                  
            GoogleHybridMapProvider.Instance.Version = "h@333000000";
            Console.WriteLine(GoogleHybridMapProvider.Instance.Version);
            Console.WriteLine(GoogleHybridMapProvider.Instance.TryCorrectVersion);
            Console.WriteLine(GoogleHybridMapProvider.WebProxy);
            */   
                     
            string[] maps = new string[] { 
                GoogleHybridMapProvider.Instance.Name, 
                GoogleSatelliteMapProvider.Instance.Name, 
                BingHybridMapProvider.Instance.Name, 
                BingSatelliteMapProvider.Instance.Name,
                OpenStreetMapProvider.Instance.Name
            };
            this.MapBox.Items.AddRange(maps);
            this.MapBox.Location = new Point(Attitude_Indicator.Location.X + Attitude_Indicator.ClientSize.Width + 2, 0);
            this.MapBox.SelectedIndexChanged += MapBox_SelectedIndexChanged;
            this.MapBox.SelectedIndex = Settings.GPSSettings.Default.Map_Type;
            this.gMapControl.Controls.Add(MapBox);
            
            // waypoint grid button
            this.waypoint_grid_button = new Button();
            this.waypoint_grid_button.Size = new System.Drawing.Size(grid_button_width, 25);
            this.waypoint_grid_button.Location = new Point(gMapControl.ClientSize.Width - waypoint_grid_button.ClientSize.Width, 0);
            this.waypoint_grid_button.BackColor = Color.FromArgb(38, 39, 41);
            this.waypoint_grid_button.FlatStyle = FlatStyle.Popup;
            this.waypoint_grid_button.BackgroundImage = Properties.Resources.button_down_40x23_;
            this.waypoint_grid_button.BackgroundImageLayout = ImageLayout.Tile;
            this.waypoint_grid_button.Click += waypoint_grid_button_Click;
            this.gMapControl.Controls.Add(waypoint_grid_button);
            
            // Consider making this object only available when grid button has been clicked
            // Future Updates
            // WayPoint DataGridView           
            this.WayPoint_DataGridView = new DataGridView();          
            this.WayPoint_DataGridView.Location = this.waypoint_grid_button.Location;
            this.WayPoint_DataGridView.Size = this.waypoint_grid_button.Size;
            Main_Form.Controls.Add(WayPoint_DataGridView);
            
            DGVTextBoxColumn = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.HeaderText = "Id";
            DGVTextBoxColumn.Name = "Id_Column";
            DGVTextBoxColumn.Width = 30;
            DGVTextBoxColumn.ReadOnly = true;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVTextBoxColumn = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.HeaderText = "Latitude(deg)";
            DGVTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.Name = "Latitude_Column";
            DGVTextBoxColumn.Width = 85;
            DGVTextBoxColumn.ReadOnly = true;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVTextBoxColumn = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.HeaderText = "Longitude(deg)";
            DGVTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.Name = "Longitude_Column";
            DGVTextBoxColumn.Width = 85;
            DGVTextBoxColumn.ReadOnly = true;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVTextBoxColumn = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.HeaderText = "Alt(m)";
            DGVTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.Name = "Altitude_Column";
            DGVTextBoxColumn.Width = 45;
            DGVTextBoxColumn.ReadOnly = false;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVTextBoxColumn = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.HeaderText = "Rad(m)";
            DGVTextBoxColumn.Name = "Radius_Column";
            DGVTextBoxColumn.Width = 45;
            DGVTextBoxColumn.ReadOnly = false;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVComboBoxColumn = new DataGridViewComboBoxColumn();
            DGVComboBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVComboBoxColumn.HeaderText = "Command";
            DGVComboBoxColumn.Name = "Command_Column";
            DGVComboBoxColumn.Items.AddRange(new string[] { "WAYPOINT", "TAKEOFF", "LAND", "RTL" });
            DGVComboBoxColumn.Width = 100;
            DGVComboBoxColumn.FlatStyle = FlatStyle.Popup;
            this.WayPoint_DataGridView.Columns.Add(DGVComboBoxColumn);

            DGVButtonColumn             = new DataGridViewButtonColumn();
            DGVButtonColumn.HeaderText  = "Delete";
            DGVButtonColumn.Name        = "Delete_Column";
            DGVButtonColumn.Width       = 47;
            DGVButtonColumn.Text        = "X";
            this.WayPoint_DataGridView.Columns.Add(DGVButtonColumn);

            DGVTextBoxColumn            = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.SortMode   = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.HeaderText = "Distance(m)";
            DGVTextBoxColumn.Name       = "Distance_Column";
            DGVTextBoxColumn.Width      = 60;
            DGVTextBoxColumn.ReadOnly   = true;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            DGVTextBoxColumn            = new DataGridViewTextBoxColumn();
            DGVTextBoxColumn.SortMode   = DataGridViewColumnSortMode.NotSortable;
            DGVTextBoxColumn.HeaderText = "Gradient(%)";
            DGVTextBoxColumn.Name       = "Gradient_Column";
            DGVTextBoxColumn.Width      = 60;
            DGVTextBoxColumn.ReadOnly   = true;
            this.WayPoint_DataGridView.Columns.Add(DGVTextBoxColumn);

            (WayPoint_DataGridView.Rows[0].Cells["Id_Column"] as DataGridViewTextBoxCell).Value = 1;
            (WayPoint_DataGridView.Rows[0].Cells["Command_Column"] as DataGridViewComboBoxCell).Value = (WayPoint_DataGridView.Rows[0].Cells["Command_Column"] as DataGridViewComboBoxCell).Items[0];
            (WayPoint_DataGridView.Rows[0].Cells["Delete_Column"] as DataGridViewButtonCell).Value = "X";           
            WayPoint_DataGridView.AllowDrop      = false;          
            WayPoint_DataGridView.RowsAdded     += WayPoint_DataGridView_RowsAdded;
            WayPoint_DataGridView.CellClick     += WayPoint_DataGridView_CellClick;
            WayPoint_DataGridView.RowsRemoved   += WayPoint_DataGridView_RowsRemoved;
            WayPoint_DataGridView.CellEndEdit   += WayPoint_DataGridView_CellEndEdit;
            
            
            SettingsCntrl = new SettingsControl();
            SettingsCntrl.Location = new Point(this.ClientSize.Width / 2, gMapControl.Location.Y - 3);
            SettingsCntrl.startXplaneButton.Click += startXplaneButton_Click;
            SettingsCntrl.udpconnectButton.Click += udpconnectButton_Click;
            SettingsCntrl.SimIpTextBox.Text = XplaneHil.Default.simIP;
            SettingsCntrl.sndPortTextBox.Text = XplaneHil.Default.sndPort;
            SettingsCntrl.recPortTextBox.Text = XplaneHil.Default.recPort;
            SettingsCntrl.Hide();
            this.Controls.Add(SettingsCntrl);   
            SettingsCntrl.Parameter_button.Click += Parameter_button_Click;
            SettingsCntrl.plotLatLng_button.Click += plotLatLng_button_Click;

           
        }


        /// <summary>
        ///  resize MainView controls
        /// </summary>
        private void Resize_Controls()
        {
            //Console.WriteLine("Humble");
            //MessageBox.Show("Humble");

            if (fraction_GmapSize != 1)
            {
                float width = this.ClientSize.Width;
                float cntrl_width = this.SettingsCntrl.ClientSize.Width - 5;
                fraction_GmapSize = 1 - (cntrl_width / width);
            }

            int size_of_buttons = 80;
            this.HomeButton.Location = new Point(0, 0);
            this.SettingsButton.Location = new Point(size_of_buttons + 2, 0);
            this.UploadButton.Location = new Point(SettingsButton.Location.X + size_of_buttons + 2, 0);
            this.Flight_Plan_Button.Location = new Point(UploadButton.Location.X + size_of_buttons + 2, 0);         
            this.SerialButton.Location = new Point(ClientSize.Width - 80, 0);
            this.comportPanel.Location = new System.Drawing.Point(SerialButton.Location.X - comportPanel.Size.Width - 2, SerialButton.Location.Y);
            this.StatusPanel.Location = new Point(comportPanel.Location.X - StatusPanel.Size.Width - 2, comportPanel.Location.Y);
            this.gMapControl.Location = new Point(0, size_of_buttons + 2);
            this.gMapControl.Size = new Size((int)(this.ClientSize.Width * fraction_GmapSize), this.ClientSize.Height - (size_of_buttons + 2));

            // Attitude indicator
            this.Attitude_Indicator.Location = new Point(0, 0);
            this.Attitude_Indicator.Size     = new Size((int)(gMapControl.Size.Width * (0.25 + (1 - fraction_GmapSize) / 3)), (int)(gMapControl.Size.Height * 0.50));
            this.MapBox.Location             = new Point(Attitude_Indicator.Location.X + Attitude_Indicator.ClientSize.Width + 2, 0);

            // waypoint grid button
            this.waypoint_grid_button.Size = new System.Drawing.Size(grid_button_width, 25);
            this.waypoint_grid_button.Location = new Point(gMapControl.ClientSize.Width - waypoint_grid_button.ClientSize.Width, waypoint_grid_button.Location.Y);

            this.WayPoint_DataGridView.Location = new Point(this.waypoint_grid_button.Location.X, 0);
            this.WayPoint_DataGridView.Size = new Size(this.waypoint_grid_button.Size.Width, WayPoint_DataGridView.Size.Height);


            // AI labels for vfr display
            AirspeedLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - AirspeedLabel.Size.Width - 2, Attitude_Indicator.ClientSize.Height - AirspeedLabel.Size.Height - 2);           
            GPSLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - GPSLabel.Size.Width - 2, Attitude_Indicator.ClientSize.Height - 3*GPSLabel.Size.Height - 2);
            SATSLabel.Location = new Point(Attitude_Indicator.ClientSize.Width - SATSLabel.Size.Width - 2, Attitude_Indicator.ClientSize.Height - 2 * SATSLabel.Size.Height - 2);
   
            ThrottleLabel.Location = new Point(2, Attitude_Indicator.ClientSize.Height - ThrottleLabel.Size.Height - 2);
            ModeLabel.Location     = new Point(2, Attitude_Indicator.ClientSize.Height - (3 * ModeLabel.Size.Height) - 2);
            GroundSpeedLablel.Location = new Point(2, Attitude_Indicator.ClientSize.Height - (2 * GroundSpeedLablel.Size.Height) - 2);

            HeadingLabel.Location  = new Point((Attitude_Indicator.ClientSize.Width / 2) - (HeadingLabel.Width / 2), 0);
            SignalLabel.Location   = new Point(Attitude_Indicator.ClientSize.Width - SignalLabel.Size.Width - 2, 0);  
            BatteryLabel.Location  = new Point(0, 0);

            htrackBar.Location = new Point(gMapControl.ClientSize.Width - htrackBar.ClientSize.Width, gMapControl.ClientSize.Height - htrackBar.ClientSize.Height);
            Update_Home_Button.Location = new Point(htrackBar.Location.X - Update_Home_Button.ClientSize.Width - 5, gMapControl.ClientSize.Height - Update_Home_Button.Height);
            Clear_Map_Button.Location = new Point(Update_Home_Button.Location.X - Clear_Map_Button.ClientSize.Width - 4, gMapControl.ClientSize.Height - Clear_Map_Button.Height);

            Graphics gfx = Attitude_Indicator.CreateGraphics();
            gfx.Clear(Color.Black);
            AI_imagelocation = new Point((Attitude_Indicator.Size.Width / 2) - (AI_imageSize.X / 2), (Attitude_Indicator.Size.Height / 2) - (AI_imageSize.Y / 2));
            AI_rotationPoint = new Point((Attitude_Indicator.Size.Width / 2), (Attitude_Indicator.Size.Height / 2));
            AI_WingPoint     = new Point((Attitude_Indicator.Size.Width / 2) - (AI_Wing.Size.Width / 2), (Attitude_Indicator.Size.Height / 2) - (AI_Wing.Size.Height / 2)); ;
            Attitude_Indicator.Invalidate();

            if (CustomDataGridCntrl != null)
            {
                CustomDataGridCntrl.Location = new Point(0, Attitude_Indicator.Location.Y + Attitude_Indicator.Size.Height + 2); // nB: if i change Y into automatic size then need to include this control in resize method
                CustomDataGridCntrl.Size = new Size(235, (int)(gMapControl.Size.Height * 0.50));
                CustomDataGridCntrl.BorderPanel.Size = new Size(231, (int)(gMapControl.Size.Height * 0.50) - 6);
            }

            if (SettingsCntrl != null)
            {
                SettingsCntrl.Location = new Point((int)(this.ClientSize.Width * fraction_GmapSize), gMapControl.Location.Y - 3);
                SettingsCntrl.Size     = new Size(SettingsCntrl.ClientSize.Width, this.ClientSize.Height - (size_of_buttons - 1));
                SettingsCntrl.BatteryVoltageTextBox.TextChanged += BatteryVoltageTextBox_TextChanged;
            }
        }
        #endregion


        #region MAINVIEW EVENTS
        /// <summary>
        /// Variables
        /// </summary>
        bool mission_planning_mode = true;
        ToolTip tip;


        /// <summary>
        ///  settings button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (fraction_GmapSize == 1)
            {
                float width = this.ClientSize.Width;
                float cntrl_width = this.SettingsCntrl.ClientSize.Width - 5;

                fraction_GmapSize = 1 - (cntrl_width / width);
                SettingsCntrl.Show();
            }
            else
            {
                fraction_GmapSize = 1;
                SettingsCntrl.Hide();
            }

            Resize_Controls();
        }


        /// <summary>
        /// Uploading parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, EventArgs e)
        {
            Upload_Parameter();
        }


        /// <summary>
        /// DoAction Certain Commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Do_Action_Button_Click(object sender, EventArgs e)
        {
            int seq = WPSeq_ComboBox.SelectedIndex;

            if (WPSeq_ComboBox.SelectedText == "Restart Mission")
            {
                Mission_Start_Command();
            }
        }


        /// <summary>
        /// Mission planning Mode Activate and Deactivate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flight_Plan_Button_Click(object sender, EventArgs e)
        {
            mission_planning_mode = !mission_planning_mode;
           
            WPSeq_ComboBox.Items.Clear();
            WPSeq_ComboBox.Items.Add("Restart Mission");

            if (mission_planning_mode)
            {
                this.Flight_Plan_Button.Image = Properties.Resources.plan_mission_status_on_50x50_;

                WayPointOverlay.Clear();
                WayPointOverlay.Polygons.Clear();

                WayPointOverlay.Markers.Add(_HomeMarker);
                for (int i = 1; i <WPCoordinates.Count; i++)
                {
                    WayPointOverlay.Markers.Add(new GMapWayPointMarker(new PointLatLng(WPCoordinates[i].Lat, WPCoordinates[i].Lng), Properties.Resources.generic_waypoint_50x58_, int.Parse((WayPoint_DataGridView.Rows[i - 1].Cells["Radius_Column"] as DataGridViewTextBoxCell).Value.ToString())) { ToolTipMode = MarkerTooltipMode.Always, ToolTipText = i.ToString()});
                }
                WayPointOverlay.Polygons.Add(new GMapPolygon(WPCoordinates, "Polygon") { Fill = Brushes.Transparent, Stroke = polyPen});
                gMapControl.Invalidate();

                gMapControl.MouseClick += gMapControl_MouseClick;
                //gMapControl.MouseDown  += gMapControl_MouseDown;
                gMapControl.MouseMove  += gMapControl_MouseMove;
                gMapControl.MouseUp    += gMapControl_MouseUp;
                Clear_Map_Button.Click += Clear_Map_Button_Click;
                WayPoint_DataGridView.CellClick += WayPoint_DataGridView_CellClick;
                WayPoint_DataGridView.CellEndEdit += WayPoint_DataGridView_CellEndEdit;
                WayPoint_DataGridView.Columns["Altitude_Column"].ReadOnly = false;
                WayPoint_DataGridView.Columns["Radius_Column"].ReadOnly = false;
                ReadWayPointsButton.Enabled = true;
                WriteWayPointsButton.Enabled = true;
                Update_Home_Button.Enabled = true;
                Speech.SpeakAsync("Mission Planning Active");
            }
            else
            {
                this.Flight_Plan_Button.Image = Properties.Resources.plan_mission_status_off_50x50_;

                WayPointOverlay.Clear();
                WayPointOverlay.Polygons.Clear();

                if (Variables.WP[0].count > 0)
                {
                    WayPointOverlay.Markers.Add(new GMapWayPointMarker(new PointLatLng(Variables.WP[0].lat, Variables.WP[0].lng), Properties.Resources.homewaypoint_new_60x70_, (int)Variables.WP[0].radius)
                    {
                        ToolTipMode = MarkerTooltipMode.Always,
                        ToolTipText = "Home",
                    });

                    WPSeq_ComboBox.Items.Add(0);

                    for (int i = 1; i < Variables.WP[0].count; i++)
                    {
                        WayPointOverlay.Markers.Add(new GMapWayPointMarker(new PointLatLng(Variables.WP[i].lat, Variables.WP[i].lng), Properties.Resources.generic_waypoint_50x58_, (int)Variables.WP[i].radius)
                        {
                            ToolTipMode = MarkerTooltipMode.Always,
                            ToolTipText = i.ToString(),
                        });
                        WPSeq_ComboBox.Items.Add(i);
                    }

                    List<PointLatLng> _coordinates = new List<PointLatLng>();
                    for (int i = 0; i < Variables.WP[0].count; i++)
                    {
                        _coordinates.Add(new PointLatLng(Variables.WP[i].lat, Variables.WP[i].lng));
                    }
                        WayPointOverlay.Polygons.Add(new GMapPolygon(_coordinates, "Polygon") { Fill = Brushes.Transparent, Stroke = polyPen });
                }


                gMapControl.Invalidate();

                WayPoint_DataGridView.CellClick -= WayPoint_DataGridView_CellClick;
                WayPoint_DataGridView.CellEndEdit -= WayPoint_DataGridView_CellEndEdit;              
                gMapControl.MouseClick -= gMapControl_MouseClick;
                //gMapControl.MouseDown  -= gMapControl_MouseDown;
                gMapControl.MouseMove  -= gMapControl_MouseMove;
                gMapControl.MouseUp    -= gMapControl_MouseUp;
                Clear_Map_Button.Click -= Clear_Map_Button_Click;
                WayPoint_DataGridView.Columns["Altitude_Column"].ReadOnly = true;
                WayPoint_DataGridView.Columns["Radius_Column"].ReadOnly = true;
                ReadWayPointsButton.Enabled = false;
                WriteWayPointsButton.Enabled = false;
                Update_Home_Button.Enabled = false;
                Speech.SpeakAsync("Mission Planning Deactivated");
            }
        }
        
        /// <summary>
        /// Mouse Entering Flight Plan Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flight_Plan_Button_MouseEnter(object sender, EventArgs e)
        {            
            if (tip != null)
            {
                tip.Dispose();
                tip = null;
            }   
            tip = new ToolTip();    
            tip.Show("Press the button to change the mission planning mode" + "\n" + "Mission planning set to: " + mission_planning_mode.ToString(), Flight_Plan_Button);         
        }

        /// <summary>
        /// upload paramters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_MouseEnter(object sender, EventArgs e)
        {
            if (tip != null)
            {
                tip.Dispose();
                tip = null;
            }
            tip = new ToolTip();
            tip.Show("Upload Changed Parameters", UploadButton); 
        }

        /// <summary>
        /// settings button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_MouseEnter(object sender, EventArgs e)
        {
            if (tip != null)
            {
                tip.Dispose();
                tip = null;
            }
            tip = new ToolTip();
            tip.Show("Settings Bar", SettingsButton); 
        }
        #endregion


        #region MAIN BACKGROUND WORKER
        /// <summary>
        ///  initialise the background worker -> called only in serialPort
        /// </summary>
        private void initialise_mainBackgroundWorker()
        {
            if (main_backgroundWorker == null)
            {
                main_backgroundWorker = new BackgroundWorker();
                main_backgroundWorker.WorkerReportsProgress = true;
                main_backgroundWorker.WorkerSupportsCancellation = true;
            }
        }

        /// <summary>
        /// start main background worker
        /// </summary>
        //TextBox ParameterNames_TextBox;
        private void start_mainBackgroundWorker()
        {
            main_backgroundWorker.DoWork                += new DoWorkEventHandler(main_backgroundWorker_DoWork);
            main_backgroundWorker.RunWorkerCompleted    += main_backgroundWorker_RunWorkerCompleted;
            if (main_backgroundWorker.IsBusy != true)
            {
                main_backgroundWorker.RunWorkerAsync();
            }
            //ParameterNames_TextBox = new TextBox();
            //ParameterNames_TextBox.Location = new Point(500, 200);
            //gMapControl.Controls.Add(ParameterNames_TextBox);
        }

        /// <summary>
        ///  dispose main background woorker.
        /// </summary>
        private void dispose_mainBackgroundWorker()
        {
            if (main_backgroundWorker != null) // cleaning / stopping this will automatically close parameter form in it's worker completed event so no need to clean up the form here
            {
                main_backgroundWorker.DoWork -= main_backgroundWorker_DoWork;
                main_backgroundWorker.CancelAsync();
                main_backgroundWorker.Dispose();
                main_backgroundWorker = null;
                
                //ParameterNames_TextBox.Dispose();
            }
        }


        /// <summary>
        ///  runworker completed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // We are sure that the background worker has finished operating and therefore we can dispose this object.
                if (Parameter_Form != null)
                {
                    Parameter_Form.Close();
                    Parameter_Form.Dispose();
                    Parameter_Form = null;
                }

                if (CustomDataGridCntrl != null)
                {
                    CustomDataGridCntrl.Dispose();
                    CustomDataGridCntrl = null;
                }
            }
        }

       

        /// <summary>
        /// Main view background worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!e.Cancel)
            {
                if (main_backgroundWorker == null)
                {
                    e.Cancel = true;
                    break;
                }

                if (main_backgroundWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    byte[] buffer = Mavlink_Protocol.readMavPackets();

                    if (buffer != null)
                    {
                        // receiving parameter list
                        switch (buffer[5])
                        {
                            case (byte)MAVLink.MAVLINK_MSG_ID.PARAM_VALUE:
                                MAVLink.mavlink_param_value_t received_paramter_t = buffer.ByteArrayToStructure<MAVLink.mavlink_param_value_t>(6);
                                string param_name   = ASCIIEncoding.ASCII.GetString(received_paramter_t.param_id, 0, 15);
                                ushort param_index  = received_paramter_t.param_index;
                                ushort param_count  = received_paramter_t.param_count;
                                float param_value   = received_paramter_t.param_value;

                                if (Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    // Set the UAV ID
                                    Variables.UAVID = buffer[3];
                                    // write all parameters to the customgrid
                                    try
                                    {
                                        string _updated_param_name = "";
                                        if (this.InvokeRequired)
                                        {    
                                            this.Invoke(new Action(() => Parameter_Form.Display_Parameters(param_index, param_count, param_name, this.Location, this.Size)));
                                            _updated_param_name       = Parameter_Form.ParameterIdLabel.Text;
                                            //string index = Parameter_Form.ParameterNumberLabel.Text;
                                        }
                                        else
                                        {
                                            _updated_param_name     = Parameter_Form.ParameterIdLabel.Text;
                                        }

                                        Log.Log.logparameter(_updated_param_name, param_index, param_value, Variables.UAVID);

                                        this.Invoke(new Action(() => CustomDataGridCntrl.Add_Rows(received_paramter_t.param_index, _updated_param_name, param_value, 3, 0.05F, Int16.MinValue, Int16.MaxValue, received_paramter_t.param_type)));

                                        Console.WriteLine("UAV" + '\t' + Variables.UAVID + " " + '\t' + _updated_param_name + " " + '\t' + received_paramter_t.param_value + '\t' + received_paramter_t.param_index + '\t' + received_paramter_t.param_count);

                                        if (param_index + 1 == param_count)
                                        {
                                            this.Invoke(new Action(() => this.SerialButton.Enabled = true));

                                            this.Invoke(new Action(()=> Parameter_Form.Hide()));
                                            this.Invoke(new Action(() => Parameter_Form.Close()));
                                            this.Invoke(new Action(() => Parameter_Form.Dispose()));     
                                        }

                                        if (param_index + 1 == param_count)
                                        {
                                            Variables.WAITING_FOR_PARAM_LIST = false;
                                            Stop_TimerEvent(LIST_TIMER_EVENTS.PARAMETER_LIST, 10);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Receiving Parameter Error: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    if (Variables.UAVID == buffer[3])
                                    {
                                        // messages sent from MAV to confirm receiving parameters sent..
                                        // since we know the index and value of parameter we sent check against the index and value received..
                                        if (IGNORE_PARAMETERS)
                                        {
                                            // set by the lasped timer to ignore sending of other paramters if the previous paramter has failed.
                                            // either MAV is to far out and we are wasting resources trying to send other paramters
                                        }
                                        else
                                        {
                                            this.Invoke(new Action(() => UploadParameter_Status(param_name)));
                                            this.Invoke(new Action(() => Upload_Parameter()));
                                        }
                                    }
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.HEARTBEAT:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_heartbeat_t received_HeartBeat = buffer.ByteArrayToStructure<MAVLink.mavlink_heartbeat_t>(6);
                                    Variables.set_FLIGHTMODE((Variables.FLIGHT_MODES)received_HeartBeat.base_mode);
                                }
                                break;


                            case (byte)MAVLink.MAVLINK_MSG_ID.ATTITUDE:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_attitude_t received_attitude_t = buffer.ByteArrayToStructure<MAVLink.mavlink_attitude_t>(6);
                                    Variables.rollDeg        = Variables.ToDeg(received_attitude_t.roll);
                                    Variables.pitchDeg       = Variables.ToDeg(received_attitude_t.pitch);
                                    Variables.yawDeg         = Variables.ToDeg(received_attitude_t.yaw);
                                    Variables.rollrateDeg    = Variables.ToDeg(received_attitude_t.rollspeed);
                                    Variables.pitchrateDeg   = Variables.ToDeg(received_attitude_t.pitchspeed);
                                    Variables.yawrateDeg     = Variables.ToDeg(received_attitude_t.yawspeed);
                                    Variables.rollraterad    = received_attitude_t.rollspeed;
                                    Variables.pitchraterad   = received_attitude_t.pitchspeed;
                                    Variables.yawraterad     = received_attitude_t.yawspeed;

                                    // Console.WriteLine("Att " + Variables.rollDeg + " P " + Variables.pitchDeg + " Y " + Variables.yawDeg);
                                }
                                break;
                                
                            case (byte) MAVLink.MAVLINK_MSG_ID.RAW_IMU:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_raw_imu_t raw_imu_t = buffer.ByteArrayToStructure<MAVLink.mavlink_raw_imu_t>(6);
                                    Variables.gyroX = raw_imu_t.xgyro;
                                    Variables.gyroY = raw_imu_t.ygyro;
                                    Variables.gyroZ = raw_imu_t.zgyro;
                                    Variables.accelX = raw_imu_t.xacc;
                                    Variables.accelY = raw_imu_t.yacc;
                                    Variables.accelZ = raw_imu_t.zacc;
                                    Variables.magX   = raw_imu_t.xmag;
                                    Variables.magY   = raw_imu_t.ymag;
                                    Variables.magZ   = raw_imu_t.zmag;

                                   // Console.WriteLine("Hi");
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_SETPOINT_INT:
                                {
                                    if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                    {
                                        MAVLink.mavlink_global_position_setpoint_int_t global_pos_int = buffer.ByteArrayToStructure<MAVLink.mavlink_global_position_setpoint_int_t>(6);
                                        Variables.auto_latitude = (float)((float)global_pos_int.latitude * 1e-7);
                                        Variables.auto_longitude = (float)((float)global_pos_int.longitude * 1e-7);
                                        Variables.auto_altitude = (float)((float)global_pos_int.altitude * 1e-3);
                                        Variables.auto_radius = (float)((float)global_pos_int.yaw * 1e-2);
                                       
                                    }
                                }
                                break;


                            case (byte)MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_gps_raw_int_t received_gps = buffer.ByteArrayToStructure<MAVLink.mavlink_gps_raw_int_t>(6);
                                    Variables.latitude = (float)(received_gps.lat * 1e-7);
                                    Variables.longitude = (float)(received_gps.lon * 1e-7);
                                    Variables.hMSL = (float)(received_gps.alt * 1e-3);
                                    Variables.gSpeed = (float)(received_gps.vel * 1e-2);
                                    Variables.cog = (float)(received_gps.cog * 1e-2);
                                    Variables.fix_type = received_gps.fix_type;
                                    Variables.numSatellites = received_gps.satellites_visible;
                                    Variables.gps_altitude = (float)(received_gps.alt * 1e-3);

                                    
                                   
                                    float Time   = (float)received_gps.time_usec;

                                    UInt64 hours   = (UInt64)Time / 100000;
                                    UInt64 minutes = ((UInt64)Time / 1000) - (hours * 100);
                                    byte seconds   = Convert.ToByte(((Time * 1e-3) % 1) * 100);
                                    // DateTime current_date = new DateTime();
                                    

                                    //Console.WriteLine("Time " + received_gps.time_usec +" UTC "+hours +":"+minutes + ":" + seconds);
                                    

                                    Log.Log.logwrite(Variables.latitude, Variables.longitude, Variables.hMSL, Variables.imu_altitude,  Variables.imu_climbrate,Variables.gSpeed, Variables.airspeed, Mavlink_Protocol.linkquality, Variables.link_rssi, Variables.throttle , Variables.rollDeg, Variables.pitchDeg, Variables.yawDeg);

                                    // Console.WriteLine("Fix " + Variables.fix_type+" Lat "+ Variables.latitude + " Lon " + Variables.longitude + " Speed "+Variables.gSpeed);
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.SERVO_OUTPUT_RAW:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_servo_output_raw_t servo_t = buffer.ByteArrayToStructure<MAVLink.mavlink_servo_output_raw_t>(6);
                                    Variables.chan1 = servo_t.servo1_raw;
                                    Variables.chan2 = servo_t.servo2_raw;
                                    Variables.chan3 = (ushort)(servo_t.servo3_raw);
                                    Variables.chan4 = servo_t.servo4_raw;
                                    Variables.chan5 = servo_t.servo5_raw;
                                    Variables.chan6 = servo_t.servo6_raw;
                                    Variables.chan7 = servo_t.servo7_raw;
                                    Variables.chan8 = servo_t.servo8_raw;

                                    // Console.WriteLine("AUX " + Variables.chan1 + " " + Variables.chan2 + " " +Variables.chan3 + " " +Variables.chan4 + " " +Variables.chan8);
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.VFR_HUD:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_vfr_hud_t vfr_t = buffer.ByteArrayToStructure<MAVLink.mavlink_vfr_hud_t>(6);
                                    Variables.airspeed      = vfr_t.airspeed;
                                    Variables.throttle      = vfr_t.throttle;
                                    Variables.vfr_groundspeed = vfr_t.groundspeed;
                                    Variables.imu_altitude  = vfr_t.alt;
                                    Variables.imu_climbrate = (vfr_t.climb * 100); // cms/s
                                    // if not in hilmode then this is equivakent to yaw
                                    Variables.heading = vfr_t.heading; // true heading indicator if not present then it is equivalent to the estimated true heading by yaw
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.SYS_STATUS:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_sys_status_t sys_status_t = buffer.ByteArrayToStructure<MAVLink.mavlink_sys_status_t>(6);
                                    Variables.batteryVolts = (float)sys_status_t.voltage_battery * 0.001f; // comes in millivolts
                                    Variables.error1 = sys_status_t.errors_count1;
                                    Variables.error2 = sys_status_t.errors_count2;
                                    Variables.error3 = sys_status_t.errors_count3;
                                    Variables.link_rssi = sys_status_t.drop_rate_comm;
                                }
                                break;

                            /* 
                             * Reading Waypoints
                             */ 
                            case (byte)MAVLink.MAVLINK_MSG_ID.MISSION_COUNT:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_mission_count_t mission_count = buffer.ByteArrayToStructure<MAVLink.mavlink_mission_count_t>(6);
                                    Variables.mission_count = mission_count.count;
                                    Variables.mission_seq = 0;
                                    //Console.WriteLine("Total waypoints: " + mission_count.count);
                                }
                                break;


                            case (byte)MAVLink.MAVLINK_MSG_ID.MISSION_ITEM:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_mission_item_t mission_item = buffer.ByteArrayToStructure<MAVLink.mavlink_mission_item_t>(6);
                                    Variables.mission_seq = mission_item.seq + 1;

                                    Variables.RecWP[mission_item.seq, 0] = mission_item.x;
                                    Variables.RecWP[mission_item.seq, 1] = mission_item.y;
                                    Variables.RecWP[mission_item.seq, 2] = mission_item.z;
                                    Variables.RecWP[mission_item.seq, 3] = mission_item.param1;
                                    Variables.RecWP[mission_item.seq, 4] = mission_item.command;

                                    //Console.WriteLine(mission_item.seq +"  "+ mission_item.x +"  "+mission_item.y + "  " + mission_item.z + "  "+ mission_item.param1 + "  "+mission_item.command);
                                }
                                break;

                            /*
                             * Sending Waypoints
                             */ 
                            case (byte)MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_mission_request_t mission_request = buffer.ByteArrayToStructure<MAVLink.mavlink_mission_request_t>(6);
                                    Variables.request_missionitem_target_system = mission_request.target_system;
                                    Variables.requested_missionitem_seq = mission_request.seq;
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.COMMAND_ACK:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_command_ack_t command_ack = buffer.ByteArrayToStructure<MAVLink.mavlink_command_ack_t>(6);                                  
                                    switch (command_ack.command)
                                    {
                                        case (ushort)MAVLink.MAV_CMD.MISSION_START:
                                            if (Variables.mission_start_sent == true)
                                            {
                                                Console.WriteLine("Mission Start Sent");
                                            }
                                            break;

                                        case (ushort)MAVLink.MAV_CMD.LOITER_TIME:
                                            if (Variables.gps_command_sent == true)
                                            {
                                                Console.WriteLine("Loiter Command Sent");
                                            }
                                            break;
                                    }
                                }
                                break;

                            case (byte)MAVLink.MAVLINK_MSG_ID.MISSION_ACK:
                                if (Variables.UAVID == buffer[3] && !Variables.WAITING_FOR_PARAM_LIST)
                                {
                                    MAVLink.mavlink_mission_ack_t mission_ack = buffer.ByteArrayToStructure<MAVLink.mavlink_mission_ack_t>(6);
                                    Variables.mission_ack           = mission_ack.type;
                                    Variables.ack_message_received  = true;
                                }
                                break;
                        }

                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
        #endregion


        #region DOUI TIMER REGION
        Xplane.sitl_fdm xplaneBuffer = new Hil.sitl_fdm();
        DateTime old_millis;
        DateTime curMillis;
        TimeSpan dt_span;
        private void DoUI_Timer_Tick(object sender, EventArgs e)
        {
            old_millis = curMillis;
            curMillis  = DateTime.Now;
            dt_span    = curMillis - old_millis;
            //Console.WriteLine(dt_span.Milliseconds);
            fast_30Hz_loop();
            medium_10Hz_loop();
        }


        /// <summary>
        ///  fast loop 30Hz
        /// </summary>
        float[] graphing_variables = new float[14];
        void fast_30Hz_loop()
        {
            if (!Variables.WAITING_FOR_PARAM_LIST)
            {
                float[] aux = { Variables.chan1, Variables.chan2, Variables.chan3, Variables.chan4 };

                Attitude_Indicator.Invalidate();

                if (xplane10.READY_XPLANE)
                {
                    xplane10.Get_FromSimulator(ref xplaneBuffer);

                    Mavlink_Protocol.sendPacket(xplane10.Mavlink_PackHilState(xplaneBuffer));

                    Mavlink_Protocol.sendPacket(xplane10.Mavlink_VfrAirspeed(xplaneBuffer));                   

                    xplane10.SendToSim(aux);
                }
            }

            // graphing tool
            if (SettingsCntrl != null)
            {
                graphing_variables[0]  = Variables.rollraterad;
                graphing_variables[1]  = Variables.pitchraterad;
                graphing_variables[2]  = Variables.yawraterad;
                graphing_variables[3]  = Variables.ToRad(Variables.rollDeg);
                graphing_variables[4]  = Variables.ToRad(Variables.pitchDeg);
                graphing_variables[5]  = Variables.ToRad(Variables.yawDeg);
                graphing_variables[6]  = Variables.ToRad(Variables.hilrollDeg);
                graphing_variables[7]  = Variables.ToRad(Variables.hilpitchDeg);
                graphing_variables[8]  = Variables.ToRad(Variables.hilyawDeg);

                graphing_variables[9]  = Variables.imu_altitude;   // altitude
                graphing_variables[10] = Variables.imu_climbrate;  // cm/s

                graphing_variables[11] = (float)Variables.accelX * 9.80665f / (8192);
                graphing_variables[12] = (float)Variables.accelY * 9.80665f / (8192);
                graphing_variables[13] = (float)Variables.accelZ * 9.80665f / (8192);

                SettingsCntrl.GraphPane_Draw(graphing_variables);
            }
        }

        /// <summary>
        ///  medium 10Hz loop
        /// </summary>
        int medium_10Hz_counter = 0;
        void medium_10Hz_loop()
        {
            switch (medium_10Hz_counter)
            {
                case 0:
                    medium_10Hz_counter++;
                    break;

                case 1:
                    medium_10Hz_counter++;                    
                    if (!Variables.WAITING_FOR_PARAM_LIST)
                    {
                        if (SettingsCntrl != null)
                        {
                            SettingsCntrl.ServoOutputs_Draw(Variables.chan1, Variables.chan2, Variables.chan3, Variables.chan4);
                        }
                    }
                    break;

                case 2:
                    medium_10Hz_counter = 0;
                    slow_1Hz_loop();
                    break;
            }
        }


        /// <summary>
        ///  slow loop 3.3Hz 
        /// </summary>
        void slow_1Hz_loop()
        {
            switch (slow_1Hz_counter)
            {
                case 0:
                    slow_1Hz_counter++;
                    if (!Variables.WAITING_FOR_PARAM_LIST)
                    {
                        Display_Vfr();
                    }
                    break;

                case
                1:
                    slow_1Hz_counter++;

                    GPRS_Map(new GMap.NET.PointLatLng(Variables.gprs_latitude, Variables.gprs_longitude));

                    if (!Variables.WAITING_FOR_PARAM_LIST)
                    {
                        // infact heading and yawdeg should both be true heading, here heading is used as the true heading from hil and yaw as the dcm true heading
                        GmapDirection(new GMap.NET.PointLatLng(Variables.latitude, Variables.longitude), Variables.rollDeg, Variables.yawDeg, Variables.heading);
                    
                        gMapControl.Invalidate();
                    }
                    break;

                case 2:
                    slow_1Hz_counter++;
                    if (!Variables.WAITING_FOR_PARAM_LIST)
                    {
                        Mavlink_Protocol.Send_HeartBeat();
                    }
                    break;

                case 3:
                    slow_1Hz_counter++;
                    break;

                case 4:
                    slow_1Hz_counter++;
                    if (!Variables.WAITING_FOR_PARAM_LIST)
                    {
                        // send info to antenna tracker
                        MAVLink.mavlink_gps_raw_int_t received_gps = new MAVLink.mavlink_gps_raw_int_t();
                        received_gps.lat  = (Int32)(Variables.latitude * 1e7);
                        received_gps.lon = (Int32)(Variables.longitude * 1e7);
                        received_gps.alt = (Int32)(Variables.gps_altitude * 1e3);
                        received_gps.fix_type = Variables.fix_type;
                        received_gps.vel = (ushort)(Variables.gSpeed * 100);
                        received_gps.satellites_visible = Variables.numSatellites;
                        received_gps.cog = (ushort)(Variables.cog * 100);
                        //SettingsCntrl.send_gps_raw_int(received_gps);
                    }
                    break;

                case 5:
                    slow_1Hz_counter++;
                    break;

                case 6:
                    slow_1Hz_counter++;
                    break;

                case 7:
                    slow_1Hz_counter++;
                    break;

                case 8:
                    slow_1Hz_counter++;
                    break;

                case 9:
                    slow_1Hz_counter = 0;
                    if(SerialPortObject.IsOpen)
                        Zoom_and_Center_DirectionMArker();
                    break;

            }
        }
        #endregion

       

       
        #region MAP CONTROL REGION
        bool hovering = false;
        private void gMapControl_MouseHover(object sender, EventArgs e)
        {
           //Console.WriteLine("Hovering");
        } 


        Pen routePen = new Pen(Color.FromArgb(255, Color.Pink), 3);
        List<PointLatLng> WPCoordinates = new List<PointLatLng>();
        Pen polyPen = new Pen(Brushes.Orange, 3);
        bool _drag_marker = false;
        GMapWayPointMarker _dragged_marker;
        int Home_Counter = 0;
        /// <summary>
        /// GmapDirection Marker over the control area.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="roll"></param>
        /// <param name="yaw"></param>
        /// <param name="cog"></param>     
        Pen nextWP_pen                  = new Pen(Brushes.Navy, 3);
        GMapPolygon nextWP_Polygon      = new GMapPolygon(new List<PointLatLng>(), "nextWP");
        List<PointLatLng> nextWPList    = new List<PointLatLng>();
        private void GmapDirection(GMap.NET.PointLatLng pt, float roll, float yaw, float cog)
        {
            if(Variables.fix_type > 1 && Home_Counter <10)
            {
                Home_Counter++;
            }

            if((Home_Counter > 2) &&  (Home_Counter < 4))
            {
                
                // GPS Stuff::
                Settings.GPSSettings.Default.Home_Latitude  = pt.Lat;
                Settings.GPSSettings.Default.Home_Longitude = pt.Lng;
                Settings.GPSSettings.Default.Home_Altitude  = Variables.gps_altitude; 
                Settings.GPSSettings.Default.Save();

                Console.WriteLine("Saving new home latitude and longitude");

                // 
                _HomeMarker.Position = pt;
                gMapControl.Invalidate();
            }

           // nextWPList.Add(pt);
           // nextWPList.Add(new PointLatLng(Variables.auto_latitude - 6, Variables.auto_longitude + 38));
            nextWP_Polygon.Clear();
            nextWP_Polygon.Points.Add(pt);
            nextWP_Polygon.Points.Add(new PointLatLng(Variables.auto_latitude, Variables.auto_longitude));
            nextWP_Polygon.Stroke = nextWP_pen;
            WayPointOverlay.Polygons.Add(nextWP_Polygon);

            target_marker.Position    = new PointLatLng(Variables.auto_latitude, Variables.auto_longitude);
            target_marker.ToolTipMode = MarkerTooltipMode.Always;
            target_marker.ToolTipText = "Alt: "+Variables.auto_altitude + "m"+"\n" + "Rad: "+ Variables.auto_radius +"m" +"\n" + "Dis: " + (gMapControl.MapProvider.Projection.GetDistance(new PointLatLng(Variables.auto_latitude, Variables.auto_longitude), pt) * 1000).ToString("0.00") + "m";
            target_marker.Radius      = (int)Variables.auto_radius;

            _GmapDirectionalMarker.set_roll    = roll;
            _GmapDirectionalMarker.set_yaw     = yaw;
            _GmapDirectionalMarker.Position    = pt;
            _GmapDirectionalMarker.set_cog     = cog;
            _GmapDirectionalMarker.ToolTipMode = MarkerTooltipMode.Always;
            _GmapDirectionalMarker.ToolTipText = Variables.imu_altitude.ToString("0") + "m";

            _GmapRoute.Points.Add(pt);
            
            //_GmapRoute
            HeaderOverlay.Routes.Add(_GmapRoute);
            HeaderOverlay.Routes.RemoveAt(0);

            if (_GmapRoute.Points.Count >= 200)
                _GmapRoute.Points.Clear();
        }


        private void GPRS_Map(GMap.NET.PointLatLng pt)
        {
            GPRS_Marker.Position = pt;
            GPRS_Route.Points.Add(pt);
            HeaderOverlay.Routes.Add(GPRS_Route);
            HeaderOverlay.Routes.RemoveAt(0);
            if (GPRS_Route.Points.Count >= 200)
                GPRS_Route.Points.Clear();

            uint pad = 20;
            MapControl_Rectangle_Mini.X         = (int)(pad);
            MapControl_Rectangle_Mini.Y     = (int)(Attitude_Indicator.ClientSize.Height + pad);
            MapControl_Rectangle_Mini.Width = (int)(Attitude_Indicator.ClientSize.Width);
            MapControl_Rectangle_Mini.Height = (int)(gMapControl.ClientSize.Height - Attitude_Indicator.ClientSize.Width - pad / 2);

            MapControl_Rectangle.X = (int)(Attitude_Indicator.ClientSize.Width + pad);
            MapControl_Rectangle.Y = (int)pad;
            MapControl_Rectangle.Width = (int)(gMapControl.ClientSize.Width - Attitude_Indicator.ClientSize.Width - 2 * pad);
            MapControl_Rectangle.Height = (int)(gMapControl.ClientSize.Height - 2 * pad);

            DirectionMarkerPoints.X = (int)gMapControl.FromLatLngToLocal(GPRS_Marker.Position).X;
            DirectionMarkerPoints.Y = (int)gMapControl.FromLatLngToLocal(GPRS_Marker.Position).Y;

            if (!MapControl_Rectangle.Contains(DirectionMarkerPoints))
            {
               // gMapControl.ZoomAndCenterRoute(GPRS_Route);
                //gMapControl.Zoom = MapZoomLevel;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Home_Button_Click(object sender, EventArgs e)
        {
            Settings.GPSSettings.Default.Home_Latitude = _GmapDirectionalMarker.Position.Lat;
            Settings.GPSSettings.Default.Home_Longitude = _GmapDirectionalMarker.Position.Lng;
            Settings.GPSSettings.Default.Save();
            _HomeMarker.Position = _GmapDirectionalMarker.Position;

            gMapControl.Invalidate();

            WPCoordinates_Update(0, _HomeMarker.Position);
            if (WPCoordinates.Count <= 1) { return; }
            WayPointOverlay.Polygons.Clear();
            WayPointOverlay.Polygons.Add(new GMapPolygon(WPCoordinates, "Poly")
            {
                Fill = Brushes.Transparent,
                Stroke = polyPen
            });
        }



        /// <summary>
        /// Clear Map Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Map_Button_Click(object sender, EventArgs e)
        {        
            WayPointOverlay.Markers.Clear();
            WayPointOverlay.Polygons.Clear();
            WPCoordinates.Clear();
            WayPoint_DataGridView.Rows.Clear();
            WPCoordinates.Add(_HomeMarker.Position);
            WayPointOverlay.Markers.Add(_HomeMarker);
            gMapControl.Invalidate();

            SurveyOverlay.Markers.Clear();
            SurveyOverlay.Polygons.Clear();
            SurveyCoordinates.Clear();

            PlaybackOverlay.Markers.Clear();
            PlaybackOverlay.Routes.Clear();
            PlaybackOverlay.Polygons.Clear();
        }


        private void update_homePos(PointLatLng pt)
        {
            _HomeMarker.Position = pt;
        }


        /// <summary>
        /// chnaging map zoom level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void htrackBar_ValueChanged(object sender, EventArgs e)
        {
            MapZoomLevel = htrackBar.Value;
            gMapControl.Zoom = MapZoomLevel;
            Settings.GPSSettings.Default.Map_Zoom = MapZoomLevel;
            Settings.GPSSettings.Default.Save();
        }


        /// <summary>
        /// map type changing method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  "Google-HB", "Google-S", "Bing-HB", "Bing-S", "OpenStreet"

            int index = MapBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    gMapControl.MapProvider = GoogleHybridMapProvider.Instance;
                    break;

                case 1:
                    gMapControl.MapProvider = GoogleSatelliteMapProvider.Instance;
                    break;

                case 2:
                    gMapControl.MapProvider = BingHybridMapProvider.Instance;
                    break;

                case 3:
                    gMapControl.MapProvider = BingSatelliteMapProvider.Instance;
                    break;

                case 4:
                    gMapControl.MapProvider = OpenStreetMapProvider.Instance;
                    break;
            }

            Settings.GPSSettings.Default.Map_Type = (byte)index;
            Settings.GPSSettings.Default.Save();
        }


     
        /// <summary>
        /// map control has been clocked. ADD WAYPOINT THROUGH THIS METHOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param
        PointLatLng poi_latlng;
        List<PointLatLng> SurveyCoordinates = new List<PointLatLng>();
        //Brush surveyBrush;
        SolidBrush surveyBrush = new SolidBrush(Color.FromArgb(30, Color.Red));
        Pen surveyPen = new Pen(Color.Pink);
        private void gMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("What");
            if (_drag_marker)
                return;          

            if (ModifierKeys == Keys.Control && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                GMap.NET.PointLatLng points = (sender as GMapControl).FromLocalToLatLng(e.X, e.Y);
                SurveyCoordinates.Add(points);         
                GMapPolygon _WPPolygon = new GMapPolygon(SurveyCoordinates, "Polygon")
                {
                    Fill = surveyBrush,
                    Stroke = surveyPen,
                };

                SurveyOverlay.Polygons.Clear();
                SurveyOverlay.Polygons.Add(_WPPolygon);             
            }
            else if(ModifierKeys == Keys.Control && ModifierKeys == Keys.C)
            {
                Zoom_and_Center_DirectionMArker();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // remember homemarker is index 0.
                GMap.NET.PointLatLng points = (sender as GMapControl).FromLocalToLatLng(e.X, e.Y);

                WPCoordinates_Update(0, _HomeMarker.Position);

                WPCoordinates.Add(points);
                float alt    = 100;
                float radius = int.Parse(RadiusNumericUpDown.Value.ToString());

                Bitmap _bmp_wp = Properties.Resources.generic_waypoint_50x58_;
                GMapWayPointMarker _WPMarker = new GMapWayPointMarker(WPCoordinates.ElementAt(WPCoordinates.Count-1), _bmp_wp, (int)radius) 
                { 
                    ToolTipMode = MarkerTooltipMode.Always, 
                    ToolTipText = WayPointOverlay.Markers.Count.ToString()
                };              

                WayPointDataGrid_Insert((float)WPCoordinates.ElementAt(WPCoordinates.Count - 1).Lat, (float)WPCoordinates.ElementAt(WPCoordinates.Count - 1).Lng, radius, alt);

                GMapPolygon _WPPolygon = new GMapPolygon(WPCoordinates, "Polygon") 
                {
                    Fill = Brushes.Transparent,
                   Stroke = polyPen,
                };

                WayPointOverlay.Polygons.Clear();
                WayPointOverlay.Polygons.Add(_WPPolygon);
                WayPointOverlay.Markers.Add(_WPMarker);             
            }
            else
            {
                string[] items_ = new string[] { }; 

                poi_latlng = gMapControl.FromLocalToLatLng(e.X, e.Y);
                gMapControl.ContextMenuStrip = new ContextMenuStrip();
                gMapControl.ContextMenuStrip.Items.Add("POI");
                gMapControl.ContextMenuStrip.Items.Add("RAKE");
                gMapControl.ContextMenuStrip.Items.Add("RTL");
                gMapControl.ContextMenuStrip.Items.Add("REPORT");
                gMapControl.ContextMenuStrip.Items.Add("SPEED");
                gMapControl.ContextMenuStrip.Items.Add("MISSION START");
                gMapControl.ContextMenuStrip.Items.Add("GRID");
                gMapControl.ContextMenuStrip.Items.Add("Populate Grid");

                //(gMapControl.ContextMenuStrip.Items[6] as ToolStripMenuItem).DropDownItems.Add("Create");
                //(gMapControl.ContextMenuStrip.Items[6] as ToolStripMenuItem).DropDownItems.Add("Populate");

                //gMapControl.ContextMenuStrip.Items.Add("HERY", Properties.Resources.plan_mission_status_off_50x50_, iam_clicked);
                //gMapControl.ContextMenuStrip.OwnerItem = 
                // gMapControl.ContextMenuStrip.OwnerItem = ow
                gMapControl.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
                gMapControl.ContextMenuStrip.Show(Cursor.Position);
                gMapControl.ContextMenuStrip = null; 
            }
        }

        private void iam_clicked(object sender, EventArgs e)
        {
            MessageBox.Show("It Works");
        }
        
       
        private void gMapControl_OnMarkerClick(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(GMapWayPointMarker))
            {
                //Console.WriteLine(sender.ToString());
            }
        }


        
        /// <summary>
        /// context menu form
        /// </summary>
        /// <param name="text"></param>
        private void Context_Form(string text)
        {
            /// get lat and lon
            /// show form to fill radius and altitude
            /// send items to fcs
            PointLatLng poi_items = poi_latlng;
            Form poi_form = new Form();
            poi_form.Size = new Size(240, 140);
            poi_form.StartPosition = FormStartPosition.Manual;
            poi_form.Location = Cursor.Position;
            poi_form.Text = text;

            Label alt_label = new Label();
            alt_label.Text = "Altitude in metres";
            alt_label.Size = new System.Drawing.Size(120, 20);

            TextBox alt_text = new TextBox();
            alt_text.Size = new System.Drawing.Size(50, 20);
            alt_text.Location = new Point(alt_label.Location.X + alt_label.Size.Width + 5, alt_label.Location.Y);
            alt_text.Text = "100";

            Label rad_label = new Label();
            rad_label.Text = "Radius in metres";
            rad_label.Size = new System.Drawing.Size(120, 20);
            rad_label.Location = new Point(alt_label.Location.X, alt_label.Location.Y + alt_label.Size.Height + 5);

            TextBox rad_text = new TextBox();
            rad_text.Size = new System.Drawing.Size(50, 20);
            rad_text.Location = new Point(alt_label.Location.X + alt_label.Size.Width + 5, rad_label.Location.Y);
            rad_text.Text        = "20";

            Label time_label     = new Label();
            time_label.Text      = "Time in Seconds"; // 4 minutes maximum
            time_label.Size      = new System.Drawing.Size(120, 20);
            time_label.Location  = new Point(rad_label.Location.X, rad_label.Location.Y + rad_label.Size.Height + 5);
             
            TextBox time_text    = new TextBox();
            time_text.Size       = new System.Drawing.Size(50, 20);
            time_text.Location   = new Point(time_label.Location.X + time_label.Size.Width + 5, time_label.Location.Y);
            time_text.Text       = "60";            // standard is 1 minute of surveillance

            Button ok_text       = new Button();
            ok_text.Size         = new System.Drawing.Size(50, 20);
            ok_text.Text         = "OK";
            ok_text.Location     = new Point(time_label.Location.X + 40, time_label.Location.Y + time_label.Size.Height + 5);
            ok_text.DialogResult = System.Windows.Forms.DialogResult.OK;

            Button cancel_text = new Button();
            cancel_text.Size = new System.Drawing.Size(50, 20);
            cancel_text.Text = "Cancel";
            cancel_text.Location = new Point(ok_text.Location.X + ok_text.Size.Width + 5, ok_text.Location.Y);
            cancel_text.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            poi_form.Controls.AddRange(new Control[] { alt_label, alt_text, rad_label, rad_text, time_label, time_text,ok_text, cancel_text });
            if (poi_form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // check radius and altitude
                float radius, altitude = 0;
                float time = 0;
                
                if (!float.TryParse(time_text.Text, out time))
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Invalid Time");
                    return;
                }

                if (!float.TryParse(rad_text.Text, out radius))
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Invalid Radius");
                    return;
                }

                if (!float.TryParse(alt_text.Text, out altitude))
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Invalid Altitude");
                    return;
                }

                if (radius < 5 || radius > 255)
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Radius out of range");
                    return;
                }

                if (altitude < 15 || altitude > 3300)
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Altitude out of range");
                    return;
                }

                if (time < 1 || time > 600) // 10minutes of loiter
                {
                    poi_form.Close();
                    poi_form.Dispose();
                    poi_form = null;
                    MessageBox.Show("Time out of range");
                    return;
                }

                // send message to fcs
                /*
                MAV_CMD_NAV_LOITER_TIME	Loiter around this MISSION for X seconds
                Mission Param #1	Seconds (decimal)
                Mission Param #2	Empty
                Mission Param #3	Radius around MISSION, in meters. If positive loiter clockwise, else counter-clockwise
                Mission Param #4	Forward moving aircraft this sets exit xtrack location: 0 for center of loiter wp, 1 for exit location. Else, this is desired yaw angle
                Mission Param #5	Latitude
                Mission Param #6	Longitude
                Mission Param #7	Altitude
                 */ 

                MAVLink.mavlink_set_global_position_setpoint_int_t gps_pos = new MAVLink.mavlink_set_global_position_setpoint_int_t();


#if !UNDER_CONST
                MAVLink.mavlink_command_long_t _gps_command = new MAVLink.mavlink_command_long_t();
                _gps_command.param1 = time;
                _gps_command.param3 = radius;
                _gps_command.param4 = 0;
                _gps_command.param5 = (float)poi_items.Lat;
                _gps_command.param6 = (float)poi_items.Lng;
                _gps_command.param7 = altitude;
                _gps_command.command = (byte)MAVLink.MAV_CMD.LOITER_TIME;
                _gps_command.target_system = Variables.UAVID;
                _gps_command.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_SYSTEM_CONTROL;
                _gps_command.confirmation = 0;
                

                Variables.gps_command_sent = false;
                Mavlink_Protocol.sendPacket(_gps_command);
                //Mavlink_Protocol.sendPacket(_gps_command);
                Variables.gps_command_sent = true;
#else


                gps_pos.latitude = (int)(poi_items.Lat * 1e7);
                gps_pos.longitude= (int)(poi_items.Lng * 1e7);
                gps_pos.altitude = (int)(altitude * 1e3);
                gps_pos.yaw      = (short)(radius * 1e2);        // define as radius or rake time
                gps_pos.coordinate_frame = (byte)time;
               
                if (text == "POI")
                {
                    gps_pos.coordinate_frame = 100;  // Unique ID for POI
                }
                else if(text == "RAKE")
                {
                     gps_pos.coordinate_frame = 101;  // Unique ID for Rake
                }

                Mavlink_Protocol.sendPacket(gps_pos);
                Mavlink_Protocol.sendPacket(gps_pos);
#endif

                poi_form.Close();
                poi_form.Dispose();
                poi_form = null;
            }
            else
            {
                poi_form.Close();
                poi_form.Dispose();
                poi_form = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        ToolTip _tip = new ToolTip();
        private void Report_Form()
        {
            double bearing = gMapControl.MapProvider.Projection.GetBearing(_HomeMarker.Position, poi_latlng);
            double distance = gMapControl.MapProvider.Projection.GetDistance(_HomeMarker.Position, poi_latlng);
            //ToolTip _tip = new ToolTip();
            string _tiptext =
                "     REPORT"
                +"\n"
                + "Distance     : " + distance.ToString("0.00") + " km"
                + "\n"
                + "Bearing      : " + bearing.ToString("0") + " degress" 
                + "\n"
                + "Latitude     : " + poi_latlng.Lat.ToString("0.0000000")
                + "\n"
                + "Longitude    : " + poi_latlng.Lng.ToString("0.0000000");
            _tip.Show(_tiptext, gMapControl, 15000);          
        }


        private void Mission_Start_Command(){
            MAVLink.mavlink_command_long_t mission_start = new MAVLink.mavlink_command_long_t();
            mission_start.command = (ushort)MAVLink.MAV_CMD.MISSION_START;
            mission_start.param1  = 0;
            mission_start.param2  = Variables.WP.Length - 1;
            mission_start.target_system = Variables.UAVID;
            mission_start.target_component = (byte)MAVLink.MAV_COMPONENT.MAV_COMP_ID_SYSTEM_CONTROL;
            mission_start.confirmation = 0;

            Variables.mission_start_sent = false;
            Mavlink_Protocol.sendPacket(mission_start);
            Variables.mission_start_sent = true;
        }


        /// <summary>
        /// 
        /// </summary>
        SolidBrush gridBrush = new SolidBrush(Color.FromArgb(30, Color.Cyan));
        Pen gridPen = new Pen(Color.Aqua);
        private void Create_Grid()
        {
            // get latitude limits
            List<PointLatLng> gridPtslat = new List<PointLatLng>();
            List<PointLatLng> gridPtslng = new List<PointLatLng>();
            List<PointLatLng> gridPts    = new List<PointLatLng>();
            //List<PointLatLng> polygridPts = new List<PointLatLng>();
            PointLatLng equator          = new PointLatLng(0, 0);


            foreach (PointLatLng ptlatlng in SurveyCoordinates)
            {
                gridPtslat.Add(new PointLatLng(ptlatlng.Lat, 0));
                gridPtslng.Add(new PointLatLng(0, ptlatlng.Lng));
            }

            // classes
            Helper poslatHelper = new Helper();
            Helper neglatHelper = new Helper();
            Helper poslngHelper = new Helper();
            Helper neglngHelper = new Helper();

            foreach (PointLatLng ptlatlng in gridPtslat)
            {
                //Console.WriteLine((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng));
                poslatHelper.RecordMax((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lat);
                    //neglatHelper.RecordMax((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lat);
                poslatHelper.RecordMin((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lat);
                    //neglatHelper.RecordMin((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lat);
            }

            foreach (PointLatLng ptlatlng in gridPtslng)
            {
                poslngHelper.RecordMax((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lng);
                    //neglngHelper.RecordMax((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lng);
                poslngHelper.RecordMin((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lng);
                    //neglngHelper.RecordMin((float)gMapControl.MapProvider.Projection.GetDistance(equator, ptlatlng), ptlatlng.Lng);
            }

            gridPts.Add(new PointLatLng(poslatHelper.max, poslngHelper.min)); // pt1 index0
            gridPts.Add(new PointLatLng(poslatHelper.min, poslngHelper.min)); // pt2 index1
                                                                              //     index2
            
            // ptn-1
            PointLatLng pt1   = new PointLatLng(poslatHelper.max, poslngHelper.min);
            PointLatLng pt2   = new PointLatLng(poslatHelper.min, poslngHelper.min);
            PointLatLng pt_n1 = new PointLatLng(poslatHelper.min, poslngHelper.max);

            // distance
            double distance    = gMapControl.MapProvider.Projection.GetDistance(pt2, pt_n1); // in Kilometres
            distance *= 1000;
            UInt16 partitions  = Convert.ToUInt16(distance / 50);
            int marker_columns = partitions - 1;

           Console.WriteLine(distance +"       "+partitions);

           bool reverse = false;
            // positions latmax, latmin, lonmax + 50m, lonmin + 50m
            for (int i = 0; i < marker_columns; i++)
            {
                // offset longitude
                double R = 6370000;
                R       = R * Math.Cos(pt2.Lat);
                double offset = 50 * (i + 1);
                double lng_offset = (offset / R) * (180 / Math.PI);

                double new_lng = lng_offset + pt2.Lng;

                // sanity checker
               PointLatLng new_pt      =  new PointLatLng(poslatHelper.min, new_lng);
               double sanity_distance  = gMapControl.MapProvider.Projection.GetDistance(new_pt, pt_n1) * 1000;

               if (sanity_distance <= 50)
               {
                   break;
               }

                if (!reverse)
                {
                    gridPts.Add(new PointLatLng(poslatHelper.min, new_lng)); // ptn-1
                    gridPts.Add(new PointLatLng(poslatHelper.max, new_lng)); // ptn
                    reverse = true;
                }
                else
                {
                    gridPts.Add(new PointLatLng(poslatHelper.max, new_lng)); // ptn
                    gridPts.Add(new PointLatLng(poslatHelper.min, new_lng)); // ptn-1
                    reverse = false;
                }
            }

            if (!reverse)
            {
                gridPts.Add(new PointLatLng(poslatHelper.min, poslngHelper.max)); // ptn-1
                gridPts.Add(new PointLatLng(poslatHelper.max, poslngHelper.max)); // ptn
            }
            else
            {
                gridPts.Add(new PointLatLng(poslatHelper.max, poslngHelper.max)); // ptn
                gridPts.Add(new PointLatLng(poslatHelper.min, poslngHelper.max)); // ptn-1
            }

            //polygridPts.Add(gridPts.ElementAt(0));
            //polygridPts.Add(gridPts.ElementAt(1));
            //polygridPts.Add(new PointLatLng(poslatHelper.min, poslngHelper.max));
            //polygridPts.Add(new PointLatLng(poslatHelper.max, poslngHelper.max));

            //GMapPolygon poly = new GMapPolygon(polygridPts, "Poly")
            //{ 
            //Fill = gridBrush,
            //Stroke = gridPen
            //};
            //  GMap.NET.WindowsForms.Markers.GMarkerGoogle empty = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(gridPts,)     
            // SurveyOverlay.Polygons.Add(poly);

            Bitmap _bmp_wp = Properties.Resources.generic_waypoint_50x58_;
            UInt16 radius  = Convert.ToUInt16(RadiusNumericUpDown.Value);
            for(int i = 0; i < gridPts.Count; i++)
            {              
                GMapWayPointMarker _WPMarker = new GMapWayPointMarker(gridPts.ElementAt(i), _bmp_wp, radius);
                //_WPMarker.ToolTipMode = MarkerTooltipMode.Always;
                //_WPMarker.ToolTipText = (i + 1).ToString();
                //SurveyOverlay.Markers.Add(_WPMarker);
                MouseEventArgs _eventargs = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)(gMapControl.FromLatLngToLocal(_WPMarker.Position)).X, (int)(gMapControl.FromLatLngToLocal(_WPMarker.Position)).Y, 20);
                object _sender            = gMapControl;
                gMapControl_MouseClick(_sender, _eventargs);
            }
        }


        private void populate_grid()
        {

        }

        /// <summary>
        /// get the clicked item and perform the neccessary action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuStrip_ItemClicked(object sender, EventArgs e)
        {
            ToolStripItemClickedEventArgs argument = e as ToolStripItemClickedEventArgs;   
          
            switch (argument.ClickedItem.ToString())
            {
                case "POI":
                    Context_Form(argument.ClickedItem.ToString());
                    break;

                case "RTL":

                    break;

                case "RAKE":
                    Context_Form(argument.ClickedItem.ToString());
                    break;

                case "REPORT":
                    Report_Form();
                    break;

                case "MISSION START":
                    Mission_Start_Command();
                    break;

                case "GRID":
                    Create_Grid();
                    break;

                case "Populate Grid":

                    break;
            }
        }

        /// <summary>
        /// mouse is down over the control area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int marker_clicked= 0;
        private void gMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (GMapWayPointMarker marker in WayPointOverlay.Markers)
            {
                /*
                if(_HomeMarker.IsMouseOver)
                {
                    _drag_marker = false;
                    return;
                }
                */

                if (marker.IsMouseOver)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        marker_clicked = WayPointOverlay.Markers.IndexOf(marker);
                        if (_toolStripDropDown != null)
                        {
                            _toolStripDropDown = null;
                        }
                    }
                    else
                    {
                        if (mission_planning_mode)
                        {
                            _drag_marker = true;
                            _dragged_marker = marker;
                        }
                    }
                }
            }           
        }



        private void _toolStripDropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            /*
             * 1. Set Current
             * 1. GeoMap
             * 2. GeoFence
             */ 
            if(e.ClickedItem.ToString() == "Set Current")
            {
                Mavlink_Protocol.Send_Sequence(marker_clicked);
            }
            else if (e.ClickedItem == _toolStripDropDown.Items[1])
            {

            }
            //MessageBox.Show(e.ClickedItem.ToString());
        }


        /// <summary>
        ///  mouse is moving over the control area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapControl_MouseMove(object sender, MouseEventArgs e)
        {
             if(_drag_marker)
             {
                 _dragged_marker.Position = gMapControl.FromLocalToLatLng(e.X, e.Y);
                 Marker_Moved(_dragged_marker);
             }
        }


        /// <summary>
        /// mouse is up over the control area after a mouse down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if(_drag_marker)
            {
                _drag_marker = false;
                _dragged_marker = null;               
            }
        }

        /// <summary>
        /// 1 based maker index -> corresponds to the Id_Column of the datagrid
        /// Marker0 is homeMarker
        /// </summary>
        /// <param name="marker_index"></param>
        private void Marker_Removed(int marker_index)
        {
            WPCoordinates.RemoveAt(marker_index);
            WayPointOverlay.Polygons.Clear(); 
            WayPointOverlay.Markers.RemoveAt(marker_index);
            WayPointOverlay.Polygons.Add(new GMapPolygon(WPCoordinates, "Poly") 
            { 
                Fill = Brushes.Transparent,
                Stroke = polyPen
            });

            for(int index = 1; index < WayPointOverlay.Markers.Count; index++)
            {
                WayPointOverlay.Markers.ElementAt(index).ToolTipText = index.ToString();
            }
        }


        private void WPCoordinates_Update(int index, PointLatLng pt){
            PointLatLng[] _list = WPCoordinates.ToArray();
            _list[index] = pt;
            WPCoordinates = _list.ToList();
        }


        private void WPCoordinates_Remove(int index){
            WPCoordinates.RemoveAt(index);
        }


        private void WPRadius_Update(int index, int Radius)
        {
            (WayPointOverlay.Markers.ElementAt(index) as GMapWayPointMarker).Radius = Radius;
            gMapControl.Invalidate();
        }


        private void RadiusNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int length = WayPointOverlay.Markers.Count;
            for (int i = 0; i < length; i++)
            {
                (WayPointOverlay.Markers.ElementAt(i) as GMapWayPointMarker).Radius = (int)RadiusNumericUpDown.Value;
                WayPoint_DataGridView.Rows[i].Cells["Radius_Column"].Value = (int)RadiusNumericUpDown.Value;
            }
            gMapControl.Invalidate();
        }

        /// <summary>
        ///  marker is being moved over map
        /// </summary>
        /// <param name="_marker"></param>
        private void Marker_Moved(GMapWayPointMarker _marker)
        {   
            if (_marker == null)
                return;

            if(WayPointOverlay.Markers.Contains(_marker))
            {
                int index = WayPointOverlay.Markers.IndexOf(_marker);

                // Meaning we havent added any points to WPCoordinates which the first Point gets added when the map is clicked
                if (WPCoordinates.Count < 1) { return; }

                // WPCoordinates.RemoveAt(index);

                // WPCoordinates.Insert(index, _marker.Position);

                /*** Under Testing ***/
                WPCoordinates_Update(index, _marker.Position);

                WayPointOverlay.Polygons.Clear();

                WayPointOverlay.Markers.ElementAt(index).Position = _marker.Position;

                WayPointOverlay.Polygons.Add(new GMapPolygon(WPCoordinates, "Poly")
                {
                   
                    Fill = Brushes.Transparent,
                    Stroke = polyPen
                });

                // Home Marker is not placed on the DataGridView
                if (index < 1) { return; }

                WayPointDataGrid_UpdateRecord(index - 1, _marker);
            }         
        }

        
        /// <summary>
        /// zoom and center directional marker
        /// </summary>
        private void Zoom_and_Center_DirectionMArker()
        {
            uint pad = 20;

            MapControl_Rectangle_Mini.X      = (int)(pad);
            MapControl_Rectangle_Mini.Y      = (int)(Attitude_Indicator.ClientSize.Height + pad);
            MapControl_Rectangle_Mini.Width  = (int)(Attitude_Indicator.ClientSize.Width );
            MapControl_Rectangle_Mini.Height = (int)(gMapControl.ClientSize.Height - Attitude_Indicator.ClientSize.Width - pad/2);

            MapControl_Rectangle.X      = (int)(Attitude_Indicator.ClientSize.Width + pad);
            MapControl_Rectangle.Y      = (int)pad;
            MapControl_Rectangle.Width  = (int)(gMapControl.ClientSize.Width - Attitude_Indicator.ClientSize.Width - 2 * pad);
            MapControl_Rectangle.Height = (int)(gMapControl.ClientSize.Height - 2 * pad);

            DirectionMarkerPoints.X = (int)gMapControl.FromLatLngToLocal(_GmapDirectionalMarker.Position).X;
            DirectionMarkerPoints.Y = (int)gMapControl.FromLatLngToLocal(_GmapDirectionalMarker.Position).Y;

            if (!MapControl_Rectangle.Contains(DirectionMarkerPoints))
            {
                if (!mission_planning_mode)
                {
                    gMapControl.ZoomAndCenterMarkers("Main Overlay");
                    gMapControl.Zoom = MapZoomLevel;
                }
            }
        }
        #endregion


        #region PLAYBACKS
        private void plotLatLng_button_Click(object sender, EventArgs e)
        {
            List<PointLatLng> points = SettingsCntrl.plotLatLng_button_Clicked(sender, e);

            if (points == null)
                MessageBox.Show("No Valid Points to Map");

            PlaybackOverlay.Clear();

            GMapRoute route = new GMapRoute("Path");
            route.Points.AddRange(points);

            PlaybackOverlay.Routes.Add(route);

            gMapControl.ZoomAndCenterRoute(route);
        }
        #endregion


        #region SERIAL CONNECTION
        private bool device_connected()
        {
            return Mavlink_Protocol.get_sp().IsOpen && !Variables.WAITING_FOR_PARAM_LIST;
        }

        /// <summary>
        ///  comport mouse enter event to refresh the available comports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComPort_ComboBox_MouseEnter(object sender, EventArgs e)
        {
            string[] portnames = SerialPort.GetPortNames();

            Console.WriteLine(portnames.Length);
            ComPort_ComboBox.Items.Clear();
            ComPort_ComboBox.Items.Add("select comport");
            ComPort_ComboBox.Items.AddRange(portnames);
            ComPort_ComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Serial Button Mouse Clicked Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SerialConnection.Connect(SerialPortObject, SerialButton, ComPort_ComboBox, BaudRate_ComboBox))
                {
                    // Initialise background worker
                    SerialOpen_Initialise();
                }

                else if (SerialConnection.DisConnect(SerialPortObject, SerialButton, ComPort_ComboBox, BaudRate_ComboBox))
                {
                    SerialClosed_CleanUp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        /// <summary>
        ///  Serial Initialise -> After Serial Button Click
        /// </summary>
        private void SerialOpen_Initialise()
        {
            if (Parameter_Form == null)
            {
                Parameter_Form = new ParameterForm();    
                Parameter_Form.Initialise_Form();
            }

            if (CustomDataGridCntrl == null)
            {
                CustomDataGridCntrl = new CustomDataGrid();
                CustomDataGridCntrl.Location = new Point(0, Attitude_Indicator.Location.Y + Attitude_Indicator.Size.Height + 2); // nB: if i change Y into automatic size then need to include this control in resize method
                CustomDataGridCntrl.BorderStyle = BorderStyle.FixedSingle;
                CustomDataGridCntrl.Size = new Size(235, (int)(gMapControl.Size.Height * 0.50));
                CustomDataGridCntrl.BorderPanel.Size = new Size(231, (int)(gMapControl.Size.Height * 0.50) - 6);
                this.gMapControl.Controls.Add(CustomDataGridCntrl);
                CustomDataGridCntrl.BringToFront();
            }

            // 
            Variables.WAITING_FOR_PARAM_LIST = true;

            Variables.reset_variables();

            System.Threading.Thread.Sleep(10);

            // initialise background worker and start
            initialise_mainBackgroundWorker();

            start_mainBackgroundWorker();

            // send a message to the ground station to request for parameters
            Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterRequestList(1));

            // intialise System timer everytime we connect and start
            if (System_Timer == null)
            {
                System_Timer = new Timer();
                System_Timer.Interval = 2000;
                System_Timer.Tick += System_Timer_Tick;
                System_Timer.Enabled = false;
                System_Timer.Stop();
            }
            // check for reception of parameters
            Start_TimerEvent(LIST_TIMER_EVENTS.PARAMETER_LIST, 10);
            //Parameters_Cycle_Counter = 30; // the number of cycles the parameter list event will run on the timer
        }

        /// <summary>
        /// Cleans up all neccessary in the event of an unexpected serial close
        /// System_Timer -> Might be running as the serial port is being closed.. stope the timer this will stop all operations with the timer
        /// main background worker -> dispose first calls cancel async and then nullifies the thread
        /// NB: main background worker is supposed to dispose paramform if closed but it may miss this if inner loop is being polled
        /// </summary>
        private void SerialClosed_CleanUp()
        {
            if (System_Timer != null)         // dispose Timer everytime we close SerialPort
            {
                if (System_Timer.Enabled)
                {
                    System_Timer.Enabled = false;
                    System_Timer.Stop();
                }
                System_Timer.Dispose();
                System_Timer = null;
            }

            dispose_mainBackgroundWorker();
        }
        #endregion


        #region READ AND WRITE WAYPOINTS
        bool write_success            = false;
        private void _bwWayPoints_DoWork(object sender, DoWorkEventArgs e)
        {
            if (reading)
            {
                // Send Request List
                Mavlink_Protocol.Send_MissionRequestList();

                DateTime to = DateTime.Now.AddMilliseconds(500);
                while (true)
                {
                    if (DateTime.Now > to)
                    {
                        MessageBox.Show("Timeout on Reading Waypoint "+ Variables.mission_seq);
                        break;
                    }
                    else
                    {
                        if (Variables.mission_seq >= Variables.mission_count)
                        {
                            MessageBox.Show("Successfull Read");
                            e.Result = Variables.RecWP;
                            break;
                        }
                        if (Variables.mission_seq != -1)
                        {
                            // request waypoint
                            Console.WriteLine("Reading WayPoint {0}", Variables.mission_seq);
                            Mavlink_Protocol.Send_MissionRequestItem((Variables.mission_seq));
                            Variables.mission_seq = -1;
                            to = DateTime.Now.AddMilliseconds(300);
                       
                        }
                    }

                    System.Threading.Thread.Sleep(50);
                }
            }
            else if (!reading)
            {
                // Get the Mission Item
                object[,] item = e.Argument as object[,];

                if (data_count > 130)
                {
                    MessageBox.Show("Waypoints exceed limit of the fcs");
                    return;
                }

                // Send WayPoint Count
                Mavlink_Protocol.Send_WayPointCount((int)data_count);

                Console.WriteLine("WayPoint Count : {0} Sent", (int)data_count);

                System.Threading.Thread.Sleep(20);             
                /*
                 * Test Code
                 * Mavlink_Protocol.Send_WayPointItem(item, 1);
                 *
                 */

                // Waiting to Receive WP Request
                // stop sending anything else
                int delay = 1000;
                DateTime _t_now = DateTime.Now.AddMilliseconds(delay);
                bool send_count_again = false;
                while (true)
                {
                    //Console.WriteLine("Running");

                    if (Variables.mission_ack == (byte)MAVLink.MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED)
                    {
                        write_success = true;
                        Variables.ack_message_received = false;
                        MessageBox.Show("Mission Succesfully Uploaded");
                        break;
                    }
                    else if (Variables.mission_ack == (byte)MAVLink.MAV_MISSION_RESULT.MAV_MISSION_NO_SPACE)
                    {
                        write_success = false;
                        Variables.ack_message_received = false;
                        MessageBox.Show("No Space to Store WayPoint");
                        break;
                    }

                    // Too much time has passed, so do counter if on waypoint 0 otherwise declare failed
                    if (DateTime.Now > _t_now)
                    {
                        if (send_count_again == false && Variables.requested_missionitem_seq == 0)
                        {
                            send_count_again = true;
                            Mavlink_Protocol.Send_WayPointCount((int)data_count);
                            Console.WriteLine("WayPoint Count : {0} Sent Again due to timeout", (int)data_count);
                            _t_now = DateTime.Now.AddMilliseconds(300);
                        }
                        else
                        {
                            Console.WriteLine("WayPoint " + Variables.requested_missionitem_seq + " has failed to upload, Please Try again.", "TimeOut");
                            MessageBox.Show("WayPoint "   + Variables.requested_missionitem_seq + " has failed to upload, Please Try again.", "TimeOut");
                            break;
                        }
                    }
                    else
                    {
                        // send the requested mission item
                        if (Variables.request_missionitem_target_system == 255)
                        {
                            Mavlink_Protocol.Send_WayPointItem(item, Variables.requested_missionitem_seq);
                            Variables.request_missionitem_target_system = 0;
                            Console.WriteLine("WayPoint " + Variables.requested_missionitem_seq + " has been sent");
  
                            Variables.WP[Variables.requested_missionitem_seq].lat = (float)item[Variables.requested_missionitem_seq, 0];                    
                            Variables.WP[Variables.requested_missionitem_seq].lng = (float)item[Variables.requested_missionitem_seq, 1];                   
                            Variables.WP[Variables.requested_missionitem_seq].alt = float.Parse(item[Variables.requested_missionitem_seq, 2].ToString());                  
                            Variables.WP[Variables.requested_missionitem_seq].radius = float.Parse(item[Variables.requested_missionitem_seq, 3].ToString());                    
                            Variables.WP[Variables.requested_missionitem_seq].command = Convert.ToUInt16(item[Variables.requested_missionitem_seq, 4]);   
                            Variables.WP[Variables.requested_missionitem_seq].sequence = Convert.ToUInt16(Variables.requested_missionitem_seq);
                            Variables.WP[Variables.requested_missionitem_seq].count = Convert.ToUInt16((int)data_count);
                            _t_now = DateTime.Now.AddMilliseconds(delay);
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
        }


        private void _bwWayPoints_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReadWayPointsButton.Enabled  = true;
            WriteWayPointsButton.Enabled = true;
            Flight_Plan_Button.Enabled   = true;

            if (reading)
            {
                if (Variables.mission_count > 1)
                {

                    update_homePos(new PointLatLng(Variables.RecWP[0,0], Variables.RecWP[0,1]));
                    WayPointOverlay.Markers.Clear();
                    WayPointOverlay.Polygons.Clear();
                    WPCoordinates.Clear();
                    WayPoint_DataGridView.Rows.Clear();
                    WPCoordinates.Add(_HomeMarker.Position);
                    WayPointOverlay.Markers.Add(_HomeMarker);
                    gMapControl.Invalidate();
                    for (int i = 1; i < Variables.mission_count; i++)
                    {
                        GPoint point_lat = gMapControl.FromLatLngToLocal(new PointLatLng((double)Variables.RecWP[i,0], (double)Variables.RecWP[i,1]));
                        MouseEventArgs p = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, (int)point_lat.X, (int)point_lat.Y, 0);
                        gMapControl_MouseClick(gMapControl, p);
                        (WayPoint_DataGridView.Rows[i - 1].Cells["Altitude_Column"] as DataGridViewTextBoxCell).Value = Variables.RecWP[i, 2];
                        (WayPoint_DataGridView.Rows[i - 1].Cells["Radius_Column"] as DataGridViewTextBoxCell).Value   = Variables.RecWP[i, 3];
                        (WayPoint_DataGridView.Rows[i - 1].Cells["Command_Column"] as DataGridViewComboBoxCell).Value = WPCommandIndex((byte)Variables.RecWP[i, 4]);
                        //(WayPoint_DataGridView.Rows[i - 1].Cells["Distance_Column"] as DataGridViewTextBoxCell).Value = Common.common.distance(WPCoordinates.ToArray()[i-1], WPCoordinates.ToArray()[i], this.gMapControl);
                    }


                    for (int i = 0; i < Variables.mission_count; i++)
                    {
                        Variables.WP[i].lat         = Variables.RecWP[i, 0];                    // = (datarows[i - 1].Cells[1] as DataGridViewTextBoxCell).Value; // Lat
                        Variables.WP[i].lng         = Variables.RecWP[i, 1];                    // = (datarows[i - 1].Cells[2] as DataGridViewTextBoxCell).Value; // Lon
                        Variables.WP[i].alt         = Variables.RecWP[i, 2];                    // = (datarows[i - 1].Cells[3] as DataGridViewTextBoxCell).Value; // Alt
                        Variables.WP[i].radius      = Variables.RecWP[i, 3];                    // = (datarows[i - 1].Cells[4] as DataGridViewTextBoxCell).Value;  // Rad
                        Variables.WP[i].command     = Convert.ToUInt16(Variables.RecWP[i, 4]);  // = WPCommand((datarows[i - 1].Cells[5] as DataGridViewComboBoxCell).Value); // Command
                        Variables.WP[i].sequence    = Convert.ToUInt16(i);
                        Variables.WP[i].count       = Convert.ToUInt16(Variables.mission_count);
                    }
                }
            }

            reading = false;

            // Reading Variables
            Variables.mission_count = 0;
            Variables.mission_seq = -1;

            // Writing Variables
            Variables.request_missionitem_target_system = 0;
            Variables.requested_missionitem_seq         = 0;
            Variables.mission_ack                       = (byte)MAVLink.MAV_MISSION_RESULT.MAV_MISSION_DENIED;
            Variables.ack_message_received              = false;
        }

        bool reading = false;
        private void ReadWayPointsButton_Click(object sender, EventArgs e)
        {
            ReadWayPointsButton.Enabled  = false;
            WriteWayPointsButton.Enabled = false;
            Flight_Plan_Button.Enabled   = false;
            reading = true;

            if (!_bwWayPoints.IsBusy)
                _bwWayPoints.RunWorkerAsync();
        }

        int data_count = 0;
        private void WriteWayPointsButton_Click(object sender, EventArgs e)
        {
            ReadWayPointsButton.Enabled  = false;
            WriteWayPointsButton.Enabled = false;
            Flight_Plan_Button.Enabled   = false;
            reading = false;

            // send a waypoint count message
            int row_count = WayPoint_DataGridView.Rows.Count;
            data_count = WPCoordinates.Count;

            // Includes WayPoint 0
            // Last Index is for DataCount and Write or Read
            if (row_count == data_count)
            {
                object[,] WP_Item = new object[row_count, 5];

                // Home WP
                WP_Item[0, 0] = (float)WPCoordinates.ElementAt(0).Lat; // Lat
                WP_Item[0, 1] = (float)WPCoordinates.ElementAt(0).Lng; // Lon
                WP_Item[0, 2] = (float)100;                            // Alt
                WP_Item[0, 3] = (float)RadiusNumericUpDown.Value;      // Rad
                WP_Item[0, 4] = (ushort)(0);                           // Command

                DataGridViewRowCollection datarows = (WayPoint_DataGridView.Rows);

                for (int i = 1; i < row_count; i++)
                {
                    WP_Item[i, 0] = (datarows[i-1].Cells[1] as DataGridViewTextBoxCell).Value; // Lat
                    WP_Item[i, 1] = (datarows[i-1].Cells[2] as DataGridViewTextBoxCell).Value; // Lon
                    WP_Item[i, 2] = (datarows[i-1].Cells[3] as DataGridViewTextBoxCell).Value; // Alt
                    WP_Item[i, 3] = (datarows[i-1].Cells[4] as DataGridViewTextBoxCell).Value;  // Rad
                    WP_Item[i, 4] = WPCommand((datarows[i - 1].Cells[5] as DataGridViewComboBoxCell).Value); // Command
                }

                
               if(!_bwWayPoints.IsBusy)
                   _bwWayPoints.RunWorkerAsync(WP_Item);
            }
        }

        /// <summary>
        ///  Receiving end can only receive a uint8_t CMD
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        private ushort WPCommand(object _cmd)
        {
            string cmd = _cmd as string;
            if (cmd == "WAYPOINT")
            {
                return (ushort)MAVLink.MAV_CMD.WAYPOINT;
            }
            else if (cmd == "TAKEOFF")
            {
                return (ushort)MAVLink.MAV_CMD.TAKEOFF;
            }
            else if (cmd == "LAND")
            {
                return (ushort)MAVLink.MAV_CMD.LAND;
            }
            else if (cmd == "RTL")
            {
                return (ushort)MAVLink.MAV_CMD.RETURN_TO_LAUNCH;
            }
            else
                return (ushort)MAVLink.MAV_CMD.WAYPOINT;
        }


        private string WPCommandIndex(byte cmd)
        {
            switch (cmd)
            {
                case (byte)MAVLink.MAV_CMD.TAKEOFF:
                    return "TAKEOFF";

                case (byte)MAVLink.MAV_CMD.WAYPOINT:
                    return "WAYPOINT";

                case (byte)MAVLink.MAV_CMD.RETURN_TO_LAUNCH:
                    return "RTL";

                case (byte)MAVLink.MAV_CMD.LAND:
                    return "LAND";

                default:
                    return "WAYPOINT";
            }
        }
        #endregion


        #region WAYPOINTS DATAGRIDVIEW REGION
        /// <summary>
        ///  occurs when a cell has been clicked Z REMOVE WAYPOINTS THROUGH THIS METHOD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>      
        bool allow_row_delete = false;
        private void WayPoint_DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rows = WayPoint_DataGridView.Rows.Count - 1;

            if (rows > 0)
            {
                WayPoint_DataGridView.Rows[rows].ReadOnly = true;
                for (int iz = 0; iz < rows; iz++)
                {
                    WayPoint_DataGridView.Rows[iz].ReadOnly = false;
                }
            }
            else
            {
                WayPoint_DataGridView.Rows[rows].ReadOnly = true;
            }
            if (e.ColumnIndex == WayPoint_DataGridView.Columns["Delete_Column"].Index && e.RowIndex >= 0 && WayPoint_DataGridView.Rows.Count > 1 && e.RowIndex < WayPoint_DataGridView.Rows.Count - 1)
            {
                DataGridViewRow remove_row = WayPoint_DataGridView.Rows[e.RowIndex];
                allow_row_delete = true;
                WayPoint_DataGridView.Rows.Remove(remove_row);
            }
        }


        private void WayPoint_DataGridView_CellEndEdit(object sender, EventArgs e)
        {

            int rowindex = WayPoint_DataGridView.CurrentCell.RowIndex;

            if (WayPoint_DataGridView.Columns["Altitude_Column"].Index == WayPoint_DataGridView.CurrentCell.ColumnIndex)
            {
                if ((WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value == null) return;
                int altitude;
                bool _parsed = int.TryParse((WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value.ToString(), out altitude);
                if (!_parsed)
                {
                    MessageBox.Show("Please Enter a Valid Altitude Numeric"+"\n"+"Otherwise a default altitude of 100 metres will be applied");
                    (WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value = 100;
                }
            }
            else if (WayPoint_DataGridView.Columns["Radius_Column"].Index == WayPoint_DataGridView.CurrentCell.ColumnIndex)
            {
                if ((WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value == null) return;
                int radius;
                bool _parsed = int.TryParse((WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value.ToString(), out radius);
                if (!_parsed)
                {
                    MessageBox.Show("Please Enter a Valid Radius Numeric" + "\n" + "Otherwise a default radius of 20 metres will be applied");
                    (WayPoint_DataGridView.CurrentCell as DataGridViewTextBoxCell).Value = 20;
                }
                else
                {
                    WPRadius_Update(rowindex+1, radius);
                }
            }

            for (int i = 0; i < WayPoint_DataGridView.Rows.Count - 1; i++)
            {
                float alt1 = 0, alt2 = 0;
                try
                {
                    (WayPoint_DataGridView.Rows[i].Cells[7] as DataGridViewTextBoxCell).Value = Common.common.distance(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], this.gMapControl);
                    if (i > 0)
                    {
                        alt1 = float.Parse((WayPoint_DataGridView.Rows[i - 1].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                    else
                    {
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                }
                catch(SystemException ex)
                {
                    MessageBox.Show("Alert", ex.Message);
                }

                (WayPoint_DataGridView.Rows[i].Cells[8] as DataGridViewTextBoxCell).Value = Common.common.gradient(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], alt1, alt2, this.gMapControl);
            }
        }


        /// <summary>
        ///  Sort the Id Column
        /// </summary>
        /// <param name="sender"></param>
        private void WayPointDataGrid_SortId(object sender)
        {
            for (int index = 0; index < (sender as DataGridView).Rows.Count; index++)
            {
                (WayPoint_DataGridView.Rows[index].Cells["Id_Column"] as DataGridViewTextBoxCell).Value = index + 1;
            }
        }


        /// <summary>
        ///  insert values into the daragridview
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <param name="rad">radius</param>
        /// <param name="alt">altitude</param>
        bool allow_row_add = false;
        private void WayPointDataGrid_Insert(float lat, float lon, float rad, float alt)
        {
            int index = WayPoint_DataGridView.Rows.Count - 1; // current index
            DataGridViewRow datarow = (DataGridViewRow)(WayPoint_DataGridView.Rows[WayPoint_DataGridView.Rows.Count-1].Clone());
            (datarow.Cells[1] as DataGridViewTextBoxCell).Value = lat;
            (datarow.Cells[2] as DataGridViewTextBoxCell).Value = lon;
            (datarow.Cells[3] as DataGridViewTextBoxCell).Value = alt;
            (datarow.Cells[4] as DataGridViewTextBoxCell).Value = rad;
            allow_row_add = true;
            WayPoint_DataGridView.Rows.Add(datarow);         
        }


        /// <summary>
        /// update records on datagridview.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="_marker"></param>
        private void WayPointDataGrid_UpdateRecord(int index, GMapWayPointMarker _marker)
        {
            DataGridViewRow datarow                             = WayPoint_DataGridView.Rows[index];
            (datarow.Cells[1] as DataGridViewTextBoxCell).Value = (float)_marker.Position.Lat;
            (datarow.Cells[2] as DataGridViewTextBoxCell).Value = (float)_marker.Position.Lng;

            for (int i = 0; i < WayPoint_DataGridView.Rows.Count - 1; i++)
            {
                (WayPoint_DataGridView.Rows[i].Cells[7] as DataGridViewTextBoxCell).Value = Common.common.distance(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], this.gMapControl);
                float alt1 = 0, alt2 = 0;
                if (i > 0)
                {
                    alt1 = float.Parse((WayPoint_DataGridView.Rows[i - 1].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                }
                else
                {
                    alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                }
                (WayPoint_DataGridView.Rows[i].Cells[8] as DataGridViewTextBoxCell).Value = Common.common.gradient(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], alt1, alt2, this.gMapControl);
            }
        }


        /// <summary>
        ///  occurs when a row has been added from user clicking on map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WayPoint_DataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (allow_row_add)
            {
                WayPointDataGrid_SortId(sender);
                (WayPoint_DataGridView.Rows[e.RowIndex].Cells["Command_Column"] as DataGridViewComboBoxCell).Value = (WayPoint_DataGridView.Rows[0].Cells["Command_Column"] as DataGridViewComboBoxCell).Items[0];
                (WayPoint_DataGridView.Rows[e.RowIndex].Cells["Delete_Column"] as DataGridViewButtonCell).Value    = "X";

                for (int i = 0; i < WayPoint_DataGridView.Rows.Count - 1; i++)
                {
                    (WayPoint_DataGridView.Rows[i].Cells[7] as DataGridViewTextBoxCell).Value = Common.common.distance(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], this.gMapControl);
                    float alt1 = 0, alt2 = 0;
                    if (i > 0)
                    {
                        alt1 = float.Parse((WayPoint_DataGridView.Rows[i - 1].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                    else
                    {
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                    (WayPoint_DataGridView.Rows[i].Cells[8] as DataGridViewTextBoxCell).Value = Common.common.gradient(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], alt1, alt2, this.gMapControl);
                }
                allow_row_add = false;
            }
        }


        /// <summary>
        ///  occurs when a rows has been removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WayPoint_DataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (allow_row_delete)
            {
                WayPointDataGrid_SortId(sender);
                int Id_removed = int.Parse((WayPoint_DataGridView.Rows[e.RowIndex].Cells["Id_Column"] as DataGridViewTextBoxCell).Value.ToString());
                Marker_Removed(Id_removed);

                for (int i = 0; i < WayPoint_DataGridView.Rows.Count - 1; i++)
                {
                    (WayPoint_DataGridView.Rows[i].Cells[7] as DataGridViewTextBoxCell).Value = Common.common.distance(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], this.gMapControl);
                    float alt1 = 0, alt2 = 0;
                    if (i > 0)
                    {
                        alt1 = float.Parse((WayPoint_DataGridView.Rows[i - 1].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                    else
                    {
                        alt2 = float.Parse((WayPoint_DataGridView.Rows[i].Cells[3] as DataGridViewTextBoxCell).Value.ToString());
                    }
                    (WayPoint_DataGridView.Rows[i].Cells[8] as DataGridViewTextBoxCell).Value = Common.common.gradient(WPCoordinates.ToArray()[i], WPCoordinates.ToArray()[i + 1], alt1, alt2, this.gMapControl);
                }
                allow_row_delete = false;
            }
        }
         

        /// <summary>
        /// controls hiding and showing of the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waypoint_grid_button_Click(object sender, EventArgs e)
        {
            int size = 150;

            Form Dataform;
            Dataform = new Form();
            if (size_grip_button == 1)
            {              
                Main_Form.Show();
                Main_Form.TopMost = true;
                Main_Form.Size = new Size(this.waypoint_grid_button.Size.Width + 20, size * 2 * size_grip_button + 100);
                this.WayPoint_DataGridView.Location = new Point(0, 0);
                this.WayPoint_DataGridView.Size     = new Size(this.waypoint_grid_button.Size.Width, size* 2 * size_grip_button);


                //this.waypoint_grid_button.Size = new System.Drawing.Size(grid_button_width, 25);
                //this.waypoint_grid_button.Location = new Point(gMapControl.ClientSize.Width - waypoint_grid_button.ClientSize.Width, size_grip_button * size);
                //this.waypoint_grid_button.BackgroundImage = Properties.Resources.button_up_40x23_;
                //this.WayPoint_DataGridView.Location = new Point(this.waypoint_grid_button.Location.X, 0);
                //this.WayPoint_DataGridView.Size = new Size(this.waypoint_grid_button.Size.Width, size * size_grip_button);
                size_grip_button = 0;
            }
            else
            {
                Main_Form.Hide();
                //this.waypoint_grid_button.Size            = new System.Drawing.Size(grid_button_width, 25);
                //this.waypoint_grid_button.Location        = new Point(gMapControl.ClientSize.Width - waypoint_grid_button.ClientSize.Width, size_grip_button * size);
                //this.waypoint_grid_button.BackgroundImage = Properties.Resources.button_down_40x23_;
                //this.WayPoint_DataGridView.Location = this.waypoint_grid_button.Location;
                //this.WayPoint_DataGridView.Size     = this.waypoint_grid_button.Size;
                size_grip_button = 1;
            }
        }
        #endregion
   

        #region HARDWARE IN LOOP
        private void startXplaneButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofldg = new OpenFileDialog();
            if (ofldg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = ofldg.FileName;
                if (!string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show(filename);

                    // Application.Run()
                    Process xplane = new Process();
                    xplane.StartInfo.FileName = filename;
                    xplane.Start();
                }

            }
        }


        private void udpconnectButton_Click(object sender, EventArgs e)
        {
            if (SettingsCntrl.udpconnectButton.Text == "udp connect")
            {
                int recPort;
                int sendPort;
                string simIp = SettingsCntrl.SimIpTextBox.Text;
                string _recPort = SettingsCntrl.recPortTextBox.Text; // should be matched with the sendingSimulator Port
                string _sndPort = SettingsCntrl.sndPortTextBox.Text; // should be matched with the receivingSimulator Port              

                if (string.IsNullOrEmpty(simIp) || string.IsNullOrEmpty(_recPort) || string.IsNullOrEmpty(_sndPort))
                {
                    MessageBox.Show("Empty string field detected", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!simIp.Contains("127") && !simIp.Contains("192"))
                {
                    MessageBox.Show("Not a valid simIp address", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!int.TryParse(_recPort, out recPort))
                {
                    MessageBox.Show("Failed to change recPort String to correct int value", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!int.TryParse(_sndPort, out sendPort))
                {
                    MessageBox.Show("Failed to change sndPort String to correct int value", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                try
                {
                    xplane10.connect(simIp, recPort, sendPort);

                    // if all goes well save settings
                    XplaneHil.Default.simIP = simIp;
                    XplaneHil.Default.sndPort = _sndPort;
                    XplaneHil.Default.recPort = _recPort;
                    XplaneHil.Default.Save();

                    SettingsCntrl.udpconnectButton.Text = "udp disconnect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                try
                {
                    xplane10.disconnect();

                    SettingsCntrl.udpconnectButton.Text = "udp connect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion


        #region VFR DISPLAY ON AI
        string old_mode;
        string gps_error;
        string baro_error;
        string ins_errro;
        int count = 0;
        float actual_volts;
        float incoming_voltage;
        float gain = 1;

        private void BatteryVoltageTextBox_TextChanged(object sender, EventArgs e)
        {
            float incoming_voltage = Variables.batteryVolts;
            float volts_entered    = 0;

            if (float.TryParse(SettingsCntrl.BatteryVoltageTextBox.Text, out volts_entered))
            {
                 gain = volts_entered / incoming_voltage;     
            }

            SettingsCntrl.BatteryGainTextBox.Text = gain.ToString();
        }

        private void Display_Vfr()
        {
            AirspeedLabel.Text = "AS:" +Variables.airspeed.ToString("0") + "m/s";
            ThrottleLabel.Text = Variables.throttle.ToString();
            //ThrottleLabel.Text = Variables.airspeed_est.ToString("0.0");

            HeadingLabel.Text = Variables.yawDeg.ToString("0");
            float actual_volts = 12;


            if ((SettingsCntrl != null) && device_connected())
            {
                incoming_voltage = Variables.batteryVolts;

                SettingsCntrl.BatteryAnalogTextBox.Text     = incoming_voltage.ToString();

                if (count < 5 )
                {
                    count++;
                    SettingsCntrl.BatteryVoltageTextBox.Text = incoming_voltage.ToString();         
                }
            }

            ComportStatuslabel.Text = Mavlink_Protocol.bps.ToString() + " Bps";

            BatteryLabel.Text = "BV:" + (incoming_voltage * gain).ToString("0.00") + " V";

            GroundSpeedLablel.Text = "GS:" + Variables.vfr_groundspeed.ToString("0.0");

            ModeLabel.Text   = Variables.get_FLIGHTMODEString;

            GPSLabel.Text    = Variables.fix_status;

            SATSLabel.Text   = Variables.numSatellites.ToString("0") + " sats";

            SignalLabel.Text = "S:"+Mavlink_Protocol.linkquality.ToString("0") + "%";


            if (gps_error != Variables.GPS_ERROR())
            {
                //StatusLabel.Text = Variables.GPS_ERROR();
                //gps_error = Variables.GPS_ERROR();
            }

            if(ins_errro != Variables.INS_ERROR())
            {
                StatusLabel.Text = Variables.INS_ERROR();
                ins_errro = Variables.INS_ERROR();
                Speech.SpeakAsync(StatusLabel.Text);
            }


            if (old_mode != ModeLabel.Text)
            {
                Speech.SpeakAsync("Mode Changed to " + ModeLabel.Text);
            }


            old_mode = ModeLabel.Text;

        }
        #endregion


        # region PARAMETERS REGION
        private void Parameter_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfld   = new OpenFileDialog();
            openfld.InitialDirectory = Environment.CurrentDirectory;

            if (openfld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = openfld.FileName;

                if (file.EndsWith(".txt") || file.EndsWith(".dat"))
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(openfld.OpenFile()))
                    {
                        Dictionary<string, string> paramdictionary = new Dictionary<string, string>();
                        string[,] parameter = new string[100, 2];
                        string data         = "";
                        while ((data        = sr.ReadLine()) != null)
                        {
                            paramdictionary.Add(data.Split('\t')[0], data.Split('\t')[2]);
                        }     
                       
                        // put this parameters in customDatagrid
                        if (CustomDataGridCntrl == null)
                        return;

                        foreach (string key in paramdictionary.Keys)
                        {
                            if (!CustomDataGridCntrl.paramter_dictionary.ContainsKey(key))
                                return;

                            decimal _value = (CustomDataGridCntrl.paramter_dictionary[key]).Value;
                            string value   = paramdictionary[key];
                            decimal _value_= _value;

                            if (decimal.TryParse(value, out _value_) == true)
                            {
                                if (_value != _value_)
                                {
                                    (CustomDataGridCntrl.paramter_dictionary[key] as NumericUpDown).Value = _value_;
                                }
                            }                           
                        }

                    }
                }
            }
        }
        

        // upload paramters
        int PARAM_INDEX_COUNTER = 0;
        bool IGNORE_PARAMETERS  = false;
        private void UploadParameter_Status(string param_id)
        {

            if (CustomDataGridCntrl != null)
            {
                int len = CustomDataGridCntrl.changed_indecies_list.Count;
                StatusProgressBar.Minimum = 0;
                StatusProgressBar.Maximum = len;
                StatusProgressBar.Step = 1;
                StatusProgressBar.Value = PARAM_INDEX_COUNTER;

                StatusLabel.Text = "Sent: " + param_id;
            }
        }


        private void Upload_Parameter()
        {
            int mask_index = PARAM_INDEX_COUNTER; // get the mask index

            if (CustomDataGridCntrl != null)
            {
                List<int> changed_indieces            = CustomDataGridCntrl.changed_indecies_list;
                Dictionary<string, NumericUpDown> dic = CustomDataGridCntrl.paramter_dictionary;
                int len                               = changed_indieces.Count;

                // we have valid changed parameters
                if (len > mask_index)
                {
                    int actual_index = changed_indieces[mask_index];

                    Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterSet(dic, changed_indieces, mask_index));

                    PARAM_INDEX_COUNTER++;          // only increment if we have safely sent parameter
                    UploadButton.Enabled = false;   // automatically disables uploadbutton

                    Start_TimerEvent(LIST_TIMER_EVENTS.PARAMETER_RECEIVED, 7); // upload parameter automatically switches on timer
                }
                else if (len == mask_index)
                {
                    PARAM_INDEX_COUNTER = 0;         // reset once we have received acknowledgement of all parameter
                    UploadButton.Enabled = true;     // enable the upload button if we have received acknowledgement of receipt of all paramters.
                    CustomDataGridCntrl.Clear_Indeces();

                    Stop_TimerEvent(LIST_TIMER_EVENTS.PARAMETER_RECEIVED, 7);
                }
            }
        }

        #endregion


        #region CONTROL EVENTS
        /// <summary>
        ///  Paint event for the attitude indicator --> invoked to draw the attitude indicator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Attitude_Indicator_Paint(object sender, PaintEventArgs e)
        {        
            Graphics gfx = e.Graphics;
            float roll, pitch, yaw;
            roll = Variables.rollDeg;
            pitch = Variables.pitchDeg;
            yaw = Variables.yawDeg;

            // Work around for 2015 scaling issues
#if VS2015
            GeneralMethods.RotateAndTranslateUpdated(e, AI_background, -roll, 0, AI_imagelocation, (double)(6.25 * pitch), AI_rotationPoint, 1);
            gfx.DrawImageUnscaledAndClipped(AI_Wing, new Rectangle(AI_WingPoint.X, AI_WingPoint.Y, AI_Wing.Size.Width,AI_Wing.Size.Height));
#else
            GeneralMethods.RotateAndTranslate(e, AI_background, -roll, 0, AI_imagelocation, (double)(6.25 * pitch), AI_rotationPoint, 1);
            gfx.DrawImage(AI_Wing, AI_WingPoint);
#endif

        }
        #endregion


        #region SYSTEM TIMER REGION
        /// <summary>
        /// Gloabl Timer Event,  manually invoked while Timer is running so that the timer can switch to processing a different event
        /// </summary>
        byte SYSTEM_TIMER_DO_EVENT = 0;


        /// <summary>
        ///  list of system timer events
        /// </summary>
        public enum LIST_TIMER_EVENTS
        {
            NOTHING                 = 0,
            PARAMETER_LIST          = 1,
            SERIAL_PORT_CHECKING,
            PARAMETER_RECEIVED,
        }


        /// <summary>
        /// Structure that takes event name and event cycle
        /// </summary>
        public struct SYSTEM_TIMER_EVENT
        {
            public LIST_TIMER_EVENTS event_name;
            public int event_cycles;
            public bool completed;
        };

        public List<SYSTEM_TIMER_EVENT> TASKS  = new List<SYSTEM_TIMER_EVENT>();
      

        /// <summary>
        ///  Timer Cycle Counters
        /// </summary>
        private int Parameters_Cycle_Counter = 30;


        /// <summary>
        ///  Start Timer Event
        /// </summary>
        /// <param name="rd"></param>
        private void Start_TimerEvent(LIST_TIMER_EVENTS rd, int cycle_counter)
        {
            this.System_Timer.Enabled = true;
            this.System_Timer.Start();


            SYSTEM_TIMER_EVENT EVENT;
            EVENT.event_name    = rd;
            EVENT.event_cycles  = cycle_counter;
            EVENT.completed     = false;


            if (!TASKS.Contains(EVENT))
            {
                TASKS.Add(EVENT);
            }


            CYCLE_TICKS = 0;
        }


        /// <summary>
        ///  Stop Timer Event
        /// </summary>
        /// <param name="rd"></param>
        private void Stop_TimerEvent(LIST_TIMER_EVENTS rd, int cycle_counter)
        {
            SYSTEM_TIMER_EVENT EVENT;
            EVENT.event_name    = rd;
            EVENT.event_cycles  = cycle_counter;
            EVENT.completed     = false;
    
            if (TASKS.Contains(EVENT))
            {
                TASKS.Remove(EVENT);
            }
        }


#if OLD
        /// <summary>
        ///  Instead of manually invoking SYSTEM_TIMER_DO_EVENT use this method for invocation
        /// </summary>
        /// <param name="rd"></param>
        private void set_SystemTimerEvent(LIST_TIMER_EVENTS rd)
        {
            SYSTEM_TIMER_DO_EVENT = (byte)rd;

            switch (rd)
            {
                case LIST_TIMER_EVENTS.PARAMETER_LIST:
                    Parameters_Cycle_Counter = 30;
                    break;

                case LIST_TIMER_EVENTS.PARAMETER_RECEIVED:
                    PARAM_INDEX_CYCLE_COUNTER = 7;
                    break;
            }
        }
#endif


        /// <summary>
        ///  TimerTick Event Processing Different Events Set by set_SystemTimerEvent(LIST_TIMER_EVENTS rd)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int CYCLE_TICKS = 0;
        private void System_Timer_Tick(object sender, EventArgs e)
        {
            if (TASKS.Count <= 0)
                return;

            CYCLE_TICKS++;

            for (int i = TASKS.Count-1; i >= 0; i--)
            {               
                switch(TASKS[i].event_name)
                {
                    case LIST_TIMER_EVENTS.NOTHING:
                        break;


                    case LIST_TIMER_EVENTS.SERIAL_PORT_CHECKING:
                        // check serial port
                        break;


                    case LIST_TIMER_EVENTS.PARAMETER_LIST:
                        // check parameter list
                        switch (CYCLE_TICKS)
                        {
                            case 10:
                                Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterRequestList(1));
                                Console.WriteLine("Second Request Parameter Attempted ");
                                break;

                            case 20:
                                Console.WriteLine("Parameter Request List Timeout");
                                MessageBox.Show("Parameter Request List Timeout", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                TASKS.RemoveAt(i);
                                SerialButton_Click(sender, e);
                                break;
                        }
                        break;


                    case LIST_TIMER_EVENTS.PARAMETER_RECEIVED:
                        // havent received acknoweldged parameter reception
                        switch (CYCLE_TICKS)
                        {
                            case 3:
                                List<int> changed_indieces              = CustomDataGridCntrl.changed_indecies_list;
                                Dictionary<string, NumericUpDown> dic   = CustomDataGridCntrl.paramter_dictionary;
                                Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterSet(dic, changed_indieces, (PARAM_INDEX_COUNTER - 1)));
                                break;


                            case 6:
                                // Parameter can still be received after this time if we dont have something in background worker 
                                // This parameter has failed
                                // If we decide to send paramters again, previous changed indieces have not been cleared, so the process will start again.
                                PARAM_INDEX_COUNTER  = 0; // start from the beginning on the next send
                                UploadButton.Enabled = true;
                                IGNORE_PARAMETERS    = true; // this makes sure that even if we receive parameters past this time in background worker, they wont be processed i.e. no parameter will be sent.   
                                StatusLabel.Text     = "Upload Fail";
                                MessageBox.Show("Sending of Parameters has failed or too much time lapsed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                TASKS.RemoveAt(i);
                                break;
                        }
                        break;
                }
            }


#if OLD
            switch (SYSTEM_TIMER_DO_EVENT)
            {
                case (byte)LIST_TIMER_EVENTS.NOTHING:

                    break;

                case (byte)LIST_TIMER_EVENTS.PARAMETER_LIST:
                    Parameters_Cycle_Counter--;
                    switch (Parameters_Cycle_Counter)
                    {
                        case 20:
                            Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterRequestList(1));
                            Console.WriteLine("Second Request Parameter Attempted " + Parameters_Cycle_Counter);
                            break;

                        case 10:
                            Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterRequestList(1));
                            Console.WriteLine("Third Request Parameter Attempted " + Parameters_Cycle_Counter);
                            break;

                        case 0:
                            Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterRequestList(1));
                            Console.WriteLine("Fourth Request Parameter Attempted " + Parameters_Cycle_Counter);
                            break;

                        case -1:
                            SerialButton_Click(sender, e); // since serial port is open this will close down serial port and perform other closes
                            Console.WriteLine("Failed to Receive any paramaters-> SerialPort will be closed");
                            MessageBox.Show("Failed to Receive any paramaters", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }
                    break;


                case (byte)LIST_TIMER_EVENTS.PARAMETER_RECEIVED:
                    PARAM_INDEX_CYCLE_COUNTER--;    // 4 seconds interval bet

                    switch (PARAM_INDEX_CYCLE_COUNTER)
                    {
                        case 4:
                            List<int> changed_indieces = CustomDataGridCntrl.changed_indecies_list;
                            Dictionary<string, NumericUpDown> dic = CustomDataGridCntrl.paramter_dictionary;
                            Mavlink_Protocol.sendPacket(MavlinkExecution.ParameterSet(dic, changed_indieces, (PARAM_INDEX_COUNTER - 1)));
                            break;


                        case 2:
                            // Parameter can still be received after this time if we dont have something in background worker 
                            // This parameter has failed
                            // If we decide to send paramters again, previous changed indieces have not been cleared, so the process will start again.
                            PARAM_INDEX_CYCLE_COUNTER = 7;
                            PARAM_INDEX_COUNTER = 0; // start from the beginning on the next send
                            UploadButton.Enabled = true;
                            IGNORE_PARAMETERS = true; // this makes sure that even if we receive parameters past this time in background worker, they wont be processed i.e. no parameter will be sent.
                            set_SystemTimerEvent(LIST_TIMER_EVENTS.NOTHING);

                            StatusLabel.Text = "Upload Fail";
                            MessageBox.Show("Sending of Parameters has failed or too much time lapsed", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                    }

                    break;
            }
#endif


        }


#endregion


#region MAIN VIEW CLOSING
        /// <summary>
        ///  Main view closing events : dispose all objects here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SerialPortObject != null)
            {
                if (SerialPortObject.IsOpen)
                {
                    try
                    {
                        SerialClosed_CleanUp();
                        SerialPortObject.Close();
                    }
                    catch (Exception ex)
                    {
                        // do nothing
                    }
                }
                SerialPortObject.Dispose();
            }

            if (gMapControl != null)
            {
                gMapControl.MouseClick -= gMapControl_MouseClick;
                gMapControl.MouseDown  -= gMapControl_MouseDown;
                gMapControl.MouseMove  -= gMapControl_MouseMove;
                gMapControl.MouseUp   -= gMapControl_MouseUp;
                gMapControl.MouseHover -= gMapControl_MouseHover;
                gMapControl.OnMarkerClick -= gMapControl_OnMarkerClick;
                gMapControl.Manager.CancelTileCaching();
                gMapControl.Dispose();
            }

            if (SettingsCntrl != null)
            {
                SettingsCntrl.Parameter_button.Click -= Parameter_button_Click;
                SettingsCntrl.plotLatLng_button.Click -= plotLatLng_button_Click;
                SettingsCntrl.Dispose();
                SettingsCntrl = null;
            }

            if (DoUI_Timer != null)
            {
                this.DoUI_Timer.Stop();
                this.DoUI_Timer.Tick -= DoUI_Timer_Tick;
                this.DoUI_Timer.Dispose();

            }

            if (Speech != null)
            {
                this.Speech.Dispose();
                Speech = null;
            }
        }
#endregion

    }


#region HELPER CLASS
    public class Helper
    {
        public Helper()
        {
            rmax1 = 0;
            min = 0;
            max = 0;
            rmin1 = 100000;
        }

        private float rmax1;
        public double max;
        private float rmin1;
        public double min;

        public float sign(double pt)
        {
            if (pt >= 0)
                return 1.0f;
            else
                return -1.0f;
        }

        public void Record(float dis, double pt)
        {
            if (sign(pt) == 1.0f)
            {
                RecordMax(dis, pt);
            }
            else
            {
                RecordMin(-dis, pt);
            }
        }

        public void RecordMax(float r1, double r2)
        {
            float new_record1 = Math.Max(rmax1, r1);

            if (new_record1 != rmax1)
            {
                max = r2;
                rmax1 = new_record1;
            }
        }


        public void RecordMin(float r1, double r2)
        {
            float new_record1 = Math.Min(rmin1, r1);

            if (new_record1 != rmin1)
            {
                min = r2;
                rmin1 = new_record1;
            }
        }
    }
#endregion


}
