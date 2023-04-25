using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DatumProperties
{
    public int epsgCode;
    public string name;
    public epsg.crs.Ellipsoid ellipsoid;
    public List<datumLink> linkedDatums;
}

[System.Serializable]
public struct datumLink
{
    public int connectedCRS;
    public double[] toConnectedCRS;
}
