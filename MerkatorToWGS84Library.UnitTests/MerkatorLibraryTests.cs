using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static UtilityLibraries.MerkatorLibrary;
using UtilityLibraries;

namespace MerkatorToWGS84Library.UnitTests
{
    [TestFixture]
    public class MerkatorLibraryTests
    {
        private MerkatorLibrary _Merkator;
        [SetUp]
        public void SetUp()
        {
            _Merkator = new MerkatorLibrary();
        }
        private const double R_MAJOR = 6378137.0;
        private const double R_MINOR = 6356752.3142;
        private const double RATIO = R_MINOR / R_MAJOR;
        private static readonly double ECCENT = Sqrt(1.0 - (RATIO * RATIO));
        private static readonly double COM = 0.5 * ECCENT;
        private static readonly double yWGS84 = Min(89.5, Max(2, -89.5));
        private static readonly double phi = DegToRad(yWGS84);
        private static readonly double sinphi = Sin(phi);
        private static readonly double con = ECCENT * sinphi;
        private static readonly double denominator = Pow((1.0 - con) / (1.0 + con), COM);
        private static readonly double ts = Tan(0.5 * ((PI * 0.5) - phi)) / denominator;
        private static MapPoint MercatorPointOne = new MapPoint(4187591.89, 7509137.58);
        private static MapPoint MercatorPointTwo = new MapPoint(4179839.45, 7522970.54);
        private List<MapPoint> MercatorList = new List<MapPoint>(2) { MercatorPointOne, MercatorPointTwo };
 
        [Test]
        public void RadToDeg_WhenCalled_ReturnConvertRadiansToDegrees()
        {
            var result = RadToDeg(2);

            Assert.That(result, Is.EqualTo(2 * (180.0 / PI)));
        }

        [Test]
        public void DegToRad_WhenCalled_ReturnConvertDegreesToRadians()
        {
            var result = DegToRad(2);

            Assert.That(result, Is.EqualTo(2 * (PI / 180.0)));
        }

        [Test]
        public void xToLon_WhenCalled_ReturnConvertXToLongitude()
        {
            var result = xToLon(2);

            Assert.That(result, Is.EqualTo(RadToDeg(2 / R_MAJOR)));
        }

        [Test]
        public void YToLat_WhenCalled_ReturnConvertYToLatitude()
        {
            var result = yToLat(2);

            Assert.That(result, Is.EqualTo(RadToDeg(2 * Atan(Exp(2 / R_MAJOR)) - PI / 2)));
        }
        [Test]
        public void LonToxWSG84_WhenCalled_ReturnConvertXWSG84()
        {
            var result = LonToxWSG84(2);
            Assert.That(result, Is.EqualTo(Round(2 * (PI/180.0) * R_MAJOR, 2)));
        }
        [Test]
        public void LatToyWSG84_WhenCalled_ReturnConvertYWSG84()
        {
            var result = LatToyWSG84(2);
            Assert.That(result, Is.EqualTo(Round(0 - R_MAJOR * Log(ts), 2)));
        }
        [Test]
        public void MerkatorSphereToWGS84_WhenCalled_ReturnSameCountValues()
        {
            var result = _Merkator.MerkatorSphereToWGS84(MercatorList);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }

    
}

