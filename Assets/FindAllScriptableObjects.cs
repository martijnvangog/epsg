using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg;
using System;

public class FindAllScriptableObjects : MonoBehaviour
{
    
    
    public RealWorldCoordinate RealWordCoordinate;
   // public DatumObject datum;
    public List<DatumObject> foundDatums = new List<DatumObject>();

    // Start is called before the first frame update
    void Start()
    {
        
        var found = Resources.FindObjectsOfTypeAll(typeof(DatumObject));
        for (int i = 0; i < found.Length; i++)
        {
            foundDatums.Add((DatumObject)found[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

