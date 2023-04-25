namespace epsg.crs
{
    public class Amersfoort : DatumSetting
    {
        public Amersfoort()
        {
            name = "Amersfoort";
            epsgCode = 4289;
            ellipsoid = new Ellipsoid(6377397.155, 299.1528128);
            connectedDatum = 4326;
            toConnectedDatum = new double[7] { 565.7381, 50.4018, 465.2904, 0.395026, -0.330772, 1.876074, 4.07244 };
        }

    }

}


