
using UnityEngine;

public class epsgUsage : MonoBehaviour
{
    
    
    public epsg.RealWorldCoordinate Inputcoordinate;
    // Start is called before the first frame update

    public epsg.CoordinateSystem transformToCoordinateSystem;

    public epsg.RealWorldCoordinate outputCoordinate;


    void Start()
    {
        outputCoordinate = epsg.Coordinates.Transform(Inputcoordinate, transformToCoordinateSystem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
