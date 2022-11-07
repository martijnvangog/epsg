using UnityEngine;

[System.Serializable]
public struct Vector3LatLong
{
    public double lattitude;
    public double longitude;
    public double ellipsoidalHeight;
    public CoordinateReferenceSystem crs;
    public Vector3LatLong(double lattitude, double longitude, double ellipsoidalHeight, CoordinateReferenceSystem crs=CoordinateReferenceSystem.WGS84)
    {
        this.lattitude = lattitude;
        this.longitude = longitude;
        this.ellipsoidalHeight = ellipsoidalHeight;
        this.crs = crs;
    }
}
