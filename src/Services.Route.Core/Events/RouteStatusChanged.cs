using Services.Route.Core.Entities;

namespace Services.Route.Core.Events
{
    public class RouteStatusChanged : IDomainEvent
    {
        public Entities.Route Route { get; }
        public Status PreviousStatus { get; }
        
        public RouteStatusChanged(Entities.Route route, Status previousStatus)
        {
            Route = route;
            PreviousStatus = previousStatus;
        }
    }
}