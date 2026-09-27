namespace FanHubPlus.Helpers
{
    // Straight-line (great-circle) distance between two GPS points, used to sort/filter
    // events by how close they are to the visitor's current location.
    public static class GeoDistance
    {
        private const double EarthRadiusKm = 6371.0;

        public static double KmBetween(double lat1, double lng1, double lat2, double lng2)
        {
            double dLat = ToRadians(lat2 - lat1);
            double dLng = ToRadians(lng2 - lng1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        // Event.LocationCoordinates is stored as free text, ideally "lat,lng" (e.g. "24.8607,67.0011").
        // Returns null if it can't be parsed, so callers can fall back to city-based filtering.
        public static (double Lat, double Lng)? TryParse(string? coordinates)
        {
            if (string.IsNullOrWhiteSpace(coordinates)) return null;

            var parts = coordinates.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return null;

            if (double.TryParse(parts[0], System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                double.TryParse(parts[1], System.Globalization.CultureInfo.InvariantCulture, out var lng))
            {
                return (lat, lng);
            }

            return null;
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}
