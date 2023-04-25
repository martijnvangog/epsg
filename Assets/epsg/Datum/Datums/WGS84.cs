namespace epsg.crs
{
    public class WGS84 : DatumSetting
    {
        public WGS84()
        {
            name = "WGS84";
            epsgCode = 4326;
            connectedDatum = 0;
            ellipsoid = new Ellipsoid(6378137, 298.257223563);
            toConnectedDatum = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
            
        }

    }
}


