using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;

public class ObliqueStereographicTest
{
 
    [Test]
    public void Testfile()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        TextAsset mytxtData = (TextAsset)Resources.Load("GIGS_conv_5104_OblStereo_output");
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
            if (linetext.Length<5)
            {
                continue;
            }
            string[] lineparts = linetext.Split('\t');
            double lat;
            double.TryParse(lineparts[1],out lat);
            double lon;
            double.TryParse(lineparts[2], out lon);
            double east;
            double.TryParse(lineparts[3], out east);
            double north;
            double.TryParse(lineparts[4], out north);
            if (lineparts[6]=="REVERSE")
            {

                bool withinTolerance = TestReverse(east, north, lat, lon);
                if (!withinTolerance)
                {
                    Debug.Log("failed at test :" + lineparts[0]);
                }
                Assert.IsTrue(withinTolerance);
            }
            if (lineparts[6]=="FORWARD")
            {
                bool withinTolerance = TestForward(lon, lat, east, north);
                if (!withinTolerance)
                {
                    Debug.Log("failed at test :" + lineparts[0]);
                }
                Assert.IsTrue(withinTolerance);
            }
        }

    }

    public bool TestReverse(double inputEast, double inputNorth, double targetLat, double targetLon)
    {
        
        OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, new Ellipsoid(6377397.155, 299.1528128));
        Vector3LatLong result = om.Reverse(new Vector3Projection(inputEast, inputNorth, 0,CoordinateSystem.AmersfoortRdNew));
        Vector3LatLong target = new Vector3LatLong(targetLat, targetLon, 0);
        bool withinTolerance = CheckReverse(result, target, 0.0000006);
        return withinTolerance;
    }

    public bool TestForward(double inputLon,double inputLat, double targetEast, double targetNorth)
    {
        OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, new Ellipsoid(6377397.155, 299.1528128));
        Vector3Projection result = om.Forward(new Vector3LatLong(inputLat, inputLon, 0));
        Vector3Projection target = new Vector3Projection(targetEast, targetNorth, 0, CoordinateSystem.AmersfoortRdNew);
        return CheckForward(result, target, 0.05);
        
    }

    bool CheckForward(Vector3Projection result, Vector3Projection target, double tolerance)
    {
        bool withinTolerance = true;
        if (AbsoluteDifference(result.east, target.east) > tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.north, target.north) > tolerance)
        {
            withinTolerance = false;
        }
        return withinTolerance;
    }

    bool CheckReverse(Vector3LatLong result, Vector3LatLong target,double tolerance)
    {
        bool withinTolerance = true;
        if (AbsoluteDifference(result.lattitude,target.lattitude)>tolerance)
        {
            withinTolerance = false;
        }
        if (AbsoluteDifference(result.longitude, target.longitude) > tolerance)
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
