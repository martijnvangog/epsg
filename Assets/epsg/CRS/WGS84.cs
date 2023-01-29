namespace epsg.crs
{
    public class WGS84 : CRS
    {
        public WGS84()
        {
            name = "WGS84";
            epsgCode = 4326;
            connectedCRS = 0;
            ellipsoid = new Ellipsoid(6378137, 298.257223563);
            toConnectedCRS = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
            
        }

    }
}
namespace epsg
{ 
    public static partial class CrsNames
    {
        public static CrsName WGS84 = new CrsName(4326, "WGS84", new crs.WGS84());
    }
}

