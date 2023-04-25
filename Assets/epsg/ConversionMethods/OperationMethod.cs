using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace epsg
{
    [System.Serializable]
    public abstract class OperationMethod
    {
        public abstract Vector3Projection Forward(Vector3Geographic ellipsoidalCoordinate);
        public abstract Vector3Geographic Reverse(Vector3Projection cartesianCoordinate);
    }




}
