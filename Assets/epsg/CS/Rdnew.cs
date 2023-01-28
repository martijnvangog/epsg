public class Rdnew : CS
{
    public Rdnew()
    {
        csEnum = CoordinateSystem.AmersfoortRdNew;
        coordinateSystemType = CoordinateSystemType.Cartesian;
        crsEnum = CoordinateReferenceSystem.Amersfoort;
        crs = new Amersfoort();
        operationMethod = new RDBenadering();
    }
}


