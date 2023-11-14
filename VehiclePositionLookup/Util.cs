using GeoCoordinatePortable;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace VehiclePositionLookup
{
    internal static class Util
    {
        internal static DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, 0);

        internal static string GetLocalFilePath(string fileName) => Util.GetLocalFilePath(string.Empty, fileName);

        internal static double DistanceBetween(
          float latitude1,
          float longitude1,
          float latitude2,
          float longitude2)
        {
            return new GeoCoordinate((double)latitude1, (double)longitude1).GetDistanceTo(new GeoCoordinate((double)latitude2, (double)longitude2));
        }

        internal static string GetLocalFilePath(string subDirectory, string fileName) => Path.Combine(Util.GetLocalPath(subDirectory), fileName);

        internal static byte[] ToNullTerminatedString(string registration)
        {
            byte[] bytes = Encoding.Default.GetBytes(registration);
            byte[] terminatedString = new byte[bytes.Length + 1];
            bytes.CopyTo((Array)terminatedString, 0);
            return terminatedString;
        }

        internal static string GetLocalPath(string subDirectory)
        {
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (subDirectory != string.Empty)
                path1 = Path.Combine(path1, subDirectory);
            return path1;
        }

        internal static string SelectRandom(Random rnd, string[] values)
        {
            int index = rnd.Next(values.Length - 1);
            return values[index];
        }

        internal static ulong ToCTime(DateTime time) => Convert.ToUInt64((time - Util.Epoch).TotalSeconds);

        internal static DateTime FromCTime(ulong cTime) => Util.Epoch.AddSeconds((double)cTime);
    }
}
