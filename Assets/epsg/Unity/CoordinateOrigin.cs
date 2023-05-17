using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace epsg
{
    public class CoordinateOrigin : MonoBehaviour
    {
        public RealWorldCoordinate CoordinateAtUnityOrigin;
        // Start is called before the first frame update
        void Awake()
        {

            CoordinateSystemCollection[] csCollections = (CoordinateSystemCollection[])Resources.FindObjectsOfTypeAll(typeof(CoordinateSystemCollection));
            for (int i = 0; i < csCollections.Length; i++)
            {
                Coordinates.AddTransformations(csCollections[i].transformations);
            }
            //result = Coordinates.Transform(coordinateFrom, targetCoordinateSystem);
        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}
