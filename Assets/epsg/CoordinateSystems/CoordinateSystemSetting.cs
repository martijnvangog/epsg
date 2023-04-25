using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg {
    [System.Serializable]
    public class CoordinateSystemSetting
    {
        public string name;
        public int epsgCode;
        public CoordinateSystemType coordinateSystemType;
        
    }
}