using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg.tranformations;

namespace epsg
{
    public class CoordinateSystemCollection : MonoBehaviour
    {
        public List<CoordinateSystem> coordinateSystems;
        public List<Transformation> transformations;

        private void Awake()
        {
            Coordinates.AddTransformations(transformations);
        }

        
    }
}
