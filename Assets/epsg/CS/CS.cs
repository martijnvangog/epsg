using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS
{
    public CoordinateSystem csEnum;
    public CoordinateSystemType coordinateSystemType;
    public CoordinateReferenceSystem crsEnum;
    public CRS crs;
    public OperationMethod operationMethod;

    public Vector3GeoCentric ToWGS84Geocentric(Vector3Projection coordinate)
    {
        //from projection to crs-ellipsoidal
        Vector3LatLong ellipsoidal = operationMethod.Reverse(coordinate);
        //from crs-ellipsoidal to crs-geocentric
        Vector3GeoCentric geocentricOnCRS = crs.ToGeoCentric(ellipsoidal);
        //from crs-geocentric to wgs84-geocentric
        Vector3GeoCentric geocentricOnWGS84 = crs.ToWGS84GeoCentric(geocentricOnCRS);
        return geocentricOnWGS84;
    }
    public Vector3Projection FromWGS84GeoCentric(Vector3GeoCentric coordinate)
    {
        // from wgs84-geocentric to crs-geocentric
        Vector3GeoCentric geocentricOnCRS = crs.FromWGS84GeoCentric(coordinate);
        //from crs-geocentric to crs-ellipsoidal
        Vector3LatLong ellipsoidal = crs.ToEllipsoidal(geocentricOnCRS);
        //from crs-ellipsoidal to cs-projection
        Vector3Projection cartesian = operationMethod.Forward(ellipsoidal);
        //attach the coordinateSystem-id to the coordinate
        cartesian.cs = csEnum;
        return cartesian;
    }
}


