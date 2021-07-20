using System;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Application.Exceptions
{
    public class InvalidDistanceBetweenPointsException: AppException
    {       
        public override string Code { get; } = "invalid_distance_between_points";
        public Guid UserId { get; }
        public Point FirstPoint { get; }
        public Point SecondPoint { get; }
        public double Distance { get; }
        public int MinDistance { get; }
        public int MaxDistance { get; }


        public InvalidDistanceBetweenPointsException(Guid userId, Point firstPoint,
            Point secondPoint, double distance, int minDistance, int maxDistance) 
            : base($"User with id: {userId} has given points in invalid distance: {distance}." +
                   $" Distance between points must be between {minDistance} and {maxDistance} meters.")
        {
            UserId = userId;
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
            Distance = distance;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
        }
    }
}