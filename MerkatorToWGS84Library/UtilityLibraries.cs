using System.Collections.Generic;
using System.Threading;
using static System.Math;

namespace UtilityLibraries
{
    public class MerkatorLibrary
    {
        public class MerkatorSphereToWGS84 : List<MapPoint>
        {
            public MerkatorSphereToWGS84(IEnumerable<MapPoint> collection) : base(collection)
            {
            }
        }
        private const double R_MAJOR = 6378137.0;
        private const double R_MINOR = 6356752.3142;
        private const double RATIO = R_MINOR / R_MAJOR;
        private static readonly double ECCENT = Sqrt(1.0 - (RATIO * RATIO));
        private static readonly double COM = 0.5 * ECCENT;
        private const double DEG2RAD = PI / 180.0;
        private const double RAD2DEG = 180.0 / PI;
        private const double PI_2 = PI / 2.0;
        private static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();

        public class MapPoint
        {
            public double x { set; get; }
            public double y { set; get; }
            public MapPoint()
            {

            }
            public MapPoint(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static double RadToDeg(double rad)
        {
            return rad * RAD2DEG;
        }

        public static double DegToRad(double deg)
        {
            return deg * DEG2RAD;
        }

        public static double xToLon(double x)
        {
            return RadToDeg(x / R_MAJOR); ;
        }

        public static double yToLat(double y)
        {
            return RadToDeg(2 * Atan(Exp(y / R_MAJOR)) - PI_2);
        }

        public static double LonToxWSG84(double x)
        {
            return (x * DEG2RAD * R_MAJOR);
        }

        public static double LatToyWSG84(double y)
        {
            double yWGS84 = Min(89.5, Max(y, -89.5));
            double phi = DegToRad(yWGS84);
            double sinphi = Sin(phi);
            double con = ECCENT * sinphi;
            con = Pow((1.0 - con) / (1.0 + con), COM);
            double ts = Tan(0.5 * ((PI * 0.5) - phi)) / con;
            return (0 - R_MAJOR * Log(ts));
        }

        public List<MapPoint> MerkatorSphereToWGS84(List<MapPoint> PointToConvert)
        {
            _rw.EnterWriteLock();
            List<MapPoint> ListWGS84Point = new List<MapPoint>();
            double xToGeo, yToGeo;
            try
            {
                foreach (MapPoint element in PointToConvert)
                {
                    MapPoint WGS84Point = new MapPoint();
                    xToGeo = xToLon(element.x);
                    yToGeo = yToLat(element.y);
                    WGS84Point.x = LonToxWSG84(xToGeo);
                    WGS84Point.y = LatToyWSG84(yToGeo);
                    ListWGS84Point.Add(WGS84Point);
                }
            }
            finally
            {
                _rw.ExitWriteLock();
            }
            return ListWGS84Point;
        }
    } 
}







