using UnityEngine;
namespace epsg
{

[System.Serializable]
public struct Vector3Geographic
{
    public double lattitude;
    public double longitude;
    public double ellipsoidalHeight;
    
    public Vector3Geographic(double lattitude, double longitude, double ellipsoidalHeight)
    {
        this.lattitude = lattitude;
        this.longitude = longitude;
        this.ellipsoidalHeight = ellipsoidalHeight;
        
    }
}

}