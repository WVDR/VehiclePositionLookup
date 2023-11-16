using System;
using System.Collections.Generic;
using System.IO;


namespace VehiclePositionLookup
{
    internal class DataFileParser
    {
        internal static List<VehiclePosition> ReadDataFile(string datafilepath)
        {
            byte[] data = ReadFileData(datafilepath);
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            int offset = 0;
            while (offset < data.Length)
                vehiclePositionList.Add(ReadVehiclePosition(data, ref offset));
            return vehiclePositionList;
        }

        internal static List<VehiclePosition> ReadDataFileOffset(string datafilepath, int offset)
        {
            byte[] data = ReadFileData(datafilepath);
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();            
            while (offset < data.Length)
                vehiclePositionList.Add(ReadVehiclePosition(data, ref offset));
            return vehiclePositionList;
        }

        internal static byte[] ReadFileData(string datafilepath)
        {            
            string localFilePath = Util.GetLocalFilePath(datafilepath);
            if (File.Exists(localFilePath))
                return File.ReadAllBytes(localFilePath);
            Console.WriteLine("Data file not found.");
            throw new FileNotFoundException(string.Format($"'{datafilepath}': Data file not found."));
        }

        private static VehiclePosition ReadVehiclePosition(byte[] data, ref int offset) => VehiclePosition.FromBytes(data, ref offset);
    }
}
