using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

using System;

namespace epsg.operationMethods
{
    public class GeographicToGeocentricTest
    {
        GeographicToGeocentricSettings geog2geoc;
        double[,] testdata;
        [Test]
        public void SingleTest()
        {
            geog2geoc = new GeographicToGeocentricSettings();
            geog2geoc.semiMajorAxis = 6378137.0;
            geog2geoc.inverseFlattening = 298.2572236;
            Vector3Any input = new Vector3Any(3771793.968, 140253.342, 5124304.349);
            Vector3Any result = GeographicToGeocentric.Reverse(input, geog2geoc);

        }

        [Test]
        public void multiTestReverse()
        {
            SetupCoordinateSystems();
            setupTestdata();

            for (int i = 0; i < 26; i++)
            {
                Vector3Any from = new Vector3Any(testdata[i, 0], testdata[i, 1], testdata[i, 2]);
                Vector3Any target = new Vector3Any(testdata[i, 3], testdata[i, 4], testdata[i, 5]);
                Vector3Any result = GeographicToGeocentric.Reverse(from, geog2geoc);
                Assert.IsTrue(CheckResultReverse(result, target));
            }

        }

        [Test]
        public void multiTestForward()
        {
            SetupCoordinateSystems();
            setupTestdata();

            for (int i = 0; i < 26; i++)
            {
                Vector3Any from = new Vector3Any(testdata[i, 0], testdata[i, 1], testdata[i, 2]);
                Vector3Any target = new Vector3Any(testdata[i, 3], testdata[i, 4], testdata[i, 5]);
                Vector3Any result = GeographicToGeocentric.Forward(target, geog2geoc);
                Assert.IsTrue(CheckResultForward(result, from));
            }

        }
        [Test]
        public void Roundtrip()
        {
            SetupCoordinateSystems();
            setupTestdata();
            for (int i = 0; i < 26; i++)
            {
                Vector3Any from = new Vector3Any(testdata[i, 0], testdata[i, 1], testdata[i, 2]);
                Vector3Any result = GeographicToGeocentric.Reverse(from, geog2geoc);
                Vector3Any reverse = GeographicToGeocentric.Forward(result, geog2geoc);
                Assert.IsTrue(CheckResultForward(from, reverse));

                from = new Vector3Any(testdata[i, 3], testdata[i, 4], testdata[i, 5]);
                result = GeographicToGeocentric.Forward(from, geog2geoc);
                reverse = GeographicToGeocentric.Reverse(result, geog2geoc);
                Assert.IsTrue(CheckResultReverse(from, reverse));


            }


        }

        bool CheckResultForward(Vector3Any result, Vector3Any target)
        {
            bool incorrect = false;
            if (Math.Abs(result.value1 - target.value1) > 0.005)
            {
                incorrect = true;
            }
            if (Math.Abs(result.value2 - target.value2) > 0.005)
            {
                incorrect = true;
            }
            if (Math.Abs(result.value3 - target.value3) > 0.005)
            {
                incorrect = true;
            }
            return !incorrect;
        }

        bool CheckResultReverse(Vector3Any result, Vector3Any target)
        {
            bool incorrect = false;
            if (Math.Abs(result.value1-target.value1)> 0.0003/3600)
            {
                incorrect = true;
            }
            if (Math.Abs(result.value2 - target.value2) > 0.0003 / 3600)
            {
                incorrect = true;
            }
            if (Math.Abs(result.value3 - target.value3) > 0.01)
            {
                incorrect = true;
            }
            return !incorrect;
        }

        private void setupTestdata()
        {
            testdata = new double[,]
            {
                {-962479.592 ,555687.852 ,6260738.653 ,80 ,150 ,1214.137},
                {-962297.006 ,555582.435 ,6259542.961 ,80 ,150 ,0},
                {-1598248.169 ,2768777.623 ,5501278.468 ,60.00475191 ,119.9952454 ,619.6317},
                {-1598023.169 ,2768387.912 ,5500499.045 ,60.00475258 ,119.9952447 ,-280.3683},
                {2764210.405 ,4787752.865 ,3170468.52 ,30 ,60 ,189.569},
                {2764128.32 ,4787610.688 ,3170373.735 ,30 ,60 ,0},
                {6377934.396 ,-112 ,434 ,0.00392509 ,-0.00100615 ,-202.5882},
                {6374934.396 ,-112 ,434 ,0.00392695 ,-0.00100662 ,-3202.5881},
{6367934.396 ,-112 ,434 ,0.00393129 ,-0.00100773 ,-10202.5881},
{2764128.32 ,-4787610.688 ,-3170373.735 ,-30 ,-60 ,0 },
{2763900.349 ,-4787215.831 ,-3170110.497 ,-30 ,-60 ,-526.476},
{2763880.863 ,-4787182.081 ,-3170087.997 ,-30 ,-60 ,-571.476},
{-1598023.169 ,-2768611.912 ,-5499631.045 ,-59.99934884 ,-119.9932376 ,-935.0995},
{-1597798.169 ,-2768222.201 ,-5498851.622 ,-59.99934874 ,-119.9932366 ,-1835.0995},
{-962297.006 ,-555582.435 ,-6259542.961 ,-80 ,-150 ,0},
{-962150.945 ,-555498.107 ,-6258586.462 ,-80 ,-150 ,-971.255},
{-961798.295 ,-555294.505 ,-6256277.087 ,-80 ,-150 ,-3316.255},
{-2187336.719 ,-112 ,5971017.093 ,70.00490733 ,-179.9970662 ,-223.6178},
{-2904698.555 ,-2904698.555 ,4862789.038 ,50 ,-135 ,0},
{371 ,-5783593.614 ,2679326.11 ,25.00366329 ,-89.99632465 ,-274.7286},
{6378137 ,0 ,0 ,0 ,0 ,0},
{-4087095.478 ,2977467.559 ,-3875457.429 ,-37.65282217 ,143.9264925 ,737.7182},
{-4085919.959 ,2976611.233 ,-3874335.274 ,-37.65282206 ,143.9264921 ,-1099.2288},
{-4084000.165 ,2975212.729 ,-3872502.631 ,-37.65282187 ,143.9264914 ,-4099.2288},
{-4079520.647 ,2971949.553 ,-3868226.465 ,-37.65282143 ,143.9264898 ,-11099.2288},
{-2904698.555 ,2904698.555 ,-4862789.038 ,-50 ,135 ,0},
{-2187336.719 ,-112 ,-5970149.093 ,-70.00224647 ,-179.9970662 ,-1039.2896}
            };
        }

        private void SetupCoordinateSystems()
        {

            geog2geoc = new GeographicToGeocentricSettings();
            geog2geoc.semiMajorAxis = 6378137;
            geog2geoc.inverseFlattening = 298.257223563;
        }
    }
}
