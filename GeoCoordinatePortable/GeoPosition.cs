using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCoordinatePortable
{
    public class GeoPosition<T>
    {
        public GeoPosition()
        {
        }

        public GeoPosition(DateTimeOffset timestamp, T location)
        {
            this.Timestamp = timestamp;
            this.Location = location;
        }

        public T Location { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
