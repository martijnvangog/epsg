using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OperationMethod 
{
    public abstract Vector3Projection Forward(Vector3LatLong ellipsoidalCoordinate);
    public abstract Vector3LatLong Reverse(Vector3Projection cartesianCoordinate);
}
