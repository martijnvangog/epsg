public class Amersfoort : CRS
{
    public Amersfoort()
    {
        crsEnum = CoordinateReferenceSystem.Amersfoort;
        ellipsoid = new Ellipsoid(6377397.155, 299.1528128);
        toWGS84 = new double[7] { 565.4171, 50.3319, 465.5524, 1.9342, -1.6677, 9.1019, 4.0725 };
    }

}


