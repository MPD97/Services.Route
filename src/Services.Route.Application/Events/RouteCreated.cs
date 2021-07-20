using Convey.CQRS.Events;

namespace Services.Route.Application.Events
{
    [Contract]
    public class RouteCreated : IEvent
    {
        public Core.Entities.Route Route { get; }

        public RouteCreated(Core.Entities.Route route)
        {
            Route = route;
        }
    }
}