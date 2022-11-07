using System;

public class Ellipsoid
{
    public double semimajorAxis;
    public double inverseFlattening;
    public double eccentricity;
    public double flattening;
    public Ellipsoid(double semimajorAxis,double inverseFlattening)
    {
        this.semimajorAxis = semimajorAxis;
        this.inverseFlattening = inverseFlattening;
        flattening = 1 / inverseFlattening;
        eccentricity = Math.Sqrt(2 * flattening - Math.Pow(flattening, 2));
    }
}


