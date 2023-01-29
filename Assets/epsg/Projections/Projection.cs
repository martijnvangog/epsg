namespace epsg
{
    public abstract class Projection
    {
        public int epsgCode;
        public string name;
        public int connectedCRS;
        public abstract Vector3Projection Project(Vector3LatLong coordinate);
        public abstract Vector3LatLong unProject(Vector3Projection coordinate);
    }
}