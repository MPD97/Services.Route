namespace Services.Route.Core.Events
{
    public class RouteCreated : IDomainEvent
    {
        public Entities.Route Route { get; }
        
        public RouteCreated(Entities.Route route)
        {
            Route = route;
        }
    }
}