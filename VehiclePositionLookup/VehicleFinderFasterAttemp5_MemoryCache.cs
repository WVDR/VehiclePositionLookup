using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp5_MemoryCache
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            var dictionary = DataFileParserMemoryCache.ReadDataFileIntoMemoryCache(dataFilePath);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            // Using memcache... could have used a dictionary but we can go from this to distrobuted mem chache or run a cron job to refresh.
            // since we are not using ef you would need to refresh the service in memory version, with the db store there as well.
            foreach (Coord coord in coords)
            {
                var coOrdinateKey = DataFileParserMemoryCache.JoinCoOrdinates(coord.Latitude, coord.Longitude);
                if (DataFileParserMemoryCache._cache.TryGetValue(coOrdinateKey, out List<VehiclePosition> vehiclePositions))
                {
                    vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out _));
                }
                else if(dictionary.ContainsKey(DataFileParserMemoryCache.JoinCoOrdinates(coord.Latitude, coord.Longitude)))
                {
                    vehiclePositionList.Add(GetNearest(dictionary[coOrdinateKey], coord.Latitude, coord.Longitude, out _));
                }
                
            }
                

            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp5_MemoryCache)} Benchmark Start: {DateTime.Now}");
            stringBuilder.AppendLine();
            foreach (VehiclePosition vehiclePosition in vehiclePositionList)
            {
                stringBuilder.AppendLine(vehiclePosition.GetTextSummary());

            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Data file read execution time : {0} ms", (object)elapsedMilliseconds));
            stringBuilder.AppendLine(string.Format("Closest position calculation execution time : {0} ms", (object)stopwatch.ElapsedMilliseconds));
            stringBuilder.AppendLine(string.Format("Total execution time : {0} ms", (object)(elapsedMilliseconds + stopwatch.ElapsedMilliseconds)));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp5_MemoryCache)} Benchmark End: {DateTime.Now}");
            stringBuilder.AppendLine();
            Console.WriteLine(stringBuilder.ToString());

            using (StreamWriter writer = new StreamWriter(benchmarkFileLocation, true))
            {
                // Write the data to the file
                writer.Write(stringBuilder.ToString());
                writer.Close();
            }
        }        

        internal static VehiclePosition GetNearest(
          IEnumerable<VehiclePosition> vehiclePositions,
          float latitude,
          float longitude,
          out double nearestDistance)
        {

            if (!vehiclePositions.Any())
            {
                throw new ArgumentException("vehiclePositions", "vehiclePositions cannot be an empty list");
            }

            nearestDistance = double.MaxValue;

            var nearest = vehiclePositions
                .OrderBy(vp => Util.DistanceBetween(latitude, longitude, vp.Latitude, vp.Longitude))
                .FirstOrDefault();

            if (nearest != null)
            {
                nearestDistance = Util.DistanceBetween(latitude, longitude, nearest.Latitude, nearest.Longitude);
            }

            return nearest;
        }       
    }
}
