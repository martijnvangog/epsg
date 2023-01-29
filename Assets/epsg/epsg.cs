using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{
    public static class epsg
    {

        internal static Dictionary<int, Projection> usedProjections = new Dictionary<int,Projection>();
        internal static Dictionary<int, crs.CRS> usedCRSs = new Dictionary<int, crs.CRS>();
        
        public static Vector3Projection ToProjection(Vector3LatLong coordinate, ProjectionName targetProjectionName)
        {

            return new Vector3Projection();
        }

        public static Vector3Projection ToProjection(Vector3GeoCentric coordinate, ProjectionName targetProjectionName)
        {
            return new Vector3Projection();
        }

        public static Vector3Projection ToProjection(Vector3Projection coordinate, ProjectionName targetProjectionName)
        {
            return new Vector3Projection();
        }

        public static Vector3LatLong FromProjection(Vector3Projection coordinate, CrsName targetCRSName)
        {
            Projection projection = usedProjections[coordinate.projectionName.epsgCode];
            int projectionCRScode = projection.connectedCRS;
            // unproject
            Vector3LatLong unprojected = projection.unProject(coordinate);
            if (targetCRSName.epsgCode==unprojected.crsName.epsgCode)
            {
                return unprojected;
            }
            return unprojected;
       }

        public static Vector3LatLong ConvertTo(Vector3LatLong coordinate, CrsName targetCRSName)
        {
            // transform to tartgetCRS
            coordinate.crsName = targetCRSName;
            return coordinate;

        }

        public static Vector3GeoCentric ConvertTo (Vector3GeoCentric coordinate, CrsName targetCRSName)
        {
            return new Vector3GeoCentric();
        }
    }
}




