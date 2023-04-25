using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{

[CreateAssetMenu(fileName = "newDatum", menuName = "EPSG/Datum", order = 1)]


    public class DatumObject : ScriptableObject
    {
    public new string name;
    public int epsgCode;
    public crs.Ellipsoid ellipsoid = new crs.Ellipsoid(0,0);
    public DatumObject connectedDatum;
    public double[] toConnectedDatum = new double[8];
    
}
}