using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;
using epsg;
public class CoordinateFrameRotationTest
{
    
    //[Test]
    //public void Testfile()
    //{
    //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    //    TextAsset mytxtData = (TextAsset)Resources.Load("GIGS_tfm_5204_CoordFrame_output_part2");
    //    string[] textLines = mytxtData.text.Split('\n');
    //    int linecount = textLines.Length;
    //    string linetext;
    //    for (int i = 0; i < linecount; i++)
    //    {
    //        linetext = textLines[i];
    //        if (linetext.StartsWith("#"))
    //        {
    //            continue;
    //        }
    //        if (linetext.Length < 5)
    //        {
    //            continue;
    //        }
    //        string[] lineparts = linetext.Split('\t');
    //        double lat;
    //        double.TryParse(lineparts[1], out lat);
    //        double lon;
    //        double.TryParse(lineparts[2], out lon);
    //        double height;
    //        double.TryParse(lineparts[3], out height);
    //        double lat2;
    //        double.TryParse(lineparts[4], out lat2);
    //        double lon2;
    //        double.TryParse(lineparts[5], out lon2);
    //        double height2;
    //        double.TryParse(lineparts[6], out height2);

    //        if (lineparts[6] == "REVERSE")
    //        {
    //            bool withinTolerance = Test2(lat2, lon2,height2, epsg.epsg.coordinateReferenceSystems.WGS84, lat, lon, height);

    //            if (!withinTolerance)
    //            {
    //                Debug.Log("failed at test :" + lineparts[0]);
    //            }
    //            Assert.IsTrue(withinTolerance);
    //        }
    //        if (lineparts[6] == "FORWARD")
    //        {

    //            bool withinTolerance = Test2(lat, lon,height, , lat2, lon2,height2);

    //            if (!withinTolerance)
    //            {
    //                Debug.Log("failed at test :" + lineparts[0]);
    //            }
    //            Assert.IsTrue(withinTolerance);
    //        }
    //    }

    //}
    //public bool Test1(double Lat, double Lon,CRS sourceCRS,double targetLat, double targetLon)
    //{
    //    CRS WGScrs = new WGS84();
    //    CRS crs = new Amersfoort();
    //    crs.toConnectedCRS = new double[] { -106.8686, 52.2978, -103.7239, -0.3366, 0.457, -1.8422, -1.2747 };
    //    crs.ellipsoid = new Ellipsoid(6378388, 297);

    //    Vector3LatLong target = new Vector3LatLong(targetLat, targetLon, 0);

    //    if (sourceCRS == epsg.coordinateReferenceSystems.WGS84)
    //    {
            
    //        Vector3GeoCentric sourceGeocentric = WGScrs.ToGeoCentric(new Vector3LatLong(Lat, Lon, 0));
    //        Vector3GeoCentric targetGeocentric = crs.FromConnectedGeoCentric(sourceGeocentric);
    //        Vector3LatLong result = crs.ToEllipsoidal(targetGeocentric);
    //        return CheckLatLong(result, target, 0.0000003);
    //    }
    //   else
    //    {
    //        Vector3GeoCentric sourceGeocentric = crs.ToGeoCentric(new Vector3LatLong(Lat, Lon, 0));
    //        Vector3GeoCentric targetGeocentric = crs.ToConnectedGeoCentric(sourceGeocentric);
    //        Vector3LatLong result = WGScrs.ToEllipsoidal(targetGeocentric);
    //        return CheckLatLong(result, target, 0.0000003);
    //    }


    //}
    //public bool Test2(double Lat, double Lon, double height, CRS sourceCRS, double targetLat, double targetLon, double targetHeight)
    //{
    //    CRS WGScrs = new WGS84();
    //    CRS crs = new Amersfoort();
    //    crs.toConnectedCRS = new double[] { -106.8686, 52.2978, -103.7239, -0.3366, 0.457, -1.8422, -1.2747 };
    //    crs.ellipsoid = new Ellipsoid(6378388, 297);

    //    Vector3LatLong target = new Vector3LatLong(targetLat, targetLon, targetHeight);

    //    if (sourceCRS == epsg.coordinateReferenceSystems.WGS84)
    //    {
    //        Vector3GeoCentric sourceGeocentric = WGScrs.ToGeoCentric(new Vector3LatLong(Lat, Lon, height));
    //        Vector3GeoCentric targetGeocentric = crs.FromConnectedGeoCentric(sourceGeocentric);
    //        Vector3LatLong result = crs.ToEllipsoidal(targetGeocentric);
    //        return CheckLatLong(result, target, 0.0000003,0.03);
    //    }
    //    else
    //    {
    //        Vector3GeoCentric sourceGeocentric = crs.ToGeoCentric(new Vector3LatLong(Lat, Lon, height));
    //        Vector3GeoCentric targetGeocentric = crs.ToConnectedGeoCentric(sourceGeocentric);
    //        Vector3LatLong result = WGScrs.ToEllipsoidal(targetGeocentric);
    //        return CheckLatLong(result, target, 0.0000003 ,0.03);
    //    }


    //}



    //bool CheckLatLong(Vector3LatLong result, Vector3LatLong target, double geographicTolerance, double cartesianTolerance=0)
    //{
    //    bool withinTolerance = true;
    //    if (AbsoluteDifference(result.lattitude, target.lattitude) > geographicTolerance)
    //    {
    //        Debug.Log("latt:" +AbsoluteDifference(result.lattitude, target.lattitude));
    //        withinTolerance = false;
    //    }
    //    if (AbsoluteDifference(result.longitude, target.longitude) > geographicTolerance)
    //    {
    //        Debug.Log("lon: "+ AbsoluteDifference(result.longitude, target.longitude));
    //        withinTolerance = false;
    //    }
    //    if (cartesianTolerance==0)
    //    {
    //        return withinTolerance;
    //    }
    //    if (AbsoluteDifference(result.ellipsoidalHeight, target.ellipsoidalHeight) > cartesianTolerance)
    //    {
    //        Debug.Log("ellipsoidal height: " + AbsoluteDifference(result.ellipsoidalHeight, target.ellipsoidalHeight));
    //        withinTolerance = false;
    //    }
    //    return withinTolerance;
    //}
    //double AbsoluteDifference(double value1, double value2)
    //{
    //    return Math.Abs(value1 - value2);
    //}
}
