using Services.Route.Core.Entities;

namespace Services.Route.Core.Exceptions
{
    public class CannotRemoveRouteWithThisStatusException : DomainException
    {
        public override string Code { get; } = "cannot_remove_route_with_this_status";
        public Status PreviusStatus { get; set; }
        public Status AvailableStatus { get; set; }

        public CannotRemoveRouteWithThisStatusException(Status previusStatus, Status availableStatus) 
            : base($"Route cannot be removed when its status is: {previusStatus}." +
                   $" Route status must be: {availableStatus}.")
        {
            PreviusStatus = previusStatus;
            AvailableStatus = availableStatus;
        }
    }
}