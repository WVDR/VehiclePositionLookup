﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    internal class VehicleFinderFasterAttemp7_HaversineSegmented
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            List<VehiclePosition> vehiclePositionListByKey = new List<VehiclePosition>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);
            var vehiclePositionSegments = VehiclePositionListProcessor.SegmentVehiclePositions(vehiclePositions, 1000000);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            //Just the basic rework and looking at the differance.
            foreach (Coord coord in coords)
            {                                
                foreach (var vehiclePositionSegment in vehiclePositionSegments)
                {                    
                    if (coord.Latitude >= vehiclePositionSegment.Key.Item1.minLat && coord.Latitude <= vehiclePositionSegment.Key.Item1.maxLat && coord.Longitude >= vehiclePositionSegment.Key.Item2.minLon && coord.Longitude <= vehiclePositionSegment.Key.Item2.maxLon)
                    {
                        vehiclePositionList.Add(HaversineCalculator.FindClosestLocation(vehiclePositionSegment.Value, coord));
                        break;
                    }
                }
            }
                

            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp7_HaversineSegmented)} Benchmark Start: {DateTime.Now}");
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
            stringBuilder.AppendLine($"{nameof(VehicleFinderFasterAttemp7_HaversineSegmented)} Benchmark End: {DateTime.Now}");
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
