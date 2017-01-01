using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

namespace UGCS3
{
    public abstract class Global : Form
    {
        public Bitmap   AI_background;
        public Bitmap   AI_Wing;
        public Point    AI_imageSize;

        public Label    AirspeedLabel;
        public Label    ThrottleLabel;
        public Label    HeadingLabel;
        public Label    BatteryLabel;
        public Label    ModeLabel;
        public Label    GPSLabel;
        public Label    GroundSpeedLablel;
        public Label    SATSLabel;
        public Label    SignalLabel;
        public ComboBox MapBox;
    }
}
