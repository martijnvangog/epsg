using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace epsg
{
    public class NTV2Test

    {
        [Test]
        public void Testfile()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            TextAsset mytxtData = (TextAsset)Resources.Load("GIGS_tfm_5207_NTv2_output_part2");
            string[] textLines = mytxtData.text.Split('\n');
            int linecount = textLines.Length;
            string linetext;
            for (int i = 0; i < linecount; i++)
            {
                linetext = textLines[i];
                if (linetext.StartsWith("#"))
                {
                    continue;
                }
                if (linetext.Length < 5)
                {
                    continue;
                }
                string[] lineparts = linetext.Split('\t');
                double lat27;
                double.TryParse(lineparts[1], out lat27);
                double lon27;
                double.TryParse(lineparts[2], out lon27);
                double lat83;
                double.TryParse(lineparts[3], out lat83);
                double lon83;
                double.TryParse(lineparts[4], out lon83);
                if (lineparts[7] == "TRUE")
                {
                    continue;
                }
                if (lineparts[6] == "REVERSE")
                {

                    bool withinTolerance = TestForward(lat83, lon83, lat27, lon27, transformationDirection.reverse);
                    if (!withinTolerance)
                    {
                        Debug.Log("failed at test :" + lineparts[0]);
                    }
                    Assert.IsTrue(withinTolerance);
                }
                if (lineparts[6] == "FORWARD")
                {
                    bool withinTolerance = TestForward(lat27, lon27, lat83, lon83, transformationDirection.Forward);
                    if (!withinTolerance)
                    {
                        Debug.Log("failed at test :" + lineparts[0]);
                    }
                    Assert.IsTrue(withinTolerance);
                }
            }

        }



        public bool TestForward(double inputLat, double inputLon, double targetLat, double targetLon, transformationDirection direction)
        {
            NTv2 ntv = new NTv2();
            ntv.readFile("ntv2_0(2)");
            Vector3LatLong target = new Vector3LatLong(targetLat, targetLon, 0,CrsNames.UNKNOWN);
            Vector3LatLong result = ntv.transformPoint(new Vector3LatLong(inputLat, inputLon, 0, CrsNames.UNKNOWN), direction);


            return CheckForward(result, target, 0.0000003);

        }

        bool CheckForward(Vector3LatLong result, Vector3LatLong target, double tolerance)
        {
            bool withinTolerance = true;
            if (AbsoluteDifference(result.longitude, target.longitude) > tolerance)
            {
                withinTolerance = false;
            }
            if (AbsoluteDifference(result.lattitude, target.lattitude) > tolerance)
            {
                withinTolerance = false;
            }
            return withinTolerance;
        }

        bool CheckReverse(Vector3LatLong result, Vector3LatLong target, double tolerance)
        {
            bool withinTolerance = true;
            if (AbsoluteDifference(result.lattitude, target.lattitude) > tolerance)
            {
                withinTolerance = false;
            }
            if (AbsoluteDifference(result.longitude, target.longitude) > tolerance)
            {
                withinTolerance = false;
            }
            return withinTolerance;
        }
        double AbsoluteDifference(double value1, double value2)
        {
            return System.Math.Abs(value1 - value2);
        }

    }
}