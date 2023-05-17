using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using epsg.coordinatesystems;

namespace epsg
{
    [CreateAssetMenu(fileName = "new CoordinateTransformation", menuName = "epsg/CoordinateSystem", order = 1)]
    public class CoordinateSystem : ScriptableObject
    {
        public int epsgCode;
        public new string name;
        [SerializeField]
        CoordinateSystemType _coordinateSystemType;
        
        public CoordinateSystemType coordinateSystemType
        {
            get { return _coordinateSystemType; }
            set { _coordinateSystemType = value;
                switch (_coordinateSystemType)
                {
                    case CoordinateSystemType.undefined:
                        Axis1 = new Axis("rechts","x",Direction.East,Unit.Meter);
                        Axis2 = new Axis("vooruit", "y", Direction.North, Unit.Meter);
                        Axis3 = new Axis("omhoog", "z", Direction.Up, Unit.Meter);
                        break;
                    case CoordinateSystemType.vertical1D:
                        Axis1 = new Axis("up", "z", Direction.Up, Unit.Meter);
                        break;
                    case CoordinateSystemType.cartesian2D:
                        Axis1 = new Axis("East", "x", Direction.East, Unit.Meter);
                        Axis2 = new Axis("North", "y", Direction.North, Unit.Meter);
                        break;
                    case CoordinateSystemType.cartesian3D:
                        Axis1 = new Axis("East", "x", Direction.East, Unit.Meter);
                        Axis2 = new Axis("North", "y", Direction.North, Unit.Meter);
                        Axis3 = new Axis("up", "z", Direction.Up, Unit.Meter);
                        break;
                    case CoordinateSystemType.ellipsoidal2D:
                        Axis1 = new Axis("Lattitude", "Lat", Direction.North, Unit.Degree);
                        Axis2 = new Axis("Longitude", "Lon", Direction.East, Unit.Degree);
                        break;
                    case CoordinateSystemType.ellipsoidal3D:
                        Axis1 = new Axis("Lattitude", "Lat", Direction.North, Unit.Degree);
                        Axis2 = new Axis("Longitude", "Lon", Direction.East, Unit.Degree);
                        Axis3 = new Axis("up", "z", Direction.Up, Unit.Meter);
                        break;
                    case CoordinateSystemType.geocentric:
                        Axis1 = new Axis("Geocentric X", "X", Direction.GeodeticX, Unit.Meter);
                        Axis2 = new Axis("Geocentric Y", "Y", Direction.GeodeticY, Unit.Meter);
                        Axis3 = new Axis("Geocentric Z", "Z", Direction.GeodeticZ, Unit.Meter);
                        break;
                    default:
                        break;
                }

            }
        }


        public Axis Axis1;
        public Axis Axis2;
        public Axis Axis3;
        
        public int AxisCount()
        {
            
            switch (coordinateSystemType)
            {
                case CoordinateSystemType.undefined:
                    return 3;
                case CoordinateSystemType.vertical1D:
                    return 1;
                case CoordinateSystemType.cartesian2D:
                    return 2;
                case CoordinateSystemType.cartesian3D:
                    return 3;
                case CoordinateSystemType.ellipsoidal2D:
                    return 2;
                case CoordinateSystemType.ellipsoidal3D:
                    return 3;
                case CoordinateSystemType.geocentric:
                    return 3;
                default:
                    return 0;
            }
        }

    }


}
