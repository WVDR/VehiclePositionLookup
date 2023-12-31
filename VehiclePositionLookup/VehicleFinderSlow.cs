﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VehiclePositionLookup
{
    internal class VehicleFinderSlow
    {
        internal static void FindClosestN(Coord[] coords, string dataFilePath, string benchmarkFileLocation)
        {
            List<VehiclePosition> vehiclePositionList = new List<VehiclePosition>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<VehiclePosition> vehiclePositions = DataFileParser.ReadDataFile(dataFilePath);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            foreach (Coord coord in coords)
                vehiclePositionList.Add(GetNearest(vehiclePositions, coord.Latitude, coord.Longitude, out _));
            stopwatch.Stop();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            stringBuilder.AppendLine($"{nameof(VehicleFinderSlow)} Benchmark Start: {DateTime.Now}");
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
            stringBuilder.AppendLine($"{nameof(VehicleFinderSlow)} Benchmark End: {DateTime.Now}");
            stringBuilder.AppendLine();
            Console.WriteLine(stringBuilder.ToString());

            using (StreamWriter writer = new StreamWriter(benchmarkFileLocation,true))
            {
                // Write the data to the file
                writer.Write(stringBuilder.ToString());
                writer.Close();
            }

        }

        internal static void FindClosest(float latitude, float longitude, string datafilepath)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double nearestDistance;
            VehiclePosition nearest = VehicleFinderSlow.GetNearest(DataFileParser.ReadDataFile(datafilepath), latitude, longitude, out nearestDistance);
            stopwatch.Stop();
            Console.WriteLine(string.Format("Execution time : {0} ms", (object)stopwatch.ElapsedMilliseconds));
            Console.WriteLine(string.Format("{0} : {1}", (object)nearestDistance, (object)nearest.GetTextSummary()));
        }

        internal static void FindClosest(int count, float latitude, float longitude, string datafilepath)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            SortedList<double, VehiclePosition> sortedList = VehicleFinderSlow.AddNearestPositions(DataFileParser.ReadDataFile(datafilepath), count, latitude, longitude);
            stopwatch.Stop();
            Console.WriteLine(string.Format("Execution time : {0} ms", (object)stopwatch.ElapsedMilliseconds));
            foreach (double key in (IEnumerable<double>)sortedList.Keys)
                Console.WriteLine(string.Format("{0} : {1}", (object)key, (object)sortedList[key].GetTextSummary()));
        }

        internal static VehiclePosition GetNearest(
          List<VehiclePosition> vehiclePositions,
          float latitude,
          float longitude,
          out double nearestDistance)
        {
            VehiclePosition? nearest = null;
            nearestDistance = double.MaxValue;
            foreach (VehiclePosition vehiclePosition in vehiclePositions)
            {
                double num = Util.DistanceBetween(latitude, longitude, vehiclePosition.Latitude, vehiclePosition.Longitude);
                if (num < nearestDistance)
                {
                    nearest = vehiclePosition;
                    nearestDistance = num;
                }
            }
            return nearest;
        }

        private static SortedList<double, VehiclePosition> AddNearestPositions(
          List<VehiclePosition> vehiclePositions,
          int count,
          float latitude,
          float longitude)
        {
            SortedList<double, VehiclePosition> sortedList = new SortedList<double, VehiclePosition>();
            foreach (VehiclePosition vehiclePosition in vehiclePositions)
            {
                double key1 = Util.DistanceBetween(latitude, longitude, vehiclePosition.Latitude, vehiclePosition.Longitude);
                if (sortedList.Count < count)
                {
                    sortedList.Add(key1, vehiclePosition);
                }
                else
                {
                    double key2 = sortedList.Keys[sortedList.Keys.Count - 1];
                    if (key1 < key2)
                    {
                        sortedList.Remove(key2);
                        sortedList.Add(key1, vehiclePosition);
                    }
                }
            }
            return sortedList;
        }
    }
}
