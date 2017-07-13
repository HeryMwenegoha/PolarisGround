using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace UGCS3
{
    public static class GeneralMethods
    {

        public static void RotateAndTranslate(PaintEventArgs pe, Image img, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
              double beta = 0;         // Angle btn Horizontal and Initial Image Position
              double d = 0;            // Distance from Coordinate System to Point of Rotation
              float deltaXRot = 0;     // Offset to Put the ImageBack After Rotation
              float deltaYRot = 0;     // Offset to Put the ImageBack After Rotation
              float deltaXTrs = 0;     // Offset to put the ImageBack After Translation in X
              float deltaYTrs = 0;     // Offset to put the ImageBack After Translation in Y
              try
              {
                  alphaRot = alphaRot * (Math.PI / 180.0);
                  alphaTrs = alphaTrs * (Math.PI / 180.0);
              }
            catch
              {

              }
              // Rotation::
              // If the Left Corner Point of the Image is not Equal to the Rotation Point::
              if (ptImg != ptRot)
              {
                  try
                  {
                      // Internal Coefficients
                      // If the X_Rotation Point is not 0
                      if (ptRot.X != 0)
                      {
                          beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                      }
                      d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));
                      // Computed Offset :: deltaX & deltaY
                      deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                      deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
                  }
                  catch
                  {
                      // do nothing
                  }
              }
              // Translation
              // Computed Offsets::
              try
              {
                  deltaXTrs = (float)(deltaPx * Math.Sin(alphaTrs));
                  deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));
                  // Now I will rotate the Image Support::
                  pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));
                // Now i am going to displaying the Image at the new coordinates defined by the deltas::

                pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor);
                // Now I am Putting the Image Support As Found::
                pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
              }
            catch
              {
                // do nothing
              }
        }

        public static void RotateAndTranslateUpdated(PaintEventArgs pe, Image img, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
            double beta = 0;         // Angle btn Horizontal and Initial Image Position
            double d = 0;            // Distance from Coordinate System to Point of Rotation
            float deltaXRot = 0;     // Offset to Put the ImageBack After Rotation
            float deltaYRot = 0;     // Offset to Put the ImageBack After Rotation
            float deltaXTrs = 0;     // Offset to put the ImageBack After Translation in X
            float deltaYTrs = 0;     // Offset to put the ImageBack After Translation in Y
            try
            {
                alphaRot = alphaRot * (Math.PI / 180.0);
                alphaTrs = alphaTrs * (Math.PI / 180.0);
            }
            catch
            {

            }
            // Rotation::
            // If the Left Corner Point of the Image is not Equal to the Rotation Point::
            if (ptImg != ptRot)
            {
                try
                {
                    // Internal Coefficients
                    // If the X_Rotation Point is not 0
                    if (ptRot.X != 0)
                    {
                        beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                    }
                    d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));
                    // Computed Offset :: deltaX & deltaY
                    deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta)));
                    deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));
                }
                catch
                {
                    // do nothing
                }
            }
            // Translation
            // Computed Offsets::
            try
            {
                deltaXTrs = (float)(deltaPx * Math.Sin(alphaTrs));
                deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));
                // Now I will rotate the Image Support::
                pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));
                // Now i am going to displaying the Image at the new coordinates defined by the deltas::

                pe.Graphics.DrawImageUnscaledAndClipped(img, new Rectangle((int)((ptImg.X + deltaXRot + deltaXTrs) * scaleFactor), (int)((ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor), img.Size.Width, img.Size.Height));

                // Now I am Putting the Image Support As Found::
                pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
            }
            catch
            {
                // do nothing
            }
        }


        public static void RotateAndTranslate2(PaintEventArgs pe, Image img, Double yawRot, Double alphaRot, Double alphaTrs, Point ptImg, double deltaPx, Point ptRot, float scaleFactor)
        {
             
            double beta = 0;
            double d = 0;
            float deltaXRot = 0;
            float deltaYRot = 0;
            float deltaXTrs = 0;
            float deltaYTrs = 0;
            try
            {
                alphaRot = alphaRot * (Math.PI / 180.0);
                alphaTrs = alphaTrs * (Math.PI / 180.0);
                yawRot = yawRot * (Math.PI / 180.0);
            }
            catch
            {
                // do nothing
            }

              // Rotation
            if (ptImg != ptRot)
            {
                try
                {
                    // Our Internal Coefficients
                    if (ptRot.X != 0)
                    {
                        beta = Math.Atan((double)ptRot.Y / (double)ptRot.X);
                    }
                    d = Math.Sqrt((ptRot.X * ptRot.X) + (ptRot.Y * ptRot.Y));

                    // Computed Offset
                    deltaXRot = (float)(d * (Math.Cos(alphaRot - beta) - Math.Cos(alphaRot) * Math.Cos(alphaRot + beta) - Math.Sin(alphaRot) * Math.Sin(alphaRot + beta) + yawRot));
                    deltaYRot = (float)(d * (Math.Sin(beta - alphaRot) + Math.Sin(alphaRot) * Math.Cos(alphaRot + beta) - Math.Cos(alphaRot) * Math.Sin(alphaRot + beta)));

                    // Translation::
                    // Computed Offset::
                    deltaXTrs = (float)(deltaPx * Math.Sin(alphaTrs));
                    deltaYTrs = (float)(-deltaPx * (-Math.Cos(alphaTrs)));

                    // Now lets Rotate the Image Support::
                    pe.Graphics.RotateTransform((float)(alphaRot * 180 / Math.PI));

                    // Now lets display the Image::
                    pe.Graphics.DrawImage(img, (ptImg.X + deltaXRot + deltaXTrs) * scaleFactor, (ptImg.Y + deltaYRot + deltaYTrs) * scaleFactor, img.Width * scaleFactor, img.Height * scaleFactor);

                    // Put image support as found::
                    pe.Graphics.RotateTransform((float)(-alphaRot * 180 / Math.PI));
                }
                catch
                {
                    // do nothing
                }
            }
    
        }

    }

}
