using System;
using System.Drawing;

namespace VehiclePositionLookup
{
    internal class Program
    {
        private const int VEHICLE_COUNT = 2000000;
        private const int FIND_VEHICLES = 10;

        private static void Main(string[] args)
        {
            VehicleFinderSlow.FindClosestN(Program.GetLookupPositions());
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static Coord[] GetLookupPositions()
        {
            Coord[] lookupPositions = new Coord[10];
            lookupPositions[0].Latitude = 34.54491f;
            lookupPositions[0].Longitude = -102.100845f;
            lookupPositions[1].Latitude = 32.3455429f;
            lookupPositions[1].Longitude = -99.12312f;
            lookupPositions[2].Latitude = 33.2342339f;
            lookupPositions[2].Longitude = -100.214127f;
            lookupPositions[3].Latitude = 35.19574f;
            lookupPositions[3].Longitude = -95.3489f;
            lookupPositions[4].Latitude = 31.89584f;
            lookupPositions[4].Longitude = -97.78957f;
            lookupPositions[5].Latitude = 32.89584f;
            lookupPositions[5].Longitude = -101.789574f;
            lookupPositions[6].Latitude = 34.1158371f;
            lookupPositions[6].Longitude = -100.225731f;
            lookupPositions[7].Latitude = 32.33584f;
            lookupPositions[7].Longitude = -99.99223f;
            lookupPositions[8].Latitude = 33.53534f;
            lookupPositions[8].Longitude = -94.79223f;
            lookupPositions[9].Latitude = 32.2342339f;
            lookupPositions[9].Longitude = -100.222221f;
            return lookupPositions;
        }
    }
}
