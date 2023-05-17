

namespace epsg.operationMethods
{
    [System.Serializable]
    public struct CoordinateOperation
    {
        public OperationMethodEnum operationMethod;
        
        public HelmertSettings HelmertSettings;
        public NullOperationSettings nullOperationSettings;
        public AddDimensionSettings addDimensionSettings;
        public GeographicToGeocentricSettings geog2GeocSettings;
        
        

        public Vector3Any Apply(Vector3Any coordinate,OperationDirection direction)
        {
            Vector3Any result = new Vector3Any();
            switch (operationMethod)
            {
                case OperationMethodEnum.Helmert7:
                    result = Helmert.Apply(coordinate, HelmertSettings, direction);
                    break;
                case OperationMethodEnum.null_operation:
                    break;
                case OperationMethodEnum.AddDimension:
                    result = AddDimension.Apply(coordinate, addDimensionSettings, direction);
                    break;
                case OperationMethodEnum.GeographicToGeocentric:
                    break;
                default:
                    break;
            }
            return result;
            
        }
    }

}
