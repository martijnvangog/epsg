using System;
public abstract class CRS
{
    public CoordinateReferenceSystem crsEnum;
    public Ellipsoid ellipsoid;
    public double[] toWGS84;

    decimal[,] rotationMatrix;
    double tX ;
    double tY ;
    double tZ ;
    double M;
    decimal[,] reverseRotationMatrix;

    public Vector3LatLong ToEllipsoidal(Vector3GeoCentric coordinate)
    {
        double eta = Math.Pow(ellipsoid.eccentricity, 2) / (1 - Math.Pow(ellipsoid.eccentricity, 2));
        double b = ellipsoid.semimajorAxis * (1 - ellipsoid.flattening);
        double p = Math.Sqrt(Math.Pow(coordinate.x, 2) + Math.Pow(coordinate.y, 2));
        double q = Math.Atan2((coordinate.z * ellipsoid.semimajorAxis), p * b);

        double lattitude = Math.Atan2((coordinate.z + eta * b * Math.Pow(Math.Sin(q), 3)), p - Math.Pow(ellipsoid.eccentricity, 2) * ellipsoid.semimajorAxis * Math.Pow(Math.Cos(q), 3));
        double longitude = Math.Atan2(coordinate.y, coordinate.x);
        double primeVerticalRadius = ellipsoid.semimajorAxis / (Math.Sqrt(1 - (Math.Pow(ellipsoid.eccentricity, 2) * Math.Pow(Math.Sin(lattitude), 2))));
        double height = (p / Math.Cos(lattitude)) - primeVerticalRadius;
        return new Vector3LatLong(lattitude * 180 / Math.PI, longitude * 180 / Math.PI, height, crsEnum);
    }

    public Vector3GeoCentric ToGeoCentric(Vector3LatLong coordinate)
    {
        double lattitude = coordinate.lattitude * Math.PI / 180;
        double longitude = coordinate.longitude * Math.PI / 180;
        //EPSG datset coordinate operation method code 9602)
        double primeVerticalRadius = ellipsoid.semimajorAxis / (Math.Sqrt(1 - (Math.Pow(ellipsoid.eccentricity, 2) * Math.Pow(Math.Sin(lattitude), 2))));
        double X = (primeVerticalRadius + coordinate.ellipsoidalHeight) * Math.Cos(lattitude) * Math.Cos(longitude);
        double Y = (primeVerticalRadius + coordinate.ellipsoidalHeight) * Math.Cos(lattitude) * Math.Sin(longitude);
        double Z = ((1 - Math.Pow(ellipsoid.eccentricity, 2)) * primeVerticalRadius + coordinate.ellipsoidalHeight) * Math.Sin(lattitude);
        return new Vector3GeoCentric(X, Y, Z, crsEnum);
    }

    public Vector3GeoCentric ToWGS84GeoCentric(Vector3GeoCentric coordinate)
    {
        if (rotationMatrix is null)
        {
            SetupMatrices();
        }
        double X =(double)((decimal)coordinate.x * rotationMatrix[0, 0] + (decimal)coordinate.y * rotationMatrix[0,1] + (decimal)coordinate.z * rotationMatrix[0,2]) * M + tX;
        double Y = (double)((decimal)coordinate.x * rotationMatrix[1,0] + (decimal)coordinate.y * rotationMatrix[1, 1] + (decimal)coordinate.z * rotationMatrix[1,2]) * M + tY;
        double Z = (double)((decimal)coordinate.x * rotationMatrix[2,0] + (decimal)coordinate.y * rotationMatrix[2,1] + (decimal)coordinate.z * rotationMatrix[2, 2]) * M + tZ;
        return new Vector3GeoCentric(X,Y,Z);
    }
    public Vector3GeoCentric FromWGS84GeoCentric(Vector3GeoCentric coordinate)
    {
        if (rotationMatrix is null)
        {
            SetupMatrices();
        }
        coordinate.x = (coordinate.x - tX);
        coordinate.y = (coordinate.y - tY);
        coordinate.z = (coordinate.z - tZ);
        double X = (double)((decimal)coordinate.x * reverseRotationMatrix[0, 0] + (decimal)coordinate.y * reverseRotationMatrix[0,1] + (decimal)coordinate.z * reverseRotationMatrix[0,2]) / M;
        double Y = (double)((decimal)coordinate.x * reverseRotationMatrix[1,0] + (decimal)coordinate.y * reverseRotationMatrix[1, 1] + (decimal)coordinate.z * reverseRotationMatrix[1,2]) / M;
        double Z = (double)((decimal)coordinate.x * reverseRotationMatrix[2,0] + (decimal)coordinate.y * reverseRotationMatrix[2,1] + (decimal)coordinate.z * reverseRotationMatrix[2, 2]) / M;
        return new Vector3GeoCentric(X, Y, Z,crsEnum);
    }

    void SetupMatrices()
    {
        tX = toWGS84[0];
        tY = toWGS84[1];
        tZ = toWGS84[2];
        decimal rX = (decimal)toWGS84[3] / (decimal)Math.Pow(10, 6);
        decimal rY = (decimal)toWGS84[4] / (decimal)Math.Pow(10, 6);
        decimal rZ = (decimal)toWGS84[5] / (decimal)Math.Pow(10, 6);
        decimal dS = (Decimal)toWGS84[6] / (decimal)Math.Pow(10, 6);
        M = 1 + (double)dS;


        rotationMatrix = new decimal[3, 3];
        rotationMatrix[0, 0] = 1.0m;
        rotationMatrix[0, 1] = rZ;
        rotationMatrix[0, 2] = -rY;
        rotationMatrix[1, 0] = -rZ;
        rotationMatrix[1, 1] = 1.0m;
        rotationMatrix[1, 2] = rX;
        rotationMatrix[2, 0] = rY;
        rotationMatrix[2, 1] = -rX;
        rotationMatrix[2, 2] = 1.0m;


        //find the inverse
        decimal[,] Minors = new decimal[3, 3];
        Minors[0, 0] = getDeterminant(0,0);
        Minors[0, 1] = getDeterminant(0, 1);
        Minors[0, 2] = getDeterminant(0, 2);
        Minors[1, 0] = getDeterminant(1, 0);
        Minors[1, 1] = getDeterminant(1, 1);
        Minors[1, 2] = getDeterminant(1, 2);
        Minors[2, 0] = getDeterminant(2, 0);
        Minors[2, 1] = getDeterminant(2, 1);
        Minors[2, 2] = getDeterminant(2, 2);

        decimal[,] coFactors = new decimal[3, 3];
        coFactors[0, 0] = Minors[0, 0];
        coFactors[0, 1] = -Minors[0, 1];
        coFactors[0, 2] = Minors[0, 2];
        coFactors[1, 0] = -Minors[1, 0];
        coFactors[1, 1] = Minors[1, 1];
        coFactors[1, 2] = -Minors[1, 2];
        coFactors[2, 0] = Minors[2, 0];
        coFactors[2, 1] = -Minors[2, 1];
        coFactors[2, 2] = Minors[2, 2];

        //adjugate
        decimal[,] adjugate = new decimal[3, 3];
        adjugate[0, 0] = coFactors[0, 0];
        adjugate[0, 1] = coFactors[1, 0];
        adjugate[0, 2] = coFactors[2, 0];
        adjugate[1, 0] = coFactors[0, 1];
        adjugate[1, 1] = coFactors[1, 1];
        adjugate[1, 2] = coFactors[2, 1];
        adjugate[2, 0] = coFactors[0, 2];
        adjugate[2, 1] = coFactors[1, 2];
        adjugate[2, 2] = coFactors[2, 2];

        // calculate determinant
        decimal det = (decimal)rotationMatrix[0, 0] * coFactors[0, 0] + (decimal)rotationMatrix[0, 1] * coFactors[0, 1] + (decimal)rotationMatrix[0, 2] * coFactors[0, 2];

        reverseRotationMatrix = new decimal[3, 3];
        reverseRotationMatrix[0, 0] = (adjugate[0, 0] / det);
        reverseRotationMatrix[0, 1] = (adjugate[0, 1] / det);
        reverseRotationMatrix[0, 2] = (adjugate[0, 2] / det);
        reverseRotationMatrix[1, 0] = (adjugate[1, 0] / det);
        reverseRotationMatrix[1, 1] = (adjugate[1, 1] / det);
        reverseRotationMatrix[1, 2] = (adjugate[1, 2] / det);
        reverseRotationMatrix[2, 0] = (adjugate[2, 0] / det);
        reverseRotationMatrix[2, 1] = (adjugate[2, 1] / det);
        reverseRotationMatrix[2, 2] = (adjugate[2, 2] / det);

    }

    decimal getDeterminant(int y, int x)
    {
        int toprow;
        int bottomrow;
        int leftcolumn;
        int rightcolumn;
        if (x==0)
        {
            leftcolumn = 1;
            rightcolumn = 2;
        }
        else if (x==1)
        {
            leftcolumn = 0;
            rightcolumn = 2;
        }
        else
        {
            leftcolumn = 0;
            rightcolumn = 1;
        }

        if (y==0)
        {
            toprow = 1;
            bottomrow = 2;
        }
        else if (y==1)
        {
            toprow = 0;
            bottomrow = 2;
        }
        else
        {
            toprow = 0;
            bottomrow = 1;
        }

        return (decimal)rotationMatrix[toprow, leftcolumn] * (decimal)rotationMatrix[bottomrow, rightcolumn] - (decimal)rotationMatrix[toprow, rightcolumn] * (decimal)rotationMatrix[bottomrow, leftcolumn];
    }
}

