using System.Collections.Generic;
using Convey.CQRS.Events;
using Services.Route.Core;

namespace Services.Route.Application.Services
{
    public interface IEventMapper
    {
        IEvent Map(IDomainEvent @event);
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
    }
}