using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;
using epsg;
using epsg.crs;

public class RDWGS84 
{
    [Test]
    public void RDNew()
    {
        Vector3Projection position = new Vector3Projection(120000, 480000, 0, ProjectionNames.rdNew);
        Vector3LatLong result = epsg.epsg.FromProjection(position, CrsNames.ETRS89);
        Vector3LatLong target = new Vector3LatLong(52.306852305, 4.874015360, 0, CrsNames.UNKNOWN);

        Debug.Log("lat: " + result.lattitude);
        Debug.Log("lon: " + result.longitude);
        // Vector3LatLong target = new Vector3LatLong(52.1551728, 5.3872037, result.ellipsoidalHeight);
        Assert.IsTrue(CheckReverse(result, target, 0.0000003));
    }

    [Test]
    public void RDtoETRS()
    {
        CRS crsAmersfoort = new Amersfoort();
        CRS etrs89 = new ETRS89();
        OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, crsAmersfoort.ellipsoid);
        //project rd to Amersfoort
        Vector3Projection position = new Vector3Projection(120000,480000,0, ProjectionNames.rdNew);
        Vector3LatLong bessel = om.Reverse(position);

        //apply correctionGrid
        NTv2 ntv2 = new NTv2();
        ntv2.readFile("rdcorr2018");
        Vector3LatLong PseudoBessel = ntv2.transformPoint(bessel,transformationDirection.Forward);

        //datumTransformation to ETRS89        
        Vector3GeoCentric amersfoortGeoCentric = crsAmersfoort.ToGeoCentric(PseudoBessel);
        Vector3GeoCentric etrsGeoCentric = crsAmersfoort.ToConnectedGeoCentric(amersfoortGeoCentric);
        
        Vector3LatLong result = etrs89.ToEllipsoidal(etrsGeoCentric);

        Vector3LatLong target = new Vector3LatLong(52.306852305, 4.874015360,0,CrsNames.UNKNOWN);

        Debug.Log("lat: " + result.lattitude);
        Debug.Log("lon: " + result.longitude);
       // Vector3LatLong target = new Vector3LatLong(52.1551728, 5.3872037, result.ellipsoidalHeight);
        Assert.IsTrue(CheckReverse(result,target,0.0000003));

    }
    [Test]
    public void ETRStoRD()
    {
        CRS crsAmersfoort = new Amersfoort();
        CRS etrs89 = new ETRS89();
        NTv2 ntv2 = new NTv2();
        ntv2.readFile("rdcorr2018");
        OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, crsAmersfoort.ellipsoid);
        Vector3LatLong input = new Vector3LatLong(52.306850000, 4.874000000, 0,CrsNames.UNKNOWN);
        Vector3GeoCentric etrsGeo = etrs89.ToGeoCentric(input);
        Vector3GeoCentric amersfoortpseudo = crsAmersfoort.FromConnectedGeoCentric(etrsGeo);
        Vector3LatLong besselPseudo = crsAmersfoort.ToEllipsoidal(amersfoortpseudo);
        Vector3LatLong bessel = ntv2.transformPoint(besselPseudo, transformationDirection.reverse);
        Vector3Projection rd = om.Forward(bessel);
        Debug.Log(rd.east);
        Debug.Log(rd.north);
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

    bool CheckReverse(Vector3LatLong result, Vector3LatLong target, double tolerance)
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
        return withinTolerance;
    }
    double AbsoluteDifference(double value1, double value2)
    {
        return Math.Abs(value1 - value2);
    }
}
