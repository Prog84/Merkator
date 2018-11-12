using System;
using System.Collections.Generic;
using System.Threading;
using static UtilityLibraries.MerkatorLibrary;
using UtilityLibraries;

namespace Merkator
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(Read).Start("A1");
            new Thread(Read).Start("A2");
            new Thread(Read).Start("A3");
            Console.ReadKey();
        }
        static void Read(object threadID)
        {
            Random rnd = new Random();
            double value = rnd.Next(1000, 10000);
            MapPoint SphereMercatorPoint1 = new MapPoint(4187591.89 + value, 7509137.58 + value);
            MapPoint SphereMercatorPoint2 = new MapPoint(4179839.4589181677 + value, 7522970.542341189 + value);
            List<MapPoint> MyMercatorPoint = new List<MapPoint>();
            MyMercatorPoint.Add(SphereMercatorPoint1);
            MyMercatorPoint.Add(SphereMercatorPoint2);
            List<MapPoint> PointToReproject1 = new MerkatorSphereToWGS84(MyMercatorPoint);
            foreach (MapPoint element in PointToReproject1)
            {
                Console.WriteLine("Thread " + threadID + " value " + value + " convert x" + element.x);
                Console.WriteLine("Thread " + threadID + " value " + value + " convert y" + element.y);
            }
                   
        }
    }
}





