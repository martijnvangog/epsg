
namespace epsg.operationMethods
{

    public static class AddDimension
    {

        public static Vector3Any Apply(Vector3Any coordinate, AddDimensionSettings settings, OperationDirection direction)
        {
            switch (direction)
            {
                case OperationDirection.Forward:
                    return Forward(coordinate, settings);
                case OperationDirection.Reverse:
                    return Reverse(coordinate, settings);
                default:
                    return new Vector3Any();
            }
        }

        static Vector3Any Forward(Vector3Any coordinate, AddDimensionSettings settings)
        {
            if (settings.AxisNumber==3)
            {
                coordinate.value3 = settings.defaultValue;
                return coordinate;
            }
            if (settings.AxisNumber==2)
            {
                
                coordinate.value2 = settings.defaultValue;
                return coordinate;
            }
                coordinate.value1 = settings.defaultValue;
                return coordinate;
        }

        static Vector3Any Reverse(Vector3Any coordinate, AddDimensionSettings settings)
        {
            if (settings.AxisNumber==3)
            {
                coordinate.value3 = settings.defaultValue;
                return coordinate;
            }
            if (settings.AxisNumber==2)
            {
                coordinate.value2 = settings.defaultValue;
                return coordinate;
            }
            coordinate.value1 = settings.defaultValue;
            return coordinate;
        }
    }
}