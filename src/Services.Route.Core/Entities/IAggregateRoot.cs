using System.Collections.Generic;

namespace Services.Route.Core.Entities
{
    public interface IAggregateRoot
    {
        IEnumerable<IDomainEvent> Events { get; }
        AggregateId Id { get;  }
    }
}