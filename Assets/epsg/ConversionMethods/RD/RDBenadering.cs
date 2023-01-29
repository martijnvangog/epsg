using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{
    public class RDBenadering : OperationMethod
    {
        private byte[] RDCorrectionX;
        private byte[] RDCorrectionY;
        private byte[] RDCorrectionZ;

        public RDBenadering()
        {
            RDCorrectionX = Resources.Load<TextAsset>("x2c").bytes;
            RDCorrectionY = Resources.Load<TextAsset>("y2c").bytes;
            RDCorrectionZ = Resources.Load<TextAsset>("nlgeo04").bytes;
        }


        //setup coefficients for lattitude-calculation
        private double[] Kp = new double[] { 0, 2, 0, 2, 0, 2, 1, 4, 2, 4, 1 };
        private double[] Kq = new double[] { 1, 0, 2, 1, 3, 2, 0, 0, 3, 1, 1 };
        private double[] Kpq = new double[] { 3235.65389, -32.58297, -0.24750, -0.84978, -0.06550, -0.01709, -0.00738, 0.00530, -0.00039, 0.00033, -0.00012 };
        //setup coefficients for longitude-calculation
        private double[] Lp = new double[] { 1, 1, 1, 3, 1, 3, 0, 3, 1, 0, 2, 5 };
        private double[] Lq = new double[] { 0, 1, 2, 0, 3, 1, 1, 2, 4, 2, 0, 0 };
        private double[] Lpq = new double[] { 5260.52916, 105.94684, 2.45656, -0.81885, 0.05594, -.05607, 0.01199, -0.00256, 0.00128, 0.00022, -0.00022, 0.00026 };
        public override Vector3LatLong Reverse(Vector3Projection cartesianCoordinate)
        {
            //coordinates of basepoint in RD
            double refRDX = 155000;
            double refRDY = 463000;

            //coordinates of basepoint in WGS84
            double refLon = 5.38720621;
            double refLat = 52.15517440;

            double correctionX = RDCorrection(cartesianCoordinate.east, cartesianCoordinate.north, "X", RDCorrectionX);
            double correctionY = RDCorrection(cartesianCoordinate.east, cartesianCoordinate.north, "Y", RDCorrectionY);

            double DeltaX = (cartesianCoordinate.east + correctionX - refRDX) * Math.Pow(10, -5);
            double DeltaY = (cartesianCoordinate.north + correctionY - refRDY) * Math.Pow(10, -5);



            //calculate lattitude
            double Deltalat = 0;
            for (int i = 0; i < Kpq.Length; i++)
            {
                Deltalat += Kpq[i] * Math.Pow(DeltaX, Kp[i]) * Math.Pow(DeltaY, Kq[i]);
            }
            Deltalat = Deltalat / 3600;
            double lat = Deltalat + refLat;

            //calculate longitude
            double Deltalon = 0;
            for (int i = 0; i < Lpq.Length; i++)
            {
                Deltalon += Lpq[i] * Math.Pow(DeltaX, Lp[i]) * Math.Pow(DeltaY, Lq[i]);
            }
            Deltalon = Deltalon / 3600;
            double lon = Deltalon + refLon;

            //output result
            Vector3LatLong output = new Vector3LatLong();
            output.longitude = lon;
            output.lattitude = lat;
            return output;
        }


        //setup coefficients for X-calculation
        private double[] Rp = new double[] { 0, 1, 2, 0, 1, 3, 1, 0, 2 };
        private double[] Rq = new double[] { 1, 1, 1, 3, 0, 1, 3, 2, 3 };
        private double[] Rpq = new double[] { 190094.945, -11832.228, -114.221, -32.391, -0.705, -2.340, -0.608, -0.008, 0.148 };
        //setup coefficients for Y-calculation
        private double[] Sp = new double[] { 1, 0, 2, 1, 3, 0, 2, 1, 0, 1 };
        private double[] Sq = new double[] { 0, 2, 0, 2, 0, 1, 2, 1, 4, 4 };
        private double[] Spq = new double[] { 309056.544, 3638.893, 73.077, -157.984, 59.788, 0.433, -6.439, -0.032, 0.092, -0.054 };

        public override Vector3Projection Forward(Vector3LatLong ellipsoidalCoordinate)
        {
            //coordinates of basepoint in RD
            double refRDX = 155000;
            double refRDY = 463000;

            //coordinates of basepoint in WGS84
            double refLon = 5.38720621;
            double refLat = 52.15517440;

            double DeltaLon = 0.36 * (ellipsoidalCoordinate.longitude - refLon);
            double DeltaLat = 0.36 * (ellipsoidalCoordinate.lattitude - refLat);

            //calculate X
            double DeltaX = 0;
            for (int i = 0; i < Rpq.Length; i++)
            {
                DeltaX += Rpq[i] * Math.Pow(DeltaLat, Rp[i]) * Math.Pow(DeltaLon, Rq[i]);
            }
            double X = DeltaX + refRDX;

            //calculate Y
            double DeltaY = 0;
            for (int i = 0; i < Spq.Length; i++)
            {
                DeltaY += Spq[i] * Math.Pow(DeltaLat, Sp[i]) * Math.Pow(DeltaLon, Sq[i]);
            }
            double Y = DeltaY + refRDY;

            double correctionX = RDCorrection(X, Y, "X", RDCorrectionX);
            double correctionY = RDCorrection(X, Y, "Y", RDCorrectionY);
            X -= correctionX;
            Y -= correctionY;

            //output result
            Vector3Projection output = new Vector3Projection();
            output.east = (float)X;
            output.north = (float)Y;
            output.up = 0;
            return output;
        }




        /// <summary>
        /// correction for RD-coordinatesystem
        /// </summary>
        /// <param name="x">X-value of coordinate when richting is X or Y, else longitude</param>
        /// <param name="y">Y-value of coordinate when richting is X or Y, else lattitude</param>
        /// <param name="richting">X, Y, or Z</param>
        /// <returns>correction for RD X and Y or Elevationdifference between WGS84  and RD</returns>
        public Double RDCorrection(double x, double y, string richting, byte[] bytes)
        {
            double waarde = 0;
            //TextAsset txt;

            if (richting == "X")
            {
                //txt = RDCorrectionX;
                waarde = -0.185;
            }
            else if (richting == "Y")
            {
                //txt = RDCorrectionY;
                waarde = -0.232;
            }
            else
            {
                //DeltaH tussen wGS en NAP
                //txt = RDCorrectionZ;
            }


            //byte[] bytes = txt.bytes;

            double Xmin;
            double Xmax;
            double Ymin;
            double Ymax;
            int sizeX;
            int sizeY;

            int datanummer;
            sizeX = BitConverter.ToInt16(bytes, 4);
            sizeY = BitConverter.ToInt16(bytes, 6);
            Xmin = BitConverter.ToDouble(bytes, 8);
            Xmax = BitConverter.ToDouble(bytes, 16);
            Ymin = BitConverter.ToDouble(bytes, 24);
            Ymax = BitConverter.ToDouble(bytes, 32);

            double kolombreedte = (Xmax - Xmin) / sizeX;
            double locatieX = Math.Floor((x - Xmin) / kolombreedte);
            double rijhoogte = (Ymax - Ymin) / sizeY;
            double locatieY = (long)Math.Floor((y - Ymin) / rijhoogte);

            if (locatieX < Xmin || locatieX > Xmax)
            {
                return waarde;
            }
            if (locatieY < Ymin || locatieY > Ymax)
            {
                return waarde;
            }

            datanummer = (int)(locatieY * sizeX + locatieX);

            // do linear interpolation on the grid
            if (locatieX < sizeX && locatieY < sizeY)
            {
                float linksonder = BitConverter.ToSingle(bytes, 56 + (datanummer * 4));
                float rechtsonder = BitConverter.ToSingle(bytes, 56 + ((datanummer + 1) * 4));
                float linksboven = BitConverter.ToSingle(bytes, 56 + ((datanummer + sizeX) * 4));
                float rechtsboven = BitConverter.ToSingle(bytes, 56 + ((datanummer + sizeX + 1) * 4));

                double Yafstand = ((y - Ymin) % rijhoogte) / rijhoogte;
                double YgewogenLinks = ((linksboven - linksonder) * Yafstand) + linksonder;
                double YgewogenRechts = ((rechtsboven - rechtsonder) * Yafstand) + rechtsonder;

                double Xafstand = ((x - Xmin) % kolombreedte) / kolombreedte;
                waarde += ((YgewogenRechts - YgewogenLinks) * Xafstand) + YgewogenLinks;
            }
            else
            {

                float myFloat = System.BitConverter.ToSingle(bytes, 56 + (datanummer * 4));
                waarde += myFloat;
            }
            //datanummer = 1500;




            return waarde;
        }

    }
}
