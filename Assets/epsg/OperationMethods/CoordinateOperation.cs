

namespace epsg.operationMethods
{
    [System.Serializable]
    public struct CoordinateOperation
    {
        public OperationMethodEnum operationMethod;
        
        public HelmertSettings HelmertSettings;
        public NullOperationSettings nullOperationSettings;
        public SetAxisvalueSettings addDimensionSettings;
        public GeographicToGeocentricSettings geog2GeocSettings;
        public ElevationgridSettings elevationGridSettings;
        public GridshiftSettings gridShiftSettings;
        

        public Vector3Any Apply(Vector3Any coordinate,OperationDirection direction)
        {
            Vector3Any result = new Vector3Any();
            switch (operationMethod)
            {
                case OperationMethodEnum.Helmert7:
                    result = Helmert.Apply(coordinate, HelmertSettings, direction);
                    break;
                case OperationMethodEnum.null_operation:
                    result = coordinate ;
                    break;
                case OperationMethodEnum.SetAxisvalue:
                    result = SetAxisvalue.Apply(coordinate, addDimensionSettings, direction);
                    break;
                case OperationMethodEnum.GeographicToGeocentric:
                    if (direction == OperationDirection.Forward)
                    {
                        result = GeographicToGeocentric.Forward(coordinate, geog2GeocSettings);
                    }
                    else
                    {
                        result = GeographicToGeocentric.Reverse(coordinate, geog2GeocSettings);
                    }
                    break;
                case OperationMethodEnum.SetAsideElevation:
                    result = ElevationGrid.SetAsideElevation(coordinate, direction);
                    break;
                case OperationMethodEnum.ElevationGridOffset:
                    result = ElevationGrid.ApplyElevationGrid(coordinate, direction,elevationGridSettings);
                    break;
                case OperationMethodEnum.GridShift:
                    result = GridShift.Apply(coordinate, direction, gridShiftSettings);
                    break;
                default:
                    break;
            }
            return result;
            
        }
    }

}
