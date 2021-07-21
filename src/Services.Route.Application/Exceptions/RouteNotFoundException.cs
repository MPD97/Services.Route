using System;

namespace Services.Route.Application.Exceptions
{
    public class RouteNotFoundException: AppException
    {       
        public override string Code { get; } = "route_not_found";
        public Guid UserId { get; }
        public Guid RouteId { get; }
        
        public RouteNotFoundException(Guid userId, Guid routeId) 
            : base($"User with id: {userId} has given not existing route: {routeId}.")
        {
            UserId = userId;
            RouteId = routeId;
        }
    }
}