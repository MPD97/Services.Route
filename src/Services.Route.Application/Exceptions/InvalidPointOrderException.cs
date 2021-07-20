using System;

namespace Services.Route.Application.Exceptions
{
    public class InvalidPointOrderException: AppException
    {       
        public override string Code { get; } = "invalid_point_order";
        public Guid UserId { get; }
        public string PointsPointsOrder { get; }
        
        public InvalidPointOrderException(Guid userId, string pointsOrder) 
            : base($"User with id: {userId} has given invalid point order: {pointsOrder}.")

        {
            UserId = userId;
            PointsPointsOrder = pointsOrder;
        }
    }
}