using epsg.operationMethods;



namespace epsg.tranformations
{
    [System.Serializable]
    public struct transformationStep 
    {
        public OperationDirection direction;
        public CoordinateOperation operation;

        public Vector3Any Apply(Vector3Any coordinate)
        {
            return operation.Apply(coordinate, direction);
        }
    }
}
