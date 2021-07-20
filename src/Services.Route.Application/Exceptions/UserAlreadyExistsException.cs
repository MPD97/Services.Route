using System;

namespace Services.Route.Application.Exceptions
{
    public class UserAlreadyExistsException : AppException
    {
        public override string Code { get; } = "user_already_exists";
        public Guid UserId { get; }

        public UserAlreadyExistsException(Guid userId) 
            : base($"User with id: {userId} already exists.")
        {
            UserId = userId;
        }
    }
}