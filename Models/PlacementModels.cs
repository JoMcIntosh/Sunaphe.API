using System;

namespace Sunaphe.API.Models {
    public record Coordinate(double Latitude, double Longitude, string Name);

    public record Placement<T>(T ObjectName, double Longitude) where T : Enum {
        private double Offset { get => Math.Floor(Longitude / 30.0); }
        public Zodiac Sign { get => (Zodiac)Offset; }
        public double Degree { get => Longitude - 30.0 * Offset; }
        public string DegreeDMS {
            get {
                var degree = Degree;
                var degreePart = Math.Truncate(degree);
                degree -= degreePart;
                var minutePart = Math.Truncate(degree * 60.0);
                degree -= minutePart / 60.0;
                var secondPart = Math.Truncate(degree * 3600.0);
                return $"{degreePart}° {minutePart}\' {secondPart}\"";
            }
        }
    }
}