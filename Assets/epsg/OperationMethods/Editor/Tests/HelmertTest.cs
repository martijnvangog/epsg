using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace epsg.operationMethods
{
    public class HelmertTest
    {
        HelmertSettings settings;
        GeographicToGeocentricSettings geog2geoc1;
        GeographicToGeocentricSettings geog2geoc2;
        double[,] testdata;
        // A Test behaves as an ordinary method



        [Test]
        public void SingleTest()
        {
            settings = new HelmertSettings();
            settings.tX = -446.448;
            settings.tY = 125.157;
            settings.tZ = -542.06;
            settings.rX = -0.1502;
            settings.rY = -0.247;
            settings.rZ = -0.8421;
            settings.dS = 20.4894;

            Vector3Any input = new Vector3Any(3909833.018, -147097.1376, 5020322.478);
            Vector3Any result = Helmert.Apply(input, settings,OperationDirection.Forward);
            Vector3Any target = new Vector3Any(3909460.068, -146987.301, 5019888.070);
            Assert.IsTrue(CompareGeocentric(result, target));
        }

        [Test]
        public void TestRoundTrip()
        {
            SetupCoordinateSystems();
            Vector3Any from = new Vector3Any(79.99487333, 150.0056747, 0);
            Vector3Any ecef = GeographicToGeocentric.Forward(from, geog2geoc1);

            Vector3Any ecef2 = Helmert.Apply(ecef, settings, OperationDirection.Forward);
            //Vector3Any result = GeographicToGeocentric.Reverse(ecef2, geog2geoc2);
            // and back again
            //Vector3Any ecef3 = GeographicToGeocentric.Forward(result, geog2geoc2);

            Vector3Any ecef3 = Helmert.Apply(ecef2, settings, OperationDirection.Reverse);
            //result = GeographicToGeocentric.Reverse(ecef, geog2geoc1);
            Assert.IsTrue(CompareGeocentric(ecef, ecef3));


        }


        [Test]
        public void TestForward()
        {
            SetupCoordinateSystems();
            SetuptestDataForward();
            for (int i = 0; i < 7; i++)
            {

                Vector3Any from = new Vector3Any(testdata[i, 0], testdata[i, 1],0);
                Vector3Any ecef = GeographicToGeocentric.Forward(from, geog2geoc1);
                Vector3Any ecef2 = Helmert.Apply(ecef, settings,OperationDirection.Forward);
                Vector3Any result = GeographicToGeocentric.Reverse(ecef2, geog2geoc2);
                Vector3Any target = new Vector3Any(testdata[i, 2], testdata[i, 3],0);

                Assert.IsTrue(compareValues(result, target));
            }
        }
        [Test]
        public void TestReverse()
        {
            SetupCoordinateSystems();
            SetuptestDataReverse();
            for (int i = 0; i < 6; i++)
            {

                Vector3Any from = new Vector3Any(testdata[i, 2], testdata[i, 3], 0);
                Vector3Any ecef = GeographicToGeocentric.Forward(from, geog2geoc2);
                Vector3Any ecef2 = Helmert.Apply(ecef, settings, OperationDirection.Reverse);
                Vector3Any result = GeographicToGeocentric.Reverse(ecef2, geog2geoc1);
                Vector3Any target = new Vector3Any(testdata[i, 0], testdata[i, 1], 0);

                Assert.IsTrue(compareValues(result, target));
            }
        }
        bool CompareGeocentric(Vector3Any result, Vector3Any target)
        {
            if (Math.Abs(result.value1 - target.value1)>0.001)
            {
                return false;
            }
            if (Math.Abs(result.value2 - target.value2) > 0.001)
            {
                return false;
            }
            if (Math.Abs(result.value3 - target.value3) > 0.001)
            {
                return false;
            }
            return true;
        }

        bool compareValues(Vector3Any result, Vector3Any target)
        {
            double tolerance = 0.0000003;
            bool inaccurate = false;
            if (Math.Abs(result.value1-target.value1)>tolerance)
            {
                inaccurate = true;
            }
            if (Math.Abs(result.value2 - target.value2) > tolerance)
            {
                inaccurate = true;
            }
            return !inaccurate;
        }

        bool compareValuesRoundtrip(Vector3Any result, Vector3Any target)
        {
            double tolerance = 0.00000006;
            bool inaccurate = false;
            if (Math.Abs(result.value1 - target.value1) > tolerance)
            {
                inaccurate = true;
            }
            if (Math.Abs(result.value2 - target.value2) > tolerance)
            {
                inaccurate = true;
            }
            return !inaccurate;
        }

        private void SetupCoordinateSystems()
        {
            //setup
            settings = new HelmertSettings();
            settings.tX = 446.448;
            settings.tY = -125.157;
            settings.tZ = 542.06;
            settings.rX = 0.1502;
            settings.rY = 0.247;
            settings.rZ = 0.8421;
            settings.dS = -20.4894;

            geog2geoc1 = new GeographicToGeocentricSettings(); //epsg 4277
            geog2geoc1.semiMajorAxis = 6377563.396;
            geog2geoc1.inverseFlattening = 299.3249646;
            geog2geoc2 = new GeographicToGeocentricSettings();
            geog2geoc2.semiMajorAxis = 6378137.0;
            geog2geoc2.inverseFlattening = 298.257223563;
        }

        void SetuptestDataForward()
        {
            testdata = new double[,]
            {
                //{79.99487333 ,150.0056747 ,80 ,150},
                {60 ,120 ,60.00569306 ,119.9943589},
                //{29.99566778 ,60.00446778 ,30 ,60},
                {0 ,0 ,0.00483333 ,-0.00089056},
                //{-30.00504639 ,-60.00357056 ,-30 ,-60},
                {-60 ,-120 ,-59.99907361 ,-119.9918525},
                //{-79.99778139 ,-150.0169311 ,-80 ,-150},
                {70 ,-180 ,70.005945 ,-179.9963736},
                //{49.99458694 ,-135.0059633 ,50 ,-135},
                {25 ,-90 ,25.00445833 ,-89.99531139},
                //{-0.00483333 ,0.00089056 ,0 ,0},
                {-37.6532236 ,143.9279419 ,-37.65235306 ,143.9263481},
                //{-49.99973028 ,135.0029119 ,-50 ,135},
                {-70 ,180 ,-70.002485 ,-179.9966014}
            };
        }

        void SetuptestDataReverse()
        {
            testdata = new double[,]
            {
                {79.99487333 ,150.0056747 ,80 ,150},
                //{60 ,120 ,60.00569306 ,119.9943589},
                {29.99566778 ,60.00446778 ,30 ,60},
                //{0 ,0 ,0.00483333 ,-0.00089056},
                {-30.00504639 ,-60.00357056 ,-30 ,-60},
                //{-60 ,-120 ,-59.99907361 ,-119.9918525},
               // {-79.99778139 ,-150.0169311 ,-80 ,-150},
                //{70 ,-180 ,70.005945 ,-179.9963736},
                {49.99458694 ,-135.0059633 ,50 ,-135},
                //{25 ,-90 ,25.00445833 ,-89.99531139},
                {-0.00483333 ,0.00089056 ,0 ,0},
                //{-37.6532236 ,143.9279419 ,-37.65235306 ,143.9263481},
                {-49.99973028 ,135.0029119 ,-50 ,135},
                //{-70 ,180 ,-70.002485 ,-179.9966014}
            };
        }
    }
}
