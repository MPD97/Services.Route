using System.Threading.Tasks;
using Convey.CQRS.Events;
using Services.Route.Application.Exceptions;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;

namespace Services.Route.Application.Events.External.Handlers
{
    public class UserCreatedHandler: IEventHandler<UserCreated>
    {
        private readonly IUserRepository _userRepository;

        public UserCreatedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            if (await _userRepository.ExistsAsync(@event.UserId))
            {
                throw new UserAlreadyExistsException(@event.UserId);
            }

            await _userRepository.AddAsync(new User(@event.UserId, @event.State));
        }
    }
}