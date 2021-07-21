using System;
using Convey.CQRS.Events;

namespace Services.Route.Application.Events
{
    [Contract]
    public class RouteCreated : IEvent
    {
        public Guid RouteId { get; }
        
        public string Status { get; }

        public RouteCreated(Guid routeId, string status)
        {
            RouteId = routeId;
            Status = status;
        }
    }
}