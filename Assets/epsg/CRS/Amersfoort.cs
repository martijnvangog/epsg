namespace epsg.crs
{
    public class Amersfoort : CRS
    {
        public Amersfoort()
        {
            name = "Amersfoort";
            epsgCode = 4289;
            ellipsoid = new Ellipsoid(6377397.155, 299.1528128);
            connectedCRS = 4326;
            toConnectedCRS = new double[7] { 565.4171, 50.3319, 465.5524, 1.9342, -1.6677, 9.1019, 4.0725 };
        }

    }

}
namespace epsg
{
    
    public static partial class CrsNames
    {
        public static CrsName Amersfoort = new CrsName(4289, "Amersfoort", new crs.Amersfoort());
    }
}

