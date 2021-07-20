using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Services.Route.Application.Exceptions;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;

namespace Services.Route.Application.Events.External.Handlers
{
    public class UserStateChangedHandler : IEventHandler<UserStateChanged>
    {
        private readonly IUserRepository _userRepository;

        public UserStateChangedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UserStateChanged @event)
        {
            var user = await _userRepository.GetAsync(@event.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(@event.UserId);
            }

            if (!Enum.TryParse<State>(@event.CurrentState, true, out var state))
            {
                throw new CannotChangeUserStateException(@event.UserId, State.Unknown);
            }

            if (user.State == state)
            {
                return;
            }

            user.ChangeState(state);
            await _userRepository.UpdateAsync(user);
        }
    }
}