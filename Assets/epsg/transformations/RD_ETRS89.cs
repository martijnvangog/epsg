using epsg.crs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace epsg
{
    public class RD_ETRS89
    {
        ElevationGrid eg;
        NTv2 ntv2;
        public Vector3Projection ETRS89toRDNAP(Vector3Geographic coordinate)
        {
            DatumSetting crsAmersfoort = new Amersfoort();
            DatumSetting etrs89 = new ETRS89();
            Vector3Projection result = new Vector3Projection(0,0,0);

            // find elveationOffset
            if (eg is null)
            {
                eg = new ElevationGrid();
                eg.ReadFile("nlgeo2018");
            }
            
            
            float elevationdifference = eg.GetElevation(coordinate);
            if (elevationdifference == float.NaN)
            {
                result.up = double.NaN;
            }
            else
            {
                result.up = coordinate.ellipsoidalHeight - elevationdifference;
            }
            //transfom to pseudo-bessel

            coordinate.ellipsoidalHeight = 43;
            Vector3GeoCentric etrsGeoCentric = etrs89.ToGeoCentric(coordinate);
            Vector3GeoCentric amersfoortGeoCentric = crsAmersfoort.FromConnectedGeoCentric(etrsGeoCentric);
            Vector3Geographic pseudobessel = crsAmersfoort.ToEllipsoidal(amersfoortGeoCentric);

            //apply correctiongrid
            //apply correctionGrid
            if (ntv2 is null)
            {
                ntv2 = new NTv2();
                ntv2.readFile("rdcorr2018");
            }
            Vector3Geographic bessel = ntv2.transformPoint(pseudobessel, transformationDirection.reverse);

            //project to RD
            OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, crsAmersfoort.ellipsoid);
            Vector3Projection position = om.Forward(bessel);
            result.east = position.east;
            result.north = position.north;
            return result;
        }

        public Vector3Geographic RDNAPtoETRS89(Vector3Projection coordinate)
        {
            DatumSetting crsAmersfoort = new Amersfoort();
            DatumSetting etrs89 = new ETRS89();
            OperationMethod om = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, crsAmersfoort.ellipsoid);
            //project rd to Amersfoort



            
            Vector3Geographic bessel = om.Reverse(coordinate);

            //apply correctionGrid
            if (ntv2 == null)
            {
                ntv2 = new NTv2();
                ntv2.readFile("rdcorr2018");
            }
            Vector3Geographic PseudoBessel = ntv2.transformPoint(bessel, transformationDirection.Forward);

            //datumTransformation to ETRS89
            PseudoBessel.ellipsoidalHeight = 0;
            Vector3GeoCentric amersfoortGeoCentric = crsAmersfoort.ToGeoCentric(PseudoBessel);
            Vector3GeoCentric etrsGeoCentric = crsAmersfoort.ToConnectedGeoCentric(amersfoortGeoCentric);

            Vector3Geographic result = etrs89.ToEllipsoidal(etrsGeoCentric);

            // apply Vertical correctiongrid
            if (eg is null)
            {
                eg = new ElevationGrid();
                eg.ReadFile("nlgeo2018");
            }
            float elevationdifference = eg.GetElevation(result);
            result.ellipsoidalHeight = coordinate.up + elevationdifference;
            return result;
        }
        
    }
}
