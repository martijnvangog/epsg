using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRSSettings : MonoBehaviour
{
    [SerializeField] CoordinateSystem baseCoordinateSystem;
    public CoordinateSystem UnityBaseCRS
    {
        get { return baseCoordinateSystem; }
    }
    [SerializeField] Vector3Projection offset;


    private void Awake()
    {
        epsg.UnityOffset = offset;
        epsg.unityBaseCoordinateSystemID = baseCoordinateSystem;
    }
}
