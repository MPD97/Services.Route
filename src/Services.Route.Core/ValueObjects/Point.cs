using System;

namespace Services.Route.Core.ValueObjects
{
    public struct Point: IEquatable<Point>
    {
        public Guid Id { get; }
        public int Order { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public int Radius { get; }

        
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