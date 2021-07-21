using System;

namespace Services.Route.Application.Exceptions
{
    public class UnauthorizedRouteStatusChangeAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_route_status_change";
        
        public Guid RouteId { get; }
        public Guid UserId { get; }
        

        public UnauthorizedRouteStatusChangeAccessException(Guid routeId, Guid userId)
            : base($"Unauthorized attempt to change route: {routeId} status by user: {userId}.")
        {
            RouteId = routeId;
            UserId = userId;
        }
    }
}