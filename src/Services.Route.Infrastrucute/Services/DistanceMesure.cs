using System;
using Services.Route.Application.Events.External;
using Services.Route.Application.Services;

namespace Services.Route.Infrastrucute.Services
{
    public class DistanceMesure : IDistanceMeasure
    {
        private const double PIx = Math.PI;
        private const int EarthRadius = 6378137;
        public double GetDistanceBetween(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double sLat1 = Math.Sin(Radians(lat1));
            double sLat2 = Math.Sin(Radians(lat2));
            double cLat1 = Math.Cos(Radians(lat1));
            double cLat2 = Math.Cos(Radians(lat2));
            double cLon = Math.Cos(Radians(lon1) - Radians(lon2));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = EarthRadius * d;

            return dist;
        }

        private double Radians(decimal x)
            =>  (double)x * PIx / 180;
    }
}