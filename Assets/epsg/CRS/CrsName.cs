namespace epsg
{
    public struct CrsName
    {
        public int epsgCode { get; }
        public string name { get; }

        
        public CrsName(int epsgCode, string name, crs.CRS crs)
        {
            this.epsgCode = epsgCode;
            this.name = name;
            if (!epsg.usedCRSs.ContainsKey(epsgCode))
            {
                epsg.usedCRSs.TryAdd(epsgCode, crs);
            }
            
                    }
        
    }
}
