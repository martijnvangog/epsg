namespace epsg
{

[System.Serializable]
public struct Vector3Projection
{
    public double east;
    public double north;
    public double up;
    public ProjectionName projectionName;
    public Vector3Projection(double east, double north, double up, ProjectionName projectionName)
    {
        this.east = east;
        this.north = north;
        this.up = up;
        this.projectionName = projectionName;
    }
}


}