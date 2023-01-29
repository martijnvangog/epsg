using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace epsg
{
public struct ProjectionName
    {
        public int epsgCode { get; }
        public string name { get; }

        
        

        public ProjectionName(int epsgCode, string name,Projection projection)
        {
            this.epsgCode = epsgCode;
            this.name = name;
            if (!epsg.usedProjections.ContainsKey(epsgCode))
            {
                epsg.usedProjections.Add(epsgCode, projection);
            }
           
            
        }
    }
}
