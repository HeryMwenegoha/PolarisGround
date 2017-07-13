using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGCS3.Common
{
    public class FlickerFreePanel : System.Windows.Forms.Panel
    {

        public FlickerFreePanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer | System.Windows.Forms.ControlStyles.ResizeRedraw | System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
