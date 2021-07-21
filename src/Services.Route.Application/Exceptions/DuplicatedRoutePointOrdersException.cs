using System;

namespace Services.Route.Application.Exceptions
{
    public class DuplicatedRoutePointOrdersException : AppException
    {
        public override string Code { get; } = "route_points_duplicated_orders";
        public Guid UserId { get; }

        public DuplicatedRoutePointOrdersException(Guid userId)
            : base($"User with id: {userId} has given points with duplicated orders.")
        {
            UserId = userId;
        }
    }
}