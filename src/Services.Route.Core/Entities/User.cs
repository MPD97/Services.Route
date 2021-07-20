using System;

namespace Services.Route.Core.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        
        public State State { get; private set; }

        public User(Guid id, State state)
        {
            Id = id;
            State = state;
        }

        public void ChangeState(State state)
            => State = state;
    }
}