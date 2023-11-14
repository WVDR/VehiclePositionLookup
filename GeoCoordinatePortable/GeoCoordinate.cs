using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCoordinatePortable
{
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        public static readonly GeoCoordinate Unknown = new GeoCoordinate();
        private double _course;
        private double _horizontalAccuracy;
        private double _latitude;
        private double _longitude;
        private double _speed;
        private double _verticalAccuracy;

        public GeoCoordinate()
          : this(double.NaN, double.NaN)
        {
        }

        public GeoCoordinate(double latitude, double longitude)
          : this(latitude, longitude, double.NaN)
        {
        }

        public GeoCoordinate(double latitude, double longitude, double altitude)
          : this(latitude, longitude, altitude, double.NaN, double.NaN, double.NaN, double.NaN)
        {
        }

        public GeoCoordinate(
          double latitude,
          double longitude,
          double altitude,
          double horizontalAccuracy,
          double verticalAccuracy,
          double speed,
          double course)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = altitude;
            this.HorizontalAccuracy = horizontalAccuracy;
            this.VerticalAccuracy = verticalAccuracy;
            this.Speed = speed;
            this.Course = course;
        }

        public double Latitude
        {
            get => this._latitude;
            set => this._latitude = value <= 90.0 && value >= -90.0 ? value : throw new ArgumentOutOfRangeException(nameof(Latitude), "Argument must be in range of -90 to 90");
        }

        public double Longitude
        {
            get => this._longitude;
            set => this._longitude = value <= 180.0 && value >= -180.0 ? value : throw new ArgumentOutOfRangeException(nameof(Longitude), "Argument must be in range of -180 to 180");
        }

        public double HorizontalAccuracy
        {
            get => this._horizontalAccuracy;
            set
            {
                if (value < 0.0)
                    throw new ArgumentOutOfRangeException(nameof(HorizontalAccuracy), "Argument must be non negative");
                this._horizontalAccuracy = value == 0.0 ? double.NaN : value;
            }
        }

        public double VerticalAccuracy
        {
            get => this._verticalAccuracy;
            set
            {
                if (value < 0.0)
                    throw new ArgumentOutOfRangeException(nameof(VerticalAccuracy), "Argument must be non negative");
                this._verticalAccuracy = value == 0.0 ? double.NaN : value;
            }
        }

        public double Speed
        {
            get => this._speed;
            set => this._speed = (double.IsNaN(value) || value >= 0.0) ? value : throw new ArgumentOutOfRangeException("speed", $"'{value}': Argument must be non negative");
        }

        public double Course
        {
            get => this._course;
            set => this._course = double.IsNaN(value) || (value >= 0.0 && value <= 360.0) ? value : throw new ArgumentOutOfRangeException("course", $"'{value}': Argument must be in range 0 to 360");
        }

        public bool IsUnknown => this.Equals(GeoCoordinate.Unknown);

        public double Altitude { get; set; }

        public bool Equals(GeoCoordinate other)
        {
            if ((object)other == null)
                return false;
            double num = this.Latitude;
            if (!num.Equals(other.Latitude))
                return false;
            num = this.Longitude;
            return num.Equals(other.Longitude);
        }

        public static bool operator ==(GeoCoordinate left, GeoCoordinate right) => (object)left == null ? (object)right == null : left.Equals(right);

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right) => !(left == right);

        public double GetDistanceTo(GeoCoordinate other)
        {
            if (double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude) || double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
                throw new ArgumentException("Argument latitude or longitude is not a number");
            double d1 = this.Latitude * (Math.PI / 180.0);
            double num1 = this.Longitude * (Math.PI / 180.0);
            double d2 = other.Latitude * (Math.PI / 180.0);
            double num2 = other.Longitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public override int GetHashCode()
        {
            double num = this.Latitude;
            int hashCode1 = num.GetHashCode();
            num = this.Longitude;
            int hashCode2 = num.GetHashCode();
            return hashCode1 ^ hashCode2;
        }

        public override bool Equals(object obj) => this.Equals(obj as GeoCoordinate);

        public override string ToString() => this == GeoCoordinate.Unknown ? "Unknown" : string.Format("{0}, {1}", (object)this.Latitude.ToString("G", (IFormatProvider)CultureInfo.InvariantCulture), (object)this.Longitude.ToString("G", (IFormatProvider)CultureInfo.InvariantCulture));
    }
}
