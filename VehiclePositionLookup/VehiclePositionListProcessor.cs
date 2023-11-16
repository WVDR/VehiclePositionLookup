using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclePositionLookup
{
    public static class VehiclePositionListProcessor
    {
        public static Dictionary<((float minLat, float maxLat), (float minLon, float maxLon)), List<VehiclePosition>> SegmentVehiclePositions(List<VehiclePosition> locations, int segmentSize)
        {
            if (locations == null || locations.Count == 0 || segmentSize <= 0)
                throw new ArgumentException("Invalid input parameters.");
            
            Dictionary<((float minLat, float maxLat), (float minLon, float maxLon)), List<VehiclePosition>> minMaxOfVehiclePosition = new Dictionary<((float minLat, float maxLat), (float minLon, float maxLon)), List<VehiclePosition>>();
            

            for (int i = 0; i < locations.Count; i += segmentSize)
            {
                List<VehiclePosition> segment = locations.Skip(i).Take(segmentSize).ToList();
                float minSegmentLat = segment.Min(m => m.Latitude);
                float maxSegmentLat = segment.Max(m => m.Latitude);
                float minSegmentLon = segment.Min(m => m.Longitude);
                float maxSegmentLon = segment.Max(m => m.Longitude);
                minMaxOfVehiclePosition.Add(((minSegmentLat, maxSegmentLat), (minSegmentLon, maxSegmentLon)), segment);
            }
            

            return minMaxOfVehiclePosition;
        }
    }
}
