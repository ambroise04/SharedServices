using Geolocation;

namespace SharedServices.BL.Services
{
    public static class Geocoding
    {
        public static double DistanceCalculator(Coordinate source, Coordinate destination)
        {
            var distance = GeoCalculator.GetDistance(source, destination, 5);

            return distance;
        }

    }
}