using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Services.Route.Application.Services;
using Services.Route.Core;
using Services.Route.Core.Entities;
using Services.Route.Core.Events;

namespace Services.Route.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case RouteCreated e: return new Application.Events.RouteCreated(e.Route.Id, e.Route.Status.ToString());
                case RouteAccepted e: return new Application.Events.RouteStatusChanged(e.Route.Id, e.Route.Status, Status.New);
                case RouteStatusChanged e: return new Application.Events.RouteStatusChanged(e.Route.Id, e.Route.Status, e.PreviousStatus);
            }

            return null;
        }
    }
}