
namespace epsg.operationMethods
{
    [System.Serializable]
    public struct Vector3Any
    {
        public double value1; //north / forward geodeticX
        public double value2; //east, right / geodeticY
        public double value3; //up, ellipsoidalheight/geodeticZ

        public Vector3Any(double value1=double.NaN, double value2=double.NaN, double value3=double.NaN)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }
        public static Vector3Any operator -(Vector3Any a, Vector3Any b)
        {
            Vector3Any result = new Vector3Any();
            if (!double.IsNaN(a.value1) && !double.IsNaN(b.value1))
            {
                result.value1 = a.value1-b.value1;
            }
            if (!double.IsNaN(a.value2) && !double.IsNaN(b.value2))
            {
                result.value2 = a.value2 - b.value2;
            }
            if (!double.IsNaN(a.value3) && !double.IsNaN(b.value3))
            {
                result.value3 = a.value3 - b.value3;
            }
            return result;
        }
        public static Vector3Any operator +(Vector3Any a, Vector3Any b)
        {
            Vector3Any result = new Vector3Any();
            if (!double.IsNaN(a.value1) && !double.IsNaN(b.value1))
            {
                result.value1 = a.value1 + b.value1;
            }
            if (!double.IsNaN(a.value2) && !double.IsNaN(b.value2))
            {
                result.value2 = a.value2 + b.value2;
            }
            if (!double.IsNaN(a.value3) && !double.IsNaN(b.value3))
            {
                result.value3 = a.value3 + b.value3;
            }
            return result;
        }


    }
}
