using System;
using Convey.CQRS.Commands;

namespace Services.Route.Application.Commands
{
    [Contract]
    public class ChangeRouteStatus : ICommand
    {
        public Guid RouteId { get; }
        public string Status { get; }

        public ChangeRouteStatus(Guid routeId, string status)
        {
            RouteId = routeId;
            Status = status;
        }
    }
}