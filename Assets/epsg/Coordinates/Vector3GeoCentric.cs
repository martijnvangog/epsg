
namespace epsg
{
    [System.Serializable]

    public struct Vector3GeoCentric
{
    public double x;
    public double y;
    public double z;

    public Vector3GeoCentric(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

    }
}
}
