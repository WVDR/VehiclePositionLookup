using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp2
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);

            Stopwatch stopwatch = Stopwatch.StartNew();
            
            List<VehiclePosition> vehiclePositionList = coords                
                .Select(coord => GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out _))
                .ToList();

            stopwatch.Stop();

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp2)} Benchmark Start: {DateTime.Now}");
            stringBuilder.AppendLine();

            foreach (VehiclePosition vehiclePosition in vehiclePositionList)
            {
                stringBuilder.AppendLine(vehiclePosition.GetTextSummary());
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Data file read execution time: {elapsedMilliseconds} ms");
            stringBuilder.AppendLine($"Closest position calculation execution time: {stopwatch.ElapsedMilliseconds} ms");
            stringBuilder.AppendLine($"Total execution time: {elapsedMilliseconds + stopwatch.ElapsedMilliseconds} ms");
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp2)} Benchmark End: {DateTime.Now}");
            stringBuilder.AppendLine();

            Console.WriteLine(stringBuilder.ToString());

            // Use a using statement to ensure proper resource disposal
            using (StreamWriter writer = new StreamWriter(benchmarkFileLocation, true))
            {
                // Write the data to the file
                writer.Write(stringBuilder.ToString());
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
