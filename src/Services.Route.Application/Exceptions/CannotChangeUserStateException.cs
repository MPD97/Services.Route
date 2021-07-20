using System;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Exceptions
{
    public class CannotChangeUserStateException : AppException
    {
        public override string Code { get; } = "cannot_change_user_state";
        public Guid UserId { get; }
        public State State { get; }

        public CannotChangeUserStateException(Guid userId, State state) : base(
            $"Cannot change user: {userId} state to: {state}.")
        {
            UserId = userId;
            State = state;
        }
    }
}