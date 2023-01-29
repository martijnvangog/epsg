using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;
namespace epsg
{

public class GeographicGeocentricTest
{


    [Test]
    public void Testfile()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        TextAsset mytxtData = (TextAsset)Resources.Load("GIGS_tfm_5201_GeogGeocen_output");
        string[] textLines = mytxtData.text.Split('\n');
        int linecount = textLines.Length;
        string linetext;
        for (int i = 0; i < linecount; i++)
        {
            linetext = textLines[i];
            if (linetext.StartsWith("#"))
            {
                continue;
            }
            if (linetext.Length < 5)
            {
                continue;
            }
            string[] lineparts = linetext.Split('\t');
            double lat;
            double.TryParse(lineparts[4], out lat);
            double lon;
            double.TryParse(lineparts[5], out lon);
            double height;
            double.TryParse(lineparts[6], out height);
            double x;
            double.TryParse(lineparts[1], out x);
            double y;
            double.TryParse(lineparts[2], out y);
            double z;
            double.TryParse(lineparts[3], out z);
            if (lineparts[8] == "REVERSE")
            {
                bool withinTolerance = TestReverse(x, y, z, lat, lon, height);
                Assert.IsTrue(withinTolerance);
                if (!withinTolerance)
                {
                    Debug.Log("failed at test :" + lineparts[0]);
                }
                Assert.IsTrue(withinTolerance);
            }
            if (lineparts[8] == "FORWARD")
            {
                
                bool withinTolerance = TestForward(x,y,z,lat,lon,height);
                Assert.IsTrue(withinTolerance);
                if (!withinTolerance)
                {
                    Debug.Log("failed at test :" + lineparts[0]);
                }
               
            }
        }

    }

    public bool TestReverse(double X, double Y, double Z, double lat, double lon, double ellipsoidalHeight)
    {
        crs.CRS crs = new crs.WGS84();
        Vector3GeoCentric result = crs.ToGeoCentric(new Vector3LatLong(lat, lon, ellipsoidalHeight,CrsNames.UNKNOWN));
        Vector3GeoCentric target = new Vector3GeoCentric(X, Y, Z,CrsNames.UNKNOWN);
        double tolerance = 0.01;
        return CheckReverse(result, target, tolerance);

    }

    public bool TestForward(double X, double Y, double Z, double lat, double lon, double ellipsoidalHeight)
    {
        crs.CRS crs = new crs.WGS84();
        Vector3LatLong result = crs.ToEllipsoidal(new Vector3GeoCentric(X, Y, Z,CrsNames.UNKNOWN));
        Vector3LatLong target = new Vector3LatLong(lat, lon, ellipsoidalHeight,CrsNames.UNKNOWN);
        double tolerance = 0.0003 / 3600;
        return CheckForward(result, target, tolerance);


    }

    bool CheckReverse(Vector3GeoCentric result, Vector3GeoCentric target, double tolerance)
    {
        bool withinTolerance = true;
        if (AbsoluteDifference(result.x, target.x) > tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.y, target.y) > tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.z, target.z) > tolerance)
        {
            withinTolerance = false;
        }
        return withinTolerance;
    }

    bool CheckForward(Vector3LatLong result, Vector3LatLong target, double tolerance)
    {
        bool withinTolerance = true;
        if (AbsoluteDifference(result.lattitude, target.lattitude) > tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.longitude, target.longitude) > tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.ellipsoidalHeight, target.ellipsoidalHeight) > 0.01)
        {
            withinTolerance = false;
        }
        return withinTolerance;
    }
    double AbsoluteDifference(double value1, double value2)
    {
        return Math.Abs(value1 - value2);
    }
}

}