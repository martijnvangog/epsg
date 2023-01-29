using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace epsg
{
    public class NTv2
    {
        // Start is called before the first frame update
        byte[] griddata;
        [Header("grid data")]
        public FileHeader fileHeader;
        public Grid grid;

        [Header("search grids")]
        public Vector3LatLong coordinate;
        public Grid FoundGrid;
        public Vector3LatLong pointoffset;
        void Start()
        {
            readFile("rdcorr2018");
            FindGridFile(coordinate);
            pointoffset = transformPoint(coordinate, transformationDirection.Forward);
        }

        public void readFile(string fileName)
        {
            griddata = Resources.Load<TextAsset>(fileName).bytes;
            fileHeader = ReadFileHeader();
            int startindex = 176;
            grid = new Grid();
            grid.SUB_NAME = "NONE    ";

            for (int i = 0; i < fileHeader.NUM_FILE; i++)
            {

                Grid newGrid = readGrid(ref startindex);

                grid.Add(newGrid);


            }
            //checkForEndOfFile
            string EOFstring = Encoding.UTF8.GetString(griddata, startindex, 4);

        }

        FileHeader ReadFileHeader()
        {
            FileHeader result = new FileHeader();
            result.NUM_OREC = BitConverter.ToInt32(griddata, 8);
            result.NUM_SREC = BitConverter.ToInt32(griddata, 24);
            result.NUM_FILE = BitConverter.ToInt32(griddata, 40);
            result.GS_TYPE = Encoding.UTF8.GetString(griddata, 56, 8);
            result.VERSION = Encoding.UTF8.GetString(griddata, 72, 8);
            result.SYSTEM_F = Encoding.UTF8.GetString(griddata, 88, 8);
            result.SYSTEM_T = Encoding.UTF8.GetString(griddata, 104, 8);
            result.MAJOR_F = BitConverter.ToDouble(griddata, 120);
            result.MINOR_F = BitConverter.ToDouble(griddata, 136);
            result.MAJOR_T = BitConverter.ToDouble(griddata, 152);
            result.MINOR_T = BitConverter.ToDouble(griddata, 168);
            return result;

        }

        Grid readGrid(ref int startposition)
        {
            Grid result = new Grid();
            startposition += 8;
            result.SUB_NAME = Encoding.UTF8.GetString(griddata, startposition, 8);
            startposition += 16;
            result.PARENT = Encoding.UTF8.GetString(griddata, startposition, 8);
            startposition += 16;
            result.CREATED = Encoding.UTF8.GetString(griddata, startposition, 8);
            startposition += 16;
            result.UPDATED = Encoding.UTF8.GetString(griddata, startposition, 8);
            startposition += 16;
            result.S_LAT = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.N_LAT = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.E_LONG = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.W_LONG = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.LAT_INC = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.LONG_INC = BitConverter.ToDouble(griddata, startposition);
            startposition += 16;
            result.GS_COUNT = BitConverter.ToInt32(griddata, startposition);
            startposition += 8;
            result.datastart = startposition;
            startposition += (16 * result.GS_COUNT);

            return result;
        }

        void FindGridFile(Vector3LatLong point)
        {
            FoundGrid = grid.getGridWithPoint(point);
        }

        public Vector3LatLong transformPoint(Vector3LatLong point, transformationDirection direction)
        {
            Vector3LatLong result = new Vector3LatLong();
            if (direction == transformationDirection.Forward)
            {
                Vector3LatLong offset = GetOffset(point);
                result.lattitude = point.lattitude + offset.lattitude;
                result.longitude = point.longitude + offset.longitude;
                return result;
            }

            Vector3LatLong inPoint = point;
            Vector3LatLong outpoint = new Vector3LatLong();


            for (int i = 0; i < 5; i++)
            {
                Vector3LatLong inpointOffset = GetOffset(inPoint);
                outpoint.lattitude = inPoint.lattitude + inpointOffset.lattitude;
                outpoint.longitude = inPoint.longitude + inpointOffset.longitude;
                Vector3LatLong offset = new Vector3LatLong();
                offset.lattitude = point.lattitude - outpoint.lattitude;
                offset.longitude = point.longitude - outpoint.longitude;
                if (Math.Abs(offset.lattitude) < 0.0000003 && Math.Abs(offset.longitude) < 0.0000003)
                {
                    i = 5;
                }
                inPoint.lattitude += offset.lattitude;
                inPoint.longitude += offset.longitude;


            }


            return inPoint;
        }


        private Vector3LatLong GetOffset(Vector3LatLong point)
        {
            FindGridFile(point);
            if (FoundGrid == null)
            {
                return new Vector3LatLong(double.NaN, double.NaN, double.NaN,CrsNames.Amersfoort);
            }
            double pointLattitude = point.lattitude * 3600;    // (in seconds)
            double pointLongitude = -point.longitude * 3600;    // (in seconds)

            int gridpointsPerColumn = 1 + (int)((FoundGrid.N_LAT - FoundGrid.S_LAT) / FoundGrid.LAT_INC); //180
            int gridpointsPerRow = 1 + (int)((FoundGrid.W_LONG - FoundGrid.E_LONG) / FoundGrid.LONG_INC);//588

            double localLattitude = pointLattitude - FoundGrid.S_LAT;
            double localLongitude = pointLongitude - FoundGrid.E_LONG;


            int rowIndex = (int)Math.Floor(localLattitude / FoundGrid.LAT_INC);//120
            int columnIndex = (int)Math.Floor(localLongitude / FoundGrid.LONG_INC);//576

            int recordIndexLR = (rowIndex * gridpointsPerRow) + columnIndex; //71136
            int recordIndexLL = recordIndexLR + 1;//71137
            int recordIndexUR = recordIndexLR + gridpointsPerRow;
            int recordIndexUL = recordIndexUR + 1;
            if (rowIndex == gridpointsPerRow)
            {
                recordIndexUL = recordIndexLL;
                recordIndexUR = recordIndexLR;
            }
            if (columnIndex == gridpointsPerColumn)
            {
                recordIndexUL = recordIndexUR;
                recordIndexLL = recordIndexLR;
            }

            double localLattitudeA = FoundGrid.LAT_INC * rowIndex;
            double localLongitudeA = FoundGrid.LONG_INC * columnIndex;

            double x = (localLattitude - localLattitudeA) / FoundGrid.LAT_INC;
            double y = (localLongitude - localLongitudeA) / FoundGrid.LONG_INC;

            float aLat = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexLR * 16);
            float aLon = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexLR * 16 + 4);

            float bLat = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexLL * 16);
            float bLon = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexLL * 16 + 4);

            float cLat = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexUR * 16);
            float cLon = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexUR * 16 + 4);

            float dLat = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexUL * 16);
            float dLon = BitConverter.ToSingle(griddata, FoundGrid.datastart + recordIndexUL * 16 + 4);

            double eLat = aLat + ((bLat - aLat) * y);
            double fLat = (double)cLat + ((double)(dLat - cLat) * (double)y);
            double pLat = eLat + ((fLat - eLat) * x);

            double eLon = aLon + ((bLon - aLon) * y);
            double fLon = cLon + ((dLon - cLon) * y);
            double pLon = eLon + ((fLon - eLon) * x);

            Vector3LatLong result = new Vector3LatLong();

            result.lattitude = (pLat / 3600);
            result.longitude = -(pLon / 3600);

            return result;

        }
    }
    public enum transformationDirection
    {
        Forward,
        reverse
    }
    [Serializable]
    public class Grid
    {
        public List<Grid> subgrids = new List<Grid>();
        public string SUB_NAME;
        public string PARENT;
        public string CREATED;
        public string UPDATED;
        public double S_LAT;
        public double N_LAT;
        public double E_LONG;
        public double W_LONG;
        public double LAT_INC;
        public double LONG_INC;
        public int GS_COUNT;
        public int datastart;

        public void Add(Grid subgrid)
        {
            if (subgrid.PARENT == SUB_NAME)
            {
                subgrids.Add(subgrid);
            }
            else
            {
                for (int i = 0; i < subgrids.Count; i++)
                {
                    subgrids[i].Add(subgrid);
                }
            }

        }

        public Grid getGridWithPoint(Vector3LatLong point)
        {
            for (int i = 0; i < subgrids.Count; i++)
            {
                Grid tempgrid = subgrids[i].getGridWithPoint(point);
                if (tempgrid != null)
                {
                    return tempgrid;
                }
            }
            double pointLattitude = point.lattitude * 3600;    // (in seconds)
            double pointLongitude = -point.longitude * 3600;    // (in seconds)
            if (pointLattitude < S_LAT)
            {
                return null;
            }
            if (pointLattitude > N_LAT)
            {
                return null;
            }
            if (pointLongitude < E_LONG)
            {
                return null;
            }
            if (pointLongitude > W_LONG)
            {
                return null;
            }

            return this;
        }

        public void GridShiftInterpolation(Vector3LatLong point)
        {


        }

    }

    [Serializable]
    public class FileHeader
    {
        public int NUM_OREC;
        public int NUM_SREC;
        public int NUM_FILE;
        public string GS_TYPE;
        public string VERSION;
        public string SYSTEM_F;
        public string SYSTEM_T;
        public double MAJOR_F;
        public double MINOR_F;
        public double MAJOR_T;
        public double MINOR_T;
    }

}