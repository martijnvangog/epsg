using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{
    [CreateAssetMenu(fileName = "newCoordinateReferenceSystem", menuName = "EPSG/CoordinateReferenceSystem", order = 1)]
    public class CoordinateReferenceSystem : ScriptableObject
    {
        public string name;
        public int epsgCode;
        public CoordinateReferenceSystemType type;
        public CoordinateReferenceSystem projectedFromCRS;
        public OperationMethod operationMethod;

    }
}
