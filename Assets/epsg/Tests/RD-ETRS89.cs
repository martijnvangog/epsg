using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;
using epsg;
using epsg.crs;

public class RDETRS89
{
   

    [Test]
    public void rdnaptransToRD()
    {
        RD_ETRS89 transformation = new RD_ETRS89();
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        TextAsset mytxtData = (TextAsset)Resources.Load("Z001_ETRS89andRDNAP");
        string[] textLines = mytxtData.text.Split('\n');
        int linecount = textLines.Length;
        //linecount = 10157;
        string linetext;
        string[] lineparts;
        int testcounter = 0;
        int succescounter = 0;
        int startline = 0;
        //startline = 157;
        for (int i = startline; i < linecount; i++)
        {
            linetext = textLines[i];
            if (linetext.Length == 0)
            {
                continue;
            }
            lineparts = linetext.Split(" ");
            Vector3Projection rdcoordinaat = new Vector3Projection();
            Vector3Geographic target = new Vector3Geographic();
            bool succes = Double.TryParse(lineparts[4], out rdcoordinaat.east);
            succes = Double.TryParse(lineparts[5], out rdcoordinaat.north);
            if (lineparts[6] == "*")
            {
                rdcoordinaat.up = double.NaN;
            }

            succes = Double.TryParse(lineparts[6], out rdcoordinaat.up);
            succes = Double.TryParse(lineparts[1], out target.lattitude);
            succes = Double.TryParse(lineparts[2], out target.longitude);
            succes = Double.TryParse(lineparts[3], out target.ellipsoidalHeight);
            testcounter++;
            Vector3Projection result = transformation.ETRS89toRDNAP(target);
            //Vector3Geographic result = transformation.RDNAPtoETRS89(rdcoordinaat);
            if (checkRD(rdcoordinaat,result))
            {
                succescounter++;
            }
            else
            {
                Debug.Log(lineparts[0]);
            }
        }




        Debug.Log(succescounter + "/"+testcounter);

        Assert.IsTrue(succescounter == testcounter);
    }
    bool checkRD(Vector3Projection target, Vector3Projection result)
    {
        if (Math.Abs(target.east-result.east)>0.001)
        {
            return false;
        }
        if (Math.Abs(target.north - result.north) > 0.001)
        {
            return false;
        }
        if (target.up == double.NaN)
        {
            if (result.up !=double.NaN)
            {
                return false;
            }
            return true;
        }
        if (Math.Abs(target.up - result.up) > 0.001)
        {
            return false;
        }
        return true;
    }

    [Test]
    public void rdnaptrans()
    {
        RD_ETRS89 transformation = new RD_ETRS89();
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        TextAsset mytxtData = (TextAsset)Resources.Load("rd-problems");
        string[] textLines = mytxtData.text.Split('\n');
        int linecount = textLines.Length;
        //linecount = 10157;
        string linetext;
        string[] lineparts;
        int testcounter = 0;
        int succescounter = 0;
        int startline = 0;
        //startline = 157;
        for (int i = startline; i < linecount; i++)
        {
            linetext = textLines[i];
            if (linetext.Length==0)
            {
                continue;
            }
            lineparts = linetext.Split(" ");
            Vector3Projection rdcoordinaat = new Vector3Projection();
            bool succes = Double.TryParse(lineparts[4], out rdcoordinaat.east);
            succes = Double.TryParse(lineparts[5], out rdcoordinaat.north);
            if (lineparts[6] == "*")
            {
                continue;
            }
            
                succes = Double.TryParse(lineparts[6], out rdcoordinaat.up);
            
            Vector3Geographic target = new Vector3Geographic();
            succes = Double.TryParse(lineparts[1], out target.lattitude);
            succes = Double.TryParse(lineparts[2], out target.longitude);
            succes = Double.TryParse(lineparts[3], out target.ellipsoidalHeight);
            testcounter++;
            Vector3Geographic result = transformation.RDNAPtoETRS89(rdcoordinaat);
            if (CheckReverse(result, target, 0.00000004))
            {
                succescounter++;
            }
            else
            {
                int deltaLon = (int)(Math.Abs(target.longitude - result.longitude) * 100000000);
                int deltaLat = (int)(Math.Abs(target.lattitude - result.lattitude) * 100000000);
                int deltaH = (int)(Math.Abs(target.ellipsoidalHeight - result.ellipsoidalHeight) * 1000);
                Debug.Log(lineparts[0]+":"+deltaLon + " "+deltaLat + " "+deltaH) ;
            }
        }



        


        Assert.IsTrue(succescounter == testcounter);
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

    bool CheckReverse(Vector3Geographic result, Vector3Geographic target, double tolerance)
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
        if (AbsoluteDifference(result.ellipsoidalHeight,target.ellipsoidalHeight)>0.001)
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
