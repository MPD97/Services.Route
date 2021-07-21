using Services.Route.Core.Entities;

namespace Services.Route.Core.Exceptions
{
    public class CannotAcceptRouteWithThisStatusException : DomainException
    {
        public override string Code { get; } = "cannot_accept_route_with_this_status";
        public Status PreviusStatus { get; set; }
        public Status AvailableStatus { get; set; }

        public CannotAcceptRouteWithThisStatusException(Status previusStatus, Status availableStatus) 
            : base($"Route cannot be accepted when its status is: {previusStatus}." +
                   $" Route status must be: {availableStatus}.")
        {
            PreviusStatus = previusStatus;
            AvailableStatus = availableStatus;
        }
    }
}