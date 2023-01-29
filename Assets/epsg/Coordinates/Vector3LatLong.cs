using UnityEngine;
namespace epsg
{

[System.Serializable]
public struct Vector3LatLong
{
    public double lattitude;
    public double longitude;
    public double ellipsoidalHeight;
    public CrsName crsName;
    public Vector3LatLong(double lattitude, double longitude, double ellipsoidalHeight, CrsName crsName)
    {
        this.lattitude = lattitude;
        this.longitude = longitude;
        this.ellipsoidalHeight = ellipsoidalHeight;
        this.crsName = crsName;
    }
}

}