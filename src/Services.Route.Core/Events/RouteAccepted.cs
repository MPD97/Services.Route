using System;

namespace Services.Route.Core.Events
{
    public class RouteAccepted : IDomainEvent
    {
        public Entities.Route Route { get; }
        
        public RouteAccepted(Entities.Route route)
        {
            Route = route;
        }
    }
}