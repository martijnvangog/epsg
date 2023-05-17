using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg.coordinatesystems;

namespace epsg
{
    [System.Serializable]
    public struct RealWorldCoordinate
    {
        public CoordinateSystem coordinateSystem;
        public double value1;
        public double value2;
        public double value3;

        public RealWorldCoordinate(CoordinateSystem coordinateSystem)
        {
            this.coordinateSystem = coordinateSystem;
            value1 = double.NaN;
            value2 = double.NaN;
            value3 = double.NaN;
        }
        public RealWorldCoordinate(CoordinateSystem coordinateSystem, double value1, double value2)
        {
            this.coordinateSystem = coordinateSystem;
            this.value1 = value1;
            this.value2 = value2;
            value3 = double.NaN;
        }
        public RealWorldCoordinate(CoordinateSystem coordinateSystem, double value1, double value2, double value3)
        {
            this.coordinateSystem = coordinateSystem;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        public double GetValue(Direction axisdirection)
        {
            if (coordinateSystem.Axis1.direction==axisdirection)
            {
                return value1;
            }
            if (coordinateSystem.Axis2.direction == axisdirection)
            {
                return value2;
            }
            if (coordinateSystem.Axis3.direction == axisdirection)
            {
                return value3;
            }
            return 0;
        }

        //public RealWorldCoordinate TransformTo(CoordinateSystem targetCoordinateSystem)
        //{
        //    RealWorldCoordinate coordinate = new RealWorldCoordinate(coordinateSystem);

        //    return coordinate;
        //}
        //public RealWorldCoordinate TransformTo(CoordinateSystem targetCoordinateSystem, out bool succes)
        //{
        //    RealWorldCoordinate coordinate = new RealWorldCoordinate(coordinateSystem);

        //    succes = false;
        //    return coordinate;
        //}
    }
}
