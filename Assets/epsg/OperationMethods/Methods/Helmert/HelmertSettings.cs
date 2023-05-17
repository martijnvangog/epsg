
using System;

namespace epsg.operationMethods
{
    [System.Serializable]
    public class HelmertSettings : OperationSettings
    {

        public double tX;
        public double tY;
        public double tZ;
        /// <summary>
        /// Rotation Around X-axis in arcseconds
        /// </summary>
        public double rX; //arcseconds
        public double rY; //arcseconds
        public double rZ; //arcseconds
        public double dS;

        public double[,] positionVectorForwardMatrix;
        public double[,] positionVectorReverseMatrix;


        public double rXrad;


        public double rYrad;

        public double rZrad;

        public double M
        {
            get { return 1 + (dS / 1000000.0); }
        }

        public void SetupMatrices()
        {
            double arsecToRad = Math.PI / (3600.0 * 180.0);
            rXrad = rX * arsecToRad;
            rYrad = rY * arsecToRad;
            rZrad = rZ * arsecToRad;
            double M = 1 + (dS / Math.Pow(10, 6));

            positionVectorForwardMatrix = new double[3, 3];
            positionVectorForwardMatrix[0, 0] = 1.0;
            positionVectorForwardMatrix[0, 1] = -rZrad;
            positionVectorForwardMatrix[0, 2] = rYrad;

            positionVectorForwardMatrix[1, 0] = rZrad;
            positionVectorForwardMatrix[1, 1] = 1;
            positionVectorForwardMatrix[1, 2] = -rXrad;

            positionVectorForwardMatrix[2, 0] = -rYrad;
            positionVectorForwardMatrix[2, 1] = rXrad;
            positionVectorForwardMatrix[2, 2] = 1;



            ////find the inverse
            //double[,] Minors = new double[3, 3];
            //Minors[0, 0] = getDeterminant(0, 0);
            //Minors[0, 1] = getDeterminant(0, 1);
            //Minors[0, 2] = getDeterminant(0, 2);
            //Minors[1, 0] = getDeterminant(1, 0);
            //Minors[1, 1] = getDeterminant(1, 1);
            //Minors[1, 2] = getDeterminant(1, 2);
            //Minors[2, 0] = getDeterminant(2, 0);
            //Minors[2, 1] = getDeterminant(2, 1);
            //Minors[2, 2] = getDeterminant(2, 2);

            //double[,] coFactors = new double[3, 3];
            //coFactors[0, 0] = Minors[0, 0];
            //coFactors[0, 1] = -Minors[0, 1];
            //coFactors[0, 2] = Minors[0, 2];
            //coFactors[1, 0] = -Minors[1, 0];
            //coFactors[1, 1] = Minors[1, 1];
            //coFactors[1, 2] = -Minors[1, 2];
            //coFactors[2, 0] = Minors[2, 0];
            //coFactors[2, 1] = -Minors[2, 1];
            //coFactors[2, 2] = Minors[2, 2];

            ////adjugate
            //double[,] adjugate = new double[3, 3];
            //adjugate[0, 0] = coFactors[0, 0];
            //adjugate[0, 1] = coFactors[1, 0];
            //adjugate[0, 2] = coFactors[2, 0];
            //adjugate[1, 0] = coFactors[0, 1];
            //adjugate[1, 1] = coFactors[1, 1];
            //adjugate[1, 2] = coFactors[2, 1];
            //adjugate[2, 0] = coFactors[0, 2];
            //adjugate[2, 1] = coFactors[1, 2];
            //adjugate[2, 2] = coFactors[2, 2];

            //double det = positionVectorForwardMatrix[0, 0] * coFactors[0, 0] + positionVectorForwardMatrix[0, 1] * coFactors[0, 1] + positionVectorForwardMatrix[0, 2] * coFactors[0, 2];

            //positionVectorReverseMatrix = new double[3, 3];
            //positionVectorReverseMatrix[0, 0] = (adjugate[0, 0] / det);
            //positionVectorReverseMatrix[0, 1] = (adjugate[0, 1] / det);
            //positionVectorReverseMatrix[0, 2] = (adjugate[0, 2] / det);
            //positionVectorReverseMatrix[1, 0] = (adjugate[1, 0] / det);
            //positionVectorReverseMatrix[1, 1] = (adjugate[1, 1] / det);
            //positionVectorReverseMatrix[1, 2] = (adjugate[1, 2] / det);
            //positionVectorReverseMatrix[2, 0] = (adjugate[2, 0] / det);
            //positionVectorReverseMatrix[2, 1] = (adjugate[2, 1] / det);
            //positionVectorReverseMatrix[2, 2] = (adjugate[2, 2] / det);


            this.positionVectorReverseMatrix = new double[3, 3];
            this.positionVectorReverseMatrix[0, 0] = 1.0;
            this.positionVectorReverseMatrix[0, 1] = rZrad;
            this.positionVectorReverseMatrix[0, 2] = -rYrad;

            this.positionVectorReverseMatrix[1, 0] = -rZrad;
            this.positionVectorReverseMatrix[1, 1] = 1;
            this.positionVectorReverseMatrix[1, 2] = rXrad;

            this.positionVectorReverseMatrix[2, 0] = rYrad;
            this.positionVectorReverseMatrix[2, 1] = -rXrad;
            this.positionVectorReverseMatrix[2, 2] = 1;
        }


        double getDeterminant(int y, int x)
        {
            int toprow;
            int bottomrow;
            int leftcolumn;
            int rightcolumn;
            if (x == 0)
            {
                leftcolumn = 1;
                rightcolumn = 2;
            }
            else if (x == 1)
            {
                leftcolumn = 0;
                rightcolumn = 2;
            }
            else
            {
                leftcolumn = 0;
                rightcolumn = 1;
            }

            if (y == 0)
            {
                toprow = 1;
                bottomrow = 2;
            }
            else if (y == 1)
            {
                toprow = 0;
                bottomrow = 2;
            }
            else
            {
                toprow = 0;
                bottomrow = 1;
            }

            return positionVectorForwardMatrix[toprow, leftcolumn] * positionVectorForwardMatrix[bottomrow, rightcolumn] - positionVectorForwardMatrix[toprow, rightcolumn] * positionVectorForwardMatrix[bottomrow, leftcolumn];
        }
    }
}
  

