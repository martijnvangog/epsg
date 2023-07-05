using epsg.operationMethods;
using System.Collections.Generic;
using epsg.coordinatesystems;
using epsg.tranformations;


namespace epsg
{
    public static class Coordinates
    {
        static List<Transformation> transformationPaths = new List<Transformation>();

        static Transformation currentTransformationPath;

        static List<CoordinateSystem> coordinateSystems = new List<CoordinateSystem>();

        public static void AddTransformations(List<Transformation> transformations)
        {
            for (int i = 0; i < transformations.Count; i++)
            {
                Transformation newPath = new Transformation();
                newPath.from = transformations[i].from;
                newPath.to = transformations[i].to;
                newPath.operations = transformations[i].operations;
                transformationPaths.Add(newPath);
                transformationPaths.Add(reverseTransformation(newPath));

                if (coordinateSystems.Contains(transformations[i].from)==false)
                {
                    coordinateSystems.Add(transformations[i].from);
                }
                if (coordinateSystems.Contains(transformations[i].to) == false)
                {
                    coordinateSystems.Add(transformations[i].to);
                }
            }
            currentTransformationPath = transformationPaths[0];
            int foundpaths = 1;
            while (foundpaths > 0)
            {
                foundpaths = CombinePaths();
            }
        }

        static Transformation reverseTransformation(Transformation originalPath)
        {
            Transformation result = new Transformation();
            result.from = originalPath.to;
            result.to = originalPath.from;
            result.operations = new List<transformationStep>();
            for (int i = originalPath.operations.Count - 1; i >= 0; i--)
            {
                transformationStep step = originalPath.operations[i];
                if (step.direction == OperationDirection.Forward)
                {
                    step.direction = OperationDirection.Reverse;
                }
                else
                {
                    step.direction = OperationDirection.Forward;
                }
                result.operations.Add(step);
            }
            return result;
        }

        static int CombinePaths()
        {
            
            List<transformationStep> path1Steps;
            List<transformationStep> path2Steps;
            int combinedPaths = 0;
            for (int startindex = 0; startindex < transformationPaths.Count-1; startindex++)
            {

           
            for (int i = startindex+1; i < transformationPaths.Count; i++)
            {
                if (TransformationPathsConnect(transformationPaths[startindex],transformationPaths[i]))
                {
                    Transformation newPath = new Transformation();
                    newPath.from = transformationPaths[startindex].from;
                    newPath.to = transformationPaths[i].to;
                    path1Steps = transformationPaths[startindex].operations;
                    path2Steps = transformationPaths[i].operations;
                    newPath.operations = new List<transformationStep>();
                    for (int j = 0; j < path1Steps.Count; j++)
                    {
                        newPath.operations.Add(path1Steps[j]);
                    }
                    for (int j = 0; j < path2Steps.Count; j++)
                    {
                        newPath.operations.Add(path2Steps[j]);
                    }
                    transformationPaths.Add(newPath);
                        
                }
            }
            }
            return combinedPaths;
        }

        static bool TransformationPathsConnect(Transformation path1, Transformation path2)
        {
            
            //check if path1-endpoint matches with path2-startpoint
            if (path1.to != path2.from) 
            {
                return false;
            }
            // check if combining the paths results in a roundtrip;
            if (path1.from == path2.to) 
            {
                return false;
            }
            // check if combined path is already defined
            for (int i = 0; i < transformationPaths.Count; i++)
            {
                if (transformationPaths[i].from == path1.from && transformationPaths[i].to == path2.to)
                {
                    return false;
                }
            }
            return true;
        }

        static void SetTransformationPath(CoordinateSystem from,CoordinateSystem to)
        {
            if (currentTransformationPath.from != from || currentTransformationPath.to !=to)
            {
                for (int i = 0; i < transformationPaths.Count; i++)
                {
                    if (transformationPaths[i].from==from && transformationPaths[i].to == to)
                    {
                        currentTransformationPath = transformationPaths[i];
                        return;
                    }
                }
            }
        }

        public static RealWorldCoordinate Transform(RealWorldCoordinate coordinate, CoordinateSystem targetCoordinateSystem)
        {
            SetTransformationPath(coordinate.coordinateSystem, targetCoordinateSystem);
            //TODO: when creating tempcoordinate, look at coordinateSystem to make sure the values are copied in the right order
            // i.e. is the first value in the tempcoordinate must be X or lat
            // the order is not always the same for different coordaintesystems
            Vector3Any tempCoordinate = new Vector3Any(coordinate.value1,coordinate.value2,coordinate.value3);
            for (int i = 0; i < currentTransformationPath.operations.Count; i++)
            {
                tempCoordinate = currentTransformationPath.operations[i].Apply(tempCoordinate);
            }

            //TODO: make sure the values of the resulting coordinate are assigned in the riggth order
            return new RealWorldCoordinate(targetCoordinateSystem, tempCoordinate.value1, tempCoordinate.value2, tempCoordinate.value3);
            
        }
        
    }
 
}
