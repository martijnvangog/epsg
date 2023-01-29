namespace epsg
{
    public class Rdnew : CS
    {
        public Rdnew()
        {
            csEnum = CoordinateSystem.AmersfoortRdNew;
            coordinateSystemType = CoordinateSystemType.Cartesian;

            crs = new crs.Amersfoort();
            operationMethod = new RDBenadering();
        }
    }
}

