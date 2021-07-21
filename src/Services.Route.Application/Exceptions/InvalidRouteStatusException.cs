using System;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Exceptions
{
    public class InvalidRouteStatusException : AppException
    {
        public override string Code { get; } = "invalid_route_status";
        public Guid UserId { get; }
        public Status  Status { get; }

        public InvalidRouteStatusException(Guid userId, Status status) 
            : base($"User with id: {userId} has given invalid route status: {status}.")
        {
            UserId = userId;
            Status = status;
        }
    }
}