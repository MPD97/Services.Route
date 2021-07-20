using System;
using Convey.CQRS.Events;

namespace Services.Route.Application.Events.Rejected
{
    [Contract]
    public class CreateRouteRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CreateRouteRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}