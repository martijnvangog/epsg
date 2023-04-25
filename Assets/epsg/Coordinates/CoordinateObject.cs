using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace epsg
{

    [CreateAssetMenu(fileName = "newCoordinateSystemtest", menuName = "EPSG/CoordinateSystem", order = 1)]
    public abstract class CoordinateObject : ScriptableObject
    {
        [SerializeField]
        public int epsgCode;
        [SerializeField]
        public new string name;
        [SerializeField]
        public CoordinateReferenceSystem crs;
        [SerializeField]
        public CoordinateSystemType type;
        [SerializeField]
        public List<Axis> Axes = new List<Axis>();
    }

  
  

  
   
        }