using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Threading;

public class RDWGS84 
{
    [Test]
    public void test1()
    {
        
        OperationMethod om = new RDBenadering();
        Vector3Projection position = new Vector3Projection(155000, 463000,0, CoordinateSystem.AmersfoortRdNew);
        Vector3LatLong result = om.Reverse(position);


        Debug.Log("lat: " + result.lattitude);
        Debug.Log("lon: " + result.longitude);
        Vector3LatLong target = new Vector3LatLong(52.1551728, 5.3872037, result.ellipsoidalHeight);
        Assert.IsTrue(CheckReverse(result,target,0.00001));

    }
    [Test]
    public void test2()
    {
        OperationMethod om = new RDBenadering();
        Vector3LatLong position = new Vector3LatLong(52.1551728, 5.3872037, 0);
        Vector3Projection result = om.Forward(position);


        Debug.Log("X: " + result.east);
        Debug.Log("Y: " + result.north);
        Vector3Projection target = new Vector3Projection(155000.00342038696, 462999.99027499696, result.up,CoordinateSystem.AmersfoortRdNew);
        Assert.IsTrue(CheckForward(result, target, 0.01));

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
