using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMap;
using GMap.NET;
using GMap.NET.WindowsForms;
namespace UGCS3.Common
{
    public static class common
    {
        /// <summary>
        /// Gets the distance in metres between 2 waypoints of a certain mapcontrol
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static UInt32 distance(PointLatLng p1, PointLatLng p2, GMapControl obj)
        {
            return (UInt32)(obj.MapProvider.Projection.GetDistance(p1, p2) * 1000);
        }


        public static float gradient(PointLatLng p1, PointLatLng p2, float alt1, float alt2, GMapControl obj)
        {
            float grad = (alt2 - alt1) / (float)distance(p1, p2, obj);
            grad = grad * 100;
            return grad;
        }
    }
}
