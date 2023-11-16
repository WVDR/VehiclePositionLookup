using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;


namespace VehiclePositionLookup
{
    
    internal class DataFileParserMemoryCache
    {
        public static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        internal static Dictionary<string, List<VehiclePosition>> ReadDataFileIntoMemoryCache(string datafilepath)
        {
            byte[] data = ReadFileData(datafilepath);
            
            int offset = 0;
            Dictionary <string,List<VehiclePosition>> VehiclePositionDictionary = new Dictionary<string, List<VehiclePosition>>();
            while (offset < data.Length)
            {
                List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
                VehiclePosition vehiclePosition = ReadVehiclePosition(data, ref offset);
                vehiclePositionList.Add(vehiclePosition);

                string vehicalePositionId = JoinCoOrdinates(vehiclePosition.Latitude, vehiclePosition.Longitude);
                if (_cache.TryGetValue(vehicalePositionId, out List<VehiclePosition> exsistingVehiclePosition))
                {
                    exsistingVehiclePosition.Add(vehiclePosition);
                    _cache.Set(vehicalePositionId, exsistingVehiclePosition, DateTime.UtcNow.AddDays(7));
                    VehiclePositionDictionary[vehicalePositionId].Add(vehiclePosition);
                }
                else
                {
                    _cache.Set(vehicalePositionId, vehiclePositionList, DateTime.UtcNow.AddDays(7));
                    VehiclePositionDictionary.Add(vehicalePositionId, vehiclePositionList);
                }             
            }
            return VehiclePositionDictionary;


        }

        private static byte[] ReadFileData(string datafilepath)
        {            
            string localFilePath = Util.GetLocalFilePath(datafilepath);
            if (File.Exists(localFilePath))
                return File.ReadAllBytes(localFilePath);
            Console.WriteLine("Data file not found.");
            throw new FileNotFoundException(string.Format($"'{datafilepath}': Data file not found."));
        }

        private static VehiclePosition ReadVehiclePosition(byte[] data, ref int offset) => VehiclePosition.FromBytes(data, ref offset);

        public static string JoinCoOrdinates(float latitude, float longitude)
        {
            float[] coords = { latitude, longitude };
            return string.Join(",", coords);
        }

    }
}
