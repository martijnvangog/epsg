public class Rdnew : CS
{
    public Rdnew()
    {
        csEnum = CoordinateSystem.AmersfoortRdNew;
        coordinateSystemType = CoordinateSystemType.Cartesian;
        crsEnum = CoordinateReferenceSystem.Amersfoort;
        crs = new Amersfoort();
        operationMethod = new ObliqueStereographic(52.1561605555556, 5.38763888888889, 0.9999079, 155000, 463000, crs.ellipsoid);
    }
}


