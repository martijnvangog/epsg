namespace epsg.coordinatesystems
{
    [System.Serializable]
    public struct Axis
    {
        public string name;
        public string abbreviation;
        public Direction direction;
        public Unit Unit;
        public Axis(string name, string abbreviation, Direction direction, Unit unit)
        {
            this.name = name;
            this.abbreviation = abbreviation;
            this.direction = direction;
            this.Unit = unit;
        }
    }
}
