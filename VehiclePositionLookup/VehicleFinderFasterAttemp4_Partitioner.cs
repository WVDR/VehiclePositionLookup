using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;


namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp4_Partitioner
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            var partitioner = Partitioner.Create(vehiclePositions);
            Parallel.ForEach(partitioner, coord => vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out _)));            

            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp4_Partitioner)} Benchmark Start: {DateTime.Now}");
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
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp4_Partitioner)} Benchmark End: {DateTime.Now}");
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
