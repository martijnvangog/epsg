
using System.Collections.Generic;


namespace epsg.tranformations
{
    [System.Serializable]
    public class Transformation 
    {
        public CoordinateSystem from;
        public CoordinateSystem to;

        public List<transformationStep> operations = new List<transformationStep>();
        
    }
}
