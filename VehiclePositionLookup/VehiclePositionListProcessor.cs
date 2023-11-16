using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    public static class VehiclePositionListProcessor
    {
        public static List<List<VehiclePosition>> SegmentVehiclePositions(List<VehiclePosition> locations, int segmentSize)
        {
            if (locations == null || locations.Count == 0 || segmentSize <= 0)
                throw new ArgumentException("Invalid input parameters.");

            List<List<VehiclePosition>> segmentedLocations = new List<List<VehiclePosition>>();

            for (int i = 0; i < locations.Count; i += segmentSize)
            {
                List<VehiclePosition> segment = locations.Skip(i).Take(segmentSize).ToList();
                segmentedLocations.Add(segment);
            }

            return segmentedLocations;
        }
    }
}
