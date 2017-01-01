using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMap;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UGCS3.Map
{
    public class GMapDirectionMarker:GMapMarker
    {
        private float yaw = 0, roll = 0;
        Bitmap _bitmap;
        Pen myPen = new Pen(Brushes.Pink, 3);
        Pen headingPen = new Pen(Brushes.Black, 2);
        Pen cogPen = new Pen(Brushes.Green, 2);
        Pen nextWPPen = new Pen(Brushes.Yellow, 2);
        

        public GMapDirectionMarker(PointLatLng pt, float _yaw, float _roll, Bitmap bmp):base(pt)
        {
          
            this._bitmap = bmp;
            this.Size = new Size(_bitmap.Width, _bitmap.Height);
            this.Offset = new Point(-Size.Width / 2, -Size.Height / 2);
            this.yaw = _yaw;
            this.roll = _roll;
        }

        public float set_roll
        {
            set { this.roll = value; }
        }

        public float set_yaw
        {
            set { this.yaw = value; }
        }

        float cog = 0;
        public float set_cog
        {
            set
            {
                cog = value;
            }
        }

        public override void OnRender(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsState savedStated = g.Save(); 
            g.RotateTransform(Convert.ToSingle(yaw)); // changed from convert to single
            g.TranslateTransform(LocalPosition.X + Size.Width / 2, LocalPosition.Y + Size.Height / 2, System.Drawing.Drawing2D.MatrixOrder.Append);
            g.DrawImage(_bitmap, -Size.Width / 2, -Size.Height / 2, Size.Width, Size.Height);
            float radius = 20f;

#if !SHOW_PREDICTIVE_PLANE_MOVEMENT
            #region RIGHT BANK HERE
            if (roll > 5)
            {
                //g.DrawRectangle(myPen, 0, -Size.Height / 2 - radius, 2 * radius, 2 * radius);
                try
                {
                    g.DrawArc(myPen, 0, -Size.Height / 2 - radius, 2 * radius, 2 * radius, 180, 2 * roll);
                }
                catch
                {
                }
            }
            #endregion

            # region LEFT BANK HERE
            if (roll < -5)
            {
                //g.DrawRectangle(myPen, -2 * radius, -Size.Height / 2 - radius, 2 * radius, 2 * radius);   
                try
                {
                    g.DrawArc(myPen, -2 * radius, -Size.Height / 2 - radius, 2 * radius, 2 * radius, 0, 2 * roll);
                }
                catch
                {
                    // System.Diagnostics.Debug.WriteLine("GMapDirectionalMarker Out of Memory Exception =>" + ex.Message);
                }
            }

            #endregion
#endif

            #region HEADING AND COURSE
#if !HEADING

            Point start   = new Point(0, 0); // (0,0) is the center of the drawm bitmap after the coordinate has been moved here 
            int magnitude = 250;
            Point finish  = new Point(start.X, -magnitude);
            g.DrawLine(headingPen, start,finish);
#endif

#if !COG
            if (cog > 180)
                cog = cog - 360;
            float theta = cog - yaw;

            GraphicsState savedStated2 = g.Save();      // Save the current state of the graphics object so that we dont alter everything to here
            g.RotateTransform(Convert.ToSingle(theta)); // rotate graphics object by difference from current rotation and draw line
            g.DrawLine(cogPen, start, new Point(start.X, -300)); // After rotating the object, draw the line to represent course over ground
            g.Restore(savedStated2);                // Restore back to ooriginal state but leaving the cog rotated.
#endif
            #endregion



            g.ResetTransform();
            g.Restore(savedStated);
        }
    }
}
