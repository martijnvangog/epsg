namespace epsg.crs
{
    public class ETRS89 : CRS
    {
        public ETRS89()
        {
            name = "ETRS89";
            epsgCode = 4937;
            ellipsoid = new Ellipsoid(6378137, 298.257222101);
            connectedCRS = 4326;
            toConnectedCRS = new double[7] { -0.0536, -0.0508, 0.0855, -0.00254, -0.00211, -0.01274, 0.02059 };
        }

    }

}
namespace epsg
{ 
public static partial class CrsNames
{
    public static CrsName ETRS89 = new CrsName(4937, "ETRS89",new crs.ETRS89());
}
}
