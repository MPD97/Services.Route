using System;
using Services.Route.Core.Exceptions;

namespace Services.Route.Core.ValueObjects
{
    public struct Point: IEquatable<Point>
    {
        public Guid Id { get; }
        public int Order { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public int Radius { get; }

        public Point(Guid id, int order, decimal latitude, decimal longitude, int radius)
        {
            Id = id;
            Order = order < 0 ? throw new InvalidPointOrderException(order) : order;
            Latitude = IsValidLatitude(latitude) ? latitude : throw new InvalidLatitudeException(latitude);
            Longitude = IsValidLongitude(longitude) ? longitude : throw new InvalidLongitudeException(longitude);
            Radius = IsValidRadius(radius) ? radius : throw new InvalidRadiusException(radius);
        }

        public static bool IsValidLatitude(decimal latitude)
            => latitude is >= -90m and <= 90m;
        public static bool IsValidLongitude(decimal longitude)
            => longitude is >= -180m and <= 180m;
        public static bool IsValidRadius(int radius)
            => radius is >= 0 and <= 100;
        
        public bool Equals(Point other)
        {
            return Id.Equals(other.Id) && Order == other.Order && Latitude == other.Latitude && Longitude == other.Longitude && Radius == other.Radius;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Order, Latitude, Longitude, Radius);
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }
    }
}