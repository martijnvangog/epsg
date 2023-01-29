using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{
    public abstract class CS
    {
        public CoordinateSystem csEnum;
        public CoordinateSystemType coordinateSystemType;
        public crs.CRS crs;

        public OperationMethod operationMethod;

        public Vector3GeoCentric ToWGS84Geocentric(Vector3LatLong coordinate)
        {
            //from crs-ellipsoidal to crs-geocentric
            Vector3GeoCentric geocentricOnCRS = crs.ToGeoCentric(coordinate);
            //from crs-geocentric to wgs84-geocentric
            Vector3GeoCentric geocentricOnWGS84 = crs.ToConnectedGeoCentric(geocentricOnCRS);
            //geocentricOnWGS84.crs = CrsNames.WGS84;
            return geocentricOnWGS84;
        }
        public Vector3LatLong FromWGS84GeoCentric(Vector3GeoCentric coordinate)
        {
            // from wgs84-geocentric to crs-geocentric
            Vector3GeoCentric geocentricOnCRS = crs.FromConnectedGeoCentric(coordinate);
            //from crs-geocentric to crs-ellipsoidal
            Vector3LatLong ellipsoidal = crs.ToEllipsoidal(geocentricOnCRS);
            //ellipsoidal.crs = CrsNames.WGS84;
            return ellipsoidal;
        }
    }

}
