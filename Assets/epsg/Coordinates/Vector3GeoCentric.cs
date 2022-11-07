[System.Serializable]
public struct Vector3GeoCentric
{
    public double x;
    public double y;
    public double z;
    public CoordinateReferenceSystem crs;
    public Vector3GeoCentric(double x, double y, double z, CoordinateReferenceSystem crs = CoordinateReferenceSystem.WGS84)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.crs = crs;
    }
}
