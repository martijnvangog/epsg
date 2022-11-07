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
        
        
        
        epsg.Setup();
        Vector3Projection position = new Vector3Projection(120000f, 480000f,0, CoordinateSystem.AmersfoortRdNew);
        Vector3LatLong coordinate = epsg.toLatLong(position, CoordinateReferenceSystem.WGS84);
        Debug.Log("lat: " + coordinate.lattitude);
        Debug.Log("lon: " + coordinate.longitude);
        Assert.IsTrue(true);

    }
}
