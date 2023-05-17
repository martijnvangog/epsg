
using System;

using UnityEngine;


namespace epsg.operationMethods
{
    [System.Serializable]
    public class GeographicToGeocentricSettings:OperationSettings
    {


        [SerializeField] double _primeMeridian;
        public double primeMeridian { get { return _primeMeridian; } set { _primeMeridian = value; } }
        [SerializeField] double _semiMajorAxis;
        public double semiMajorAxis { get { return _semiMajorAxis; } set { _semiMajorAxis = value; } }
        [SerializeField] double _inverseFlattening;
        public double inverseFlattening { get { return _inverseFlattening; } set { _inverseFlattening = value; } }

        //derived variables
        double _flattening;
        public double flattening
        {
            get
            {
                if (_flattening == default(double))
                {
                    _flattening =1/inverseFlattening;
                }
                return _flattening;
            }
        }

        double _eccentricity;
        public double eccentricity
        {
            get {
                if (_eccentricity==default(double))
                {
                    _eccentricity = Math.Sqrt(2 * (1 / inverseFlattening) - Math.Pow(1 / inverseFlattening, 2));
                }
                return _eccentricity; 
            }
        }

        double _eta;
        public double eta
        {
            get {
                if (_eta == default(double))
                {
                    _eta = Math.Pow(eccentricity, 2) / (1 - Math.Pow(eccentricity, 2));
                }
                return _eta;
            }

        }

        double _b;
        public double b
        {
            get
            {
                if (_b == default)
                {
                    _b = semiMajorAxis * (1 - flattening);
                }
                return _b;
            }

        }

        
    }
}
