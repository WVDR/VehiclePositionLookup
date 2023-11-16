using ClusterCreator;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;


namespace VehiclePositionLookup
{
    
    internal class DataFileParserMemoryCache
    {
        public static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        internal static Dictionary<int, BaseCluster> ReadDataFileIntoMemoryCache(string datafilepath)
        {
            byte[] data = ReadFileData(datafilepath);
            
            int offset = 0;
            Dictionary <string,List<VehiclePosition>> VehiclePositionDictionary = new Dictionary<string, List<VehiclePosition>>();
            List<BaseCluster> clusters = new List<BaseCluster>();
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            while (offset < data.Length)
            {
                var vehicalpos = ReadVehiclePosition(data, ref offset);
                BaseCluster baseCluster = new BaseCluster(vehicalpos.ID, vehicalpos.Registration, vehicalpos.Latitude,vehicalpos.Longitude);
                clusters.Add(baseCluster);
                vehiclePositionList.Add(vehicalpos);
            }
            VehiclePositionCluster vehiclePositionCluster = new ClusterCreator.VehiclePositionCluster();
            var result = vehiclePositionCluster.GetClusters(clusters, 800);
            return result;


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
