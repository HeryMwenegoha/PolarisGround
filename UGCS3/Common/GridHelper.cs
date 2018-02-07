using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGCS3.Common
{
    public class GridHelper
    {
        private float rmax1;
        public double max;
        private float rmin1;
        public double min;

        public GridHelper()
        {
          rmax1 = 0;
          max = 0;
          rmin1 = 100000;
          min = 0;
        }

        public float sign(double pt)
        {
            if (pt >= 0)
                return 1.0f;
            else
                return -1.0f;
        }

        public  void Record(float dis, double pt)
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

        public  void RecordMax(float r1, double r2)
        {
            float new_record1 = Math.Max(rmax1, r1);
            if (new_record1 != rmax1)
            {
                max = r2;
                rmax1 = new_record1;
            }
        }


        public  void RecordMin(float r1, double r2)
        {
            float new_record1 = Math.Min(rmin1, r1);
            if (new_record1 != rmin1)
            {
                min = r2;
                rmin1 = new_record1;
            }
        }


    }
}
