using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using epsg;
using System.Threading;

public class rdValidation : MonoBehaviour
{

    RD_ETRS89 transformation;
    StreamWriter outputfile;
    
    // Start is called before the first frame update
    void Start()
    {
        transformation = new RD_ETRS89();
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        TestToETRS();
        TestToRD();
    }
    void TestToRD()
    {
        string filepath = "D:/002_ETRS89.txt";
        string text = File.ReadAllText(filepath);
        string[] lines = text.Split('\n');
        int linecount = lines.Length;

        outputfile = new StreamWriter("D:/RDNAP_output.txt");


        for (int i = 1; i < linecount; i++)
        {
            if (lines[i].Length < 10)
            {
                continue;
            }
            ProcessLineToRD(lines[i]);
        }
        outputfile.Flush();
        outputfile.Close();
    }
   void ProcessLineToRD(string linestring)
    {
        string[] lineparts = linestring.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        Vector3Geographic input = new Vector3Geographic();
        double.TryParse(lineparts[1], out input.lattitude);
        double.TryParse(lineparts[2], out input.longitude);
        double.TryParse(lineparts[3], out input.ellipsoidalHeight);
        Vector3Projection result = transformation.ETRS89toRDNAP(input);
        string[] results = new string[3];

        outputfile.WriteLine(lineparts[0]+ " " + result.east + " "+result.north + " "+result.up);
    }

    void TestToETRS()
    {
        string filepath = "D:/002_RDNAP.txt";
        string text = File.ReadAllText(filepath);
        string[] lines = text.Split('\n');
        int linecount = lines.Length;

        outputfile = new StreamWriter("D:/EPSG_output.txt");


        for (int i = 1; i < linecount; i++)
        {
            if (lines[i].Length < 10)
            {
                continue;
            }
            ProcessLineToEPSG(lines[i]);
            
        }
        outputfile.Flush();
        outputfile.Close();
    }
    void ProcessLineToEPSG(string linestring)
    {
        string[] lineparts = linestring.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        Vector3Projection input = new Vector3Projection();
        
        double.TryParse(lineparts[1], out input.east);
        double.TryParse(lineparts[2], out input.north);
        double.TryParse(lineparts[3], out input.up);
        Vector3Geographic result = transformation.RDNAPtoETRS89(input);
        string[] results = new string[3];

        outputfile.WriteLine(lineparts[0] + " " + result.lattitude + " " + result.longitude + " " + result.ellipsoidalHeight);
    }
}
