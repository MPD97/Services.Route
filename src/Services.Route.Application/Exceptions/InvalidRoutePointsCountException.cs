using System;

namespace Services.Route.Application.Exceptions
{
    public class InvalidRoutePointsCountException : AppException
    {
        public override string Code { get; } = "invalid_route_points_count";
        public Guid UserId { get; }
        public int MinCount { get; }
        public int Count { get; }

        public InvalidRoutePointsCountException(Guid userId, int minCount, int count) 
            : base($"User with id: {userId} has given not enough number of points: {count}." +
                   $" Minimum number of points is: {minCount}")
        {
            UserId = userId;
            MinCount = minCount;
            Count = count;
        }
    }
}