using Services.Route.Core.Entities;

namespace Services.Route.Core.Exceptions
{
    public class CannotRejectRouteWithThisStatusException : DomainException
    {
        public override string Code { get; } = "cannot_reject_route_with_this_status";
        public Status PreviusStatus { get; set; }
        public Status AvailableStatus { get; set; }

        public CannotRejectRouteWithThisStatusException(Status previusStatus, Status availableStatus) 
            : base($"Route cannot be rejected when its status is: {previusStatus}." +
                   $" Route status must be: {availableStatus}.")
        {
            PreviusStatus = previusStatus;
            AvailableStatus = availableStatus;
        }
    }
}