using ClusterCreator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp5_MaxDistance
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);
            List<double> distanceList = new List<double>();
            List<double> distanceListFromSuppliedList = new List<double>();
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            //Normally we would be able to specify each time what the distance is to search per cluster/group...
            //here we are pre-determining the max and then  working backwards, to min, for the most optimal.

            for (int i = 1; i <= coords.Length-1; i++)
            {
                var distance = DistanceCalculator.GetDistanceFromLatLonInKm(coords[i - 1].Latitude, coords[i - 1].Longitude, coords[i].Latitude, coords[i].Longitude);
                distanceListFromSuppliedList.Add(distance);

            }

            foreach (Coord coord in coords)
            {
                vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out _));
                foreach (VehiclePosition vehiclePosition in vehiclePositions)
                {
                    var distance = DistanceCalculator.GetDistanceFromLatLonInKm(coord.Latitude, coord.Longitude, vehiclePosition.Latitude, vehiclePosition.Longitude);
                    distanceList.Add(distance);
                }
                
            }
                

            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp5_MaxDistance)} Benchmark Start: {DateTime.Now}");
            stringBuilder.AppendLine();
            foreach (VehiclePosition vehiclePosition in vehiclePositionList)
            {
                stringBuilder.AppendLine(vehiclePosition.GetTextSummary());

            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(string.Format("Data file read execution time : {0} ms", (object)elapsedMilliseconds));
            stringBuilder.AppendLine(string.Format("Max distance calculation execution time : {0} ms", (object)stopwatch.ElapsedMilliseconds));
            stringBuilder.AppendLine(string.Format("Total execution time : {0} ms", (object)(elapsedMilliseconds + stopwatch.ElapsedMilliseconds)));
            stringBuilder.AppendLine(string.Format("Max distance from data set : {0} km", distanceListFromSuppliedList.Max()));
            stringBuilder.AppendLine(string.Format("Max distance from supplied set : {0} km", distanceList.Max()));
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp5_MaxDistance)} Benchmark End: {DateTime.Now}");
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
