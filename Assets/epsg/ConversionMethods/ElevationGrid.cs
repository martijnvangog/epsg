using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace epsg
{
    public class ElevationGrid 
    {
        //file description at https://vdatum.noaa.gov/docs/gtx_info.html
        // file is in java binary format (= Big Endian)
        byte[] griddata;
        byte[] buffer = new byte[8];
        //[Header("grid data")]

        double lowerLeftLattitude;
        double lowerLeftLongitude;
        double deltaLattitude;
        double deltaLongitude;
        int rowCount;
        int columnCount;
        int firstValueIndex;
        public void ReadFile(string filename)
        {
            
            griddata = Resources.Load<TextAsset>(filename).bytes;
            
            ReadFileHeader();

            Vector3Geographic position = new Vector3Geographic(50, 2.36, 0);
            
        }

        void ReadFileHeader()
        {
            //var bigendianBitConverter = new miscUtil.conversion.Bi
            
            //FileHeader result = new FileHeader();
            bool isLE = BitConverter.IsLittleEndian;

            getBytes(griddata, 0, 8);
            lowerLeftLattitude = BitConverter.ToDouble(buffer, 0);
            getBytes(griddata, 8, 8);
            lowerLeftLongitude = BitConverter.ToDouble(buffer, 0);
            getBytes(griddata, 16, 8);
            deltaLattitude = BitConverter.ToDouble(buffer, 0);
            getBytes(griddata, 24, 8);
            deltaLongitude = BitConverter.ToDouble(buffer, 0);
            getBytes(griddata, 32, 4);
            rowCount = BitConverter.ToInt32(buffer, 0);
            getBytes(griddata, 36, 4);
            columnCount = BitConverter.ToInt32(buffer, 0);
            firstValueIndex = 40;

        }

        public float GetElevation(Vector3Geographic position)
        {
            float output = float.NaN;
            double localLattitude = position.lattitude - lowerLeftLattitude;
            if (localLattitude<0)
            {
                return output;
            }
            double localLongitude = position.longitude - lowerLeftLongitude;
            if (localLongitude<0)
            {
                return output;
            }
            int columnNumber = (int)Math.Floor(localLongitude / deltaLongitude);
            if (columnNumber>columnCount-2)
            {
                return output;
            }
            int rowNumber = (int)Math.Floor(localLattitude / deltaLattitude);
            if (rowNumber > rowCount - 2)
            {
                return output;
            }

            int lowerLeftIndex = 4*(rowNumber * columnCount + columnNumber);
            int lowerRightIndex = lowerLeftIndex + 4;
            int topLeftIndex = lowerLeftIndex + (columnCount*4);
            int topRightIndex = topLeftIndex + 4;

            double ingridLattitude = localLattitude - (rowNumber * deltaLattitude);
            double ingridLongitude = localLongitude - (columnNumber * deltaLongitude);

            double x = ingridLongitude / deltaLongitude;
            double y = ingridLattitude / deltaLattitude;

            // get the elevationvalues for the four corners
            getBytes(griddata, lowerLeftIndex + firstValueIndex, 4);
            double lowerLeftHeight = BitConverter.ToSingle(buffer);
            getBytes(griddata, lowerRightIndex + firstValueIndex, 4);
            double lowerRightHeight = BitConverter.ToSingle(buffer);
            getBytes(griddata, topLeftIndex + firstValueIndex, 4);
            double topLeftHeight = BitConverter.ToSingle(buffer);
            getBytes(griddata, topRightIndex + firstValueIndex, 4);
            double topRightHeight = BitConverter.ToSingle(buffer);

            double leftHeight = lowerLeftHeight + (topLeftHeight - lowerLeftHeight) * y;
            double rightHeight = lowerRightHeight + (topRightHeight - lowerRightHeight) * y;
            double centerHeight = leftHeight + (rightHeight - leftHeight) * x;

            return (float)centerHeight;
        }

        void getBytes(byte[] source, int index, int bytecount)
        {
            int j = 0;
            
            for (int i = bytecount - 1; i >= 0; i--)
            {
                buffer[j] = source[index + i];
                j++;
            }
        }
      
    }
}