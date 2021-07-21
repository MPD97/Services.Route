using System;

namespace Services.Route.Application.Exceptions
{
    public class RouteAlreadyExistsException: AppException
    {       
        public override string Code { get; } = "route_already_exists";
        public Guid UserId { get; }
        public string Name { get; }
        
        public RouteAlreadyExistsException(Guid userId, string name) 
            : base($"User with id: {userId} has given existing route name: {name}.")

        {
            UserId = userId;
            Name = name;
        }
    }
}