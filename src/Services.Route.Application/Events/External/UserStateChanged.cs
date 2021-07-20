using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Services.Route.Application.Events.External
{
    [Message("users")]
    public class UserStateChanged : IEvent
    {
        public Guid UserId { get; }
        public string CurrentState { get; }
        public string PreviousState { get; }

        public UserStateChanged(Guid userId, string currentState, string previousState)
        {
            UserId = userId;
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }
}