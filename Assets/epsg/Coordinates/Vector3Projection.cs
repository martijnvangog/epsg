[System.Serializable]
public struct Vector3Projection
{
    public double east;
    public double north;
    public double up;
    public CoordinateSystem cs;
    public Vector3Projection(double east, double north, double up, CoordinateSystem cs)
    {
        this.east = east;
        this.north = north;
        this.up = up;
        this.cs = cs;
    }
}
