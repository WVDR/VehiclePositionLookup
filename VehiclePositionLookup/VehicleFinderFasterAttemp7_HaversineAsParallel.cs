using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp7_HaversineAsParallel
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            //Just the basic rework and looking at the differance.
            List<VehiclePosition> vehiclePositionList = coords
                .AsParallel()
                .Select(coord => HaversineCalculator.FindClosestLocation(vehiclePositions, coord))
                .ToList();           

            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp7_HaversineAsParallel)} Benchmark Start: {DateTime.Now}");
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
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp7_HaversineAsParallel)} Benchmark End: {DateTime.Now}");
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
