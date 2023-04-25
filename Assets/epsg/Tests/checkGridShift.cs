using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using epsg;
public class checkGridShift : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        string filepath = "D:/RDNAPTRANS2018_v220627/Variant1/rdcorr2018.txt";
        string text = File.ReadAllText(filepath);
        string[] lines = text.Split('\n');
        int linecount = lines.Length;
        NTv2 grid = new NTv2();
        grid.readFile("rdcorr2018");
        for (int i = 9933; i < linecount; i++)
        {

            string[] lineparts = lines[i].Split('\t', System.StringSplitOptions.RemoveEmptyEntries);
            Vector3Geographic listposition = new Vector3Geographic();
            double.TryParse(lineparts[0], out listposition.lattitude);
            double.TryParse(lineparts[1], out listposition.longitude);
            double deltaLat;
            double deltaLon;
            double.TryParse(lineparts[2], out deltaLat);
            double.TryParse(lineparts[3], out deltaLon);
            Vector3Geographic gridfiledifference = grid.transformPoint(listposition,transformationDirection.Forward);


            int differenceLat = (int)(1000000000 * (gridfiledifference.lattitude-listposition.lattitude - deltaLat));
            int differenceLon = (int)(1000000000 * (gridfiledifference.longitude-listposition.longitude- deltaLon));
            if (differenceLat!=0)
            {
                Debug.Log(listposition.lattitude + " " + listposition.longitude);
            }
            if (differenceLon != 0)
            {
                Debug.Log(listposition.lattitude + " " + listposition.longitude);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
