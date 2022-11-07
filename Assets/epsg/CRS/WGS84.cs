public class WGS84 : CRS
{
    public WGS84()
    {
        crsEnum = CoordinateReferenceSystem.WGS84;
        ellipsoid = new Ellipsoid(6378137, 298.257223563);
        toWGS84 = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
    }

}


