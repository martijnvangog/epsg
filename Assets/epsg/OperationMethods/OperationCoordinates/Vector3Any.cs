
namespace epsg.operationMethods
{
    [System.Serializable]
    public struct Vector3Any
    {
        public double value1; //north / forward geodeticX
        public double value2; //east, right / geodeticY
        public double value3; //up, ellipsoidalheight/geodeticZ

        public Vector3Any(double value1, double value2, double value3)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

    }
}
