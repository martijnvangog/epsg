using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace epsg
{
    public class RDNew : Projection
    {
        public RDNew()
        {
            epsgCode = 7415;
            name = "RDNew";
            connectedCRS = 4937;
           
        }


        crs.CRS amersfoortCrs;
        crs.CRS etrs89Crs;
        OperationMethod om;
        NTv2 ntv2;
        bool datacollected = false;
        public void collectData()
        {
            if (!datacollected)
            {
                amersfoortCrs = epsg.usedCRSs[4289];
                etrs89Crs = epsg.usedCRSs[4937];
                om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, amersfoortCrs.ellipsoid);
                ntv2 = new NTv2();
                ntv2.readFile("rdcorr2018");
                datacollected = true;
            }
        
        }
        
        public override Vector3Projection Project(Vector3LatLong coordinate)
        {
            collectData();
            Vector3GeoCentric etrsGeoCentric = etrs89Crs.ToGeoCentric(coordinate);
            Vector3GeoCentric amersfoortrGeoCentric = amersfoortCrs.FromConnectedGeoCentric(etrsGeoCentric);
            Vector3LatLong amersfoortLatLon = amersfoortCrs.ToEllipsoidal(amersfoortrGeoCentric);
            Vector3LatLong amersfoortReal = ntv2.transformPoint(amersfoortLatLon, transformationDirection.reverse);
            Vector3Projection result = om.Forward(amersfoortReal);
            result.projectionName = ProjectionNames.rdNew;
            return result;
            
        }

        public override Vector3LatLong unProject(Vector3Projection coordinate)
        {
            collectData();
            //projectToAmersfoort
            Vector3LatLong bessel = om.Reverse(coordinate);
            //apply correctionGrid
            Vector3LatLong PseudoBessel = ntv2.transformPoint(bessel, transformationDirection.Forward);

            //datumTransformation to ETRS89        
            Vector3GeoCentric amersfoortGeoCentric = amersfoortCrs.ToGeoCentric(PseudoBessel);
            Vector3GeoCentric etrsGeoCentric = amersfoortCrs.ToConnectedGeoCentric(amersfoortGeoCentric);
            Vector3LatLong result = etrs89Crs.ToEllipsoidal(etrsGeoCentric);
            result.crsName = CrsNames.ETRS89;
            return result;
            
        }
    }
    public static partial class ProjectionNames
    {
        public static ProjectionName rdNew { get; } = new ProjectionName(7415, "RD", new RDNew());

    }
}