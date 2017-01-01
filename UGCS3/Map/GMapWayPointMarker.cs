using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;


using System.Drawing;

namespace UGCS3.Map
{
    class GMapWayPointMarker:GMap.NET.WindowsForms.GMapMarker
    {

        Bitmap bitmap;
        Pen bitpen;
        public int Radius;
        public Rectangle rect;
        private Rectangle rectB;
        private Point pnt;
        private Point pntB;
        int RR; // radius resolution
        public GMapWayPointMarker(PointLatLng pt, Bitmap bmp, int radius):base(pt)
        {
            bitmap = bmp;
            // Size = bitmap.Size;

            bitpen = new Pen(Brushes.White, 3);
            bitpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Radius = radius;

            // Offset = new Point(-Size.Width / 2, -Size.Height / 2);           
            rect  = new Rectangle(LocalPosition.X-RR, LocalPosition.Y - RR, 2*RR, 2*RR);
            rectB = new Rectangle(LocalPosition.X - this.bitmap.Size.Width / 2, LocalPosition.Y - this.bitmap.Size.Height, this.bitmap.Size.Width, this.bitmap.Size.Height);
          
            pnt   = new Point();
            pntB  = new Point();
        }

        private void Update_Rect(int _rr)
        {
            // local position gives marker position in local coordinates to the map.
            // local posiion gives left corner points of the marker 
            /*
            int center_coordinate_x = LocalPosition.X + Size.Width / 2;
            int center_coordinate_y = LocalPosition.Y + Size.Height / 2;

            pnt.X = center_coordinate_x - RR;
            pnt.Y = center_coordinate_y - RR;
            rect.Location = pnt;
            rect.Width = 2*RR;
            rect.Height = 2*RR;
            */

            pnt.X = LocalPosition.X;
            pnt.Y = LocalPosition.Y;
            rect.Location = pnt;
            rect.Width = 2 * RR;
            rect.Height = 2 * RR;
        }

        private void Update_RectB()
        {
            pntB.X = LocalPosition.X + RR - bitmap.Size.Width/2;// -this.bitmap.Size.Width / 2;
            pntB.Y = LocalPosition.Y + RR - bitmap.Size.Height;// -this.bitmap.Size.Height;
            rectB.Width = this.bitmap.Size.Width;
            rectB.Height = this.bitmap.Size.Height;
            rectB.Location = pntB;

            /*
            pntB.X = LocalPosition.X;// -this.bitmap.Size.Width / 2;
            pntB.Y = LocalPosition.Y;// -this.bitmap.Size.Height;
            rectB.Width = this.bitmap.Size.Width;
            rectB.Height = this.bitmap.Size.Height;
            rectB.Location = pntB;
             */ 
        }


        Pen rectBPen = new Pen(Brushes.Green, 3);
        int _old_rr;
        public override void OnRender(System.Drawing.Graphics g)
        {
            // ground resolution gives you the distance on the ground reprensented by a single pixel
            // ground distance/resolution gives the number of screen pixels.

            int R = (int)((Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat));
            RR = R;

            if(_old_rr != RR)
            {
                Offset = new Point(-RR, -RR);
                Size = new Size(RR * 2, RR * 2);
            }
            _old_rr = RR;


            Update_Rect(RR);
            Update_RectB();
            
            g.DrawImage(bitmap, rectB);
            // g.DrawRectangle(rectBPen, rectB);

            g.DrawEllipse(bitpen, rect);
        }
    }
}
