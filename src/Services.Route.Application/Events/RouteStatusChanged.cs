using System;
using Convey.CQRS.Events;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Events
{
    [Contract]
    public class RouteStatusChanged : IEvent
    {
        public Guid RouteId { get; }
        public Status CurrentStatus { get; }
        public Status PreviousStatus { get; }

        public RouteStatusChanged(Guid routeId, Status currentStatus, Status previousStatus)
        {
            RouteId = routeId;
            CurrentStatus = currentStatus;
            PreviousStatus = previousStatus;
        }
        
    }
}