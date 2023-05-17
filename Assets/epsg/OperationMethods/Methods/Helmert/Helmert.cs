
namespace epsg.operationMethods
{

    public static class Helmert
    {

        public static Vector3Any Apply(Vector3Any coordinate, HelmertSettings settings, OperationDirection direction)
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
        static Vector3Any Forward(Vector3Any coordinate, HelmertSettings settings)
        {
            if (settings.positionVectorForwardMatrix is null)
            {
                settings.SetupMatrices();
            }
            double m = settings.M;
            Vector3Any result = new Vector3Any();
            result.value1 = m*(coordinate.value1*settings.positionVectorForwardMatrix[0,0] + coordinate.value2 * settings.positionVectorForwardMatrix[0, 1] + coordinate.value3 * settings.positionVectorForwardMatrix[0, 2]) + settings.tX;
            result.value2 = m*(coordinate.value1 * settings.positionVectorForwardMatrix[1, 0] + coordinate.value2 * settings.positionVectorForwardMatrix[1, 1] + coordinate.value3 * settings.positionVectorForwardMatrix[1, 2]) + settings.tY;
            result.value3 = m*(coordinate.value1 * settings.positionVectorForwardMatrix[2, 0] + coordinate.value2 * settings.positionVectorForwardMatrix[2, 1] + coordinate.value3 * settings.positionVectorForwardMatrix[2, 2]) + settings.tZ;
            return result;
        }

        static Vector3Any Reverse(Vector3Any coordinate, HelmertSettings settings)
        {
            if (settings.positionVectorReverseMatrix is null)
            {
                settings.SetupMatrices();
            }
            double m = settings.M;
            coordinate.value1 -= settings.tX;
            coordinate.value2 -= settings.tY;
            coordinate.value3 -= settings.tZ;
            Vector3Any result = new Vector3Any();
            result.value1 = (coordinate.value1 * settings.positionVectorReverseMatrix[0, 0] + coordinate.value2 * settings.positionVectorReverseMatrix[0, 1] + coordinate.value3 * settings.positionVectorReverseMatrix[0, 2])/m ;
            result.value2 = (coordinate.value1 * settings.positionVectorReverseMatrix[1, 0] + coordinate.value2 * settings.positionVectorReverseMatrix[1, 1] + coordinate.value3 * settings.positionVectorReverseMatrix[1, 2])/m ;
            result.value3 = (coordinate.value1 * settings.positionVectorReverseMatrix[2, 0] + coordinate.value2 * settings.positionVectorReverseMatrix[2, 1] + coordinate.value3 * settings.positionVectorReverseMatrix[2, 2])/m ;
            return result;
        }
    }
}
