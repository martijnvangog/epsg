
namespace epsg
{
    [System.Serializable]

    public struct Vector3GeoCentric
{
    public double x;
    public double y;
    public double z;
    public CrsName crs;
    public Vector3GeoCentric(double x, double y, double z, CrsName crsname)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.crs = crsname;

    }
}
}
