using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    public class HaversineCalculator
    {
        public static VehiclePosition FindClosestLocation(List<VehiclePosition> locations, Coord targetLocation)
        {
            if (locations == null || locations.Count == 0)
                throw new ArgumentException("The list of locations is empty or null.");

            VehiclePosition closestLocation = locations[0];
            double closestDistance = CalculateHaversineDistance(targetLocation, closestLocation);

            foreach (var location in locations)
            {
                double distance = CalculateHaversineDistance(targetLocation, location);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLocation = location;
                }
            }

            return closestLocation;
        }

        private static double CalculateHaversineDistance(Coord location1, VehiclePosition location2)
        {
            const double R = 6371; // Earth radius in kilometers

            double dLat = Deg2Rad(location2.Latitude - location1.Latitude);
            double dLon = Deg2Rad(location2.Longitude - location1.Longitude);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(Deg2Rad(location1.Latitude)) * Math.Cos(Deg2Rad(location2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }

        private static double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180);
        }        
    }
}
