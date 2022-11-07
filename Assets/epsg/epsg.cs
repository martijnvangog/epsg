using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class epsg
{
    
    public static CoordinateSystem unityBaseCoordinateSystemID;
    public static Vector3Projection UnityOffset;

   
    public static Dictionary<CoordinateSystem, CS> availableCoordinateSystems;
    public static Dictionary<CoordinateReferenceSystem, CRS> availableCoordinateReferenceSystems;

    public static void Setup()
    {
       
        availableCoordinateSystems = new Dictionary<CoordinateSystem, CS>();
        availableCoordinateReferenceSystems = new Dictionary<CoordinateReferenceSystem, CRS>();
    }
    public static CS SetCoordinateSytem(CoordinateSystem systemID)
    {
        CS cs = null;
        if (availableCoordinateSystems.ContainsKey(systemID))
        {
            cs = availableCoordinateSystems[systemID];
        }
        else
        {
        switch (systemID)
        {
            case CoordinateSystem.None:
                break;
            case CoordinateSystem.Unity:
                break;
            case CoordinateSystem.AmersfoortRdNew:
                    cs = new Rdnew();
                    availableCoordinateSystems.Add(systemID, cs);
                    if (!availableCoordinateReferenceSystems.ContainsKey(cs.crsEnum))
                    {
                        availableCoordinateReferenceSystems.Add(cs.crsEnum, cs.crs);
                    }
                break;
            case CoordinateSystem.WGS84Utm30N:
                break;
            case CoordinateSystem.WGS84PseudoMercator:
                break;
            default:
                break;
        }
            
        }
        return cs;
    }
    public static CRS SetCoordinateReferenceSytem(CoordinateReferenceSystem systemID)
    {

        CRS crs = null;
        if (availableCoordinateReferenceSystems.ContainsKey(systemID))
        {
            crs= availableCoordinateReferenceSystems[systemID];
        }
        else
        {
            switch (systemID)
            {
                case CoordinateReferenceSystem.None:
                    break;
                case CoordinateReferenceSystem.WGS84:
                    crs = new WGS84();
                    availableCoordinateReferenceSystems.Add(systemID,crs);
                    break;
                case CoordinateReferenceSystem.Amersfoort:
                    crs = new Amersfoort();
                    availableCoordinateReferenceSystems.Add(systemID, crs);
                    break;
                default:
                    break;
            }

        }
        return crs;
    }

    //public static Vector3 ToUnity(Vector3Projection coordinate)
    //{
    //    return new Vector3();
    //}
    //public static Vector3 ToUnity(Vector3LatLong coordinate)
    //{
    //    //CoordinateReferenceSystem sourceCRS = coordinate.crs;
    //    //if (sourceCRS == baseCoordinateSystem.crsEnum)
    //    //{
    //    //    Vector3Projection projection = baseCoordinateSystem.operationMethod.Forward(coordinate);
    //    //    Vector3 unityPosition = new Vector3((float)(projection.east - UnityOffset.east), (float)(projection.up - UnityOffset.up), (float)(projection.north - UnityOffset.north));
    //    //    return unityPosition;
    //    //}
    //    //else
    //    //{
    //    //    SetCoordinateReferenceSytem(sourceCRS);
    //    //    conversionCRS = availableCoordinateReferenceSystems[sourceCRS];
    //    //    Vector3GeoCentric geocentricInCRS = conversionCRS.ToGeoCentric(coordinate);
    //    //    Vector3GeoCentric geocentricInWGS84 = conversionCRS.ToWGS84GeoCentric(geocentricInCRS);
    //    //    Vector3Projection projection = baseCoordinateSystem.FromWGS84GeoCentric(geocentricInWGS84);
    //    //    Vector3 unityPosition = new Vector3((float)(projection.east - UnityOffset.east), (float)(projection.up - UnityOffset.up), (float)(projection.north - UnityOffset.north));
    //    //    return unityPosition;
    //    //}
        
    //}
    //public static Vector3 ToUnity(Vector3GeoCentric coordinate)
    //{
    //    return new Vector3();
    //}

    //public static Vector3Projection toProjection(Vector3 coordinate,CoordinateSystem cs)
    //{
    //    return new Vector3Projection();
    //}
    public static Vector3LatLong toLatLong(Vector3Projection coordinate, CoordinateReferenceSystem targetCrsID)
    {
        CS sourceCS = SetCoordinateSytem(coordinate.cs);
        CRS targetCRS = SetCoordinateReferenceSytem(targetCrsID);
        if (sourceCS.crsEnum != targetCrsID)
        {
            //translate to WGS84GeoCEntric
            Vector3GeoCentric geocentric = sourceCS.ToWGS84Geocentric(coordinate);
            if (targetCrsID==CoordinateReferenceSystem.WGS84)
            {
                return targetCRS.ToEllipsoidal(geocentric);
            }
        }


        return new Vector3LatLong();
    }
    //public static Vector3GeoCentric toGeoCentric(Vector3 coordinate, CoordinateReferenceSystem crs)
    //{
    //    return new Vector3GeoCentric();
    //}

}




