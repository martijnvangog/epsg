using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3LatLong coords = new Vector3LatLong(1, 2, 3,CrsNames.Amersfoort);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
