using System;

namespace Services.Route.Application.Exceptions
{
    public class InvalidPointRadiusException: AppException
    {
        public override string Code { get; } = "invalid_point_radius";
        public Guid UserId { get; }
        public int Radius { get; }
        public int MinRadius { get; }
        public int MaxRadius { get; }

        public InvalidPointRadiusException(Guid userId, int radius, int minRadius, int maxRadius) 
            : base($"User with id: {userId} has given point with invalid radius: {radius}." +
                   $" Point radius must be between {minRadius} and {maxRadius} meters.")
        {
            UserId = userId;
            Radius = radius;
            MinRadius = minRadius;
            MaxRadius = maxRadius;
        }
       
       
    }
}