using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Services.Route.Application.Exceptions;
using Services.Route.Application.Services;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;

namespace Services.Route.Application.Commands.Handlers
{
    public class ChangeRouteStatusHandler : ICommandHandler<ChangeRouteStatus>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public ChangeRouteStatusHandler(IRouteRepository routeRepository,
            IEventMapper eventMapper, IMessageBroker messageBroker, IAppContext appContext)
        {
            _routeRepository = routeRepository;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }


        public async Task HandleAsync(ChangeRouteStatus command)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated || !identity.IsAdmin)
            {
                throw new UnauthorizedRouteStatusChangeAccessException(command.RouteId, identity.Id);
            }
            
            var route = await _routeRepository.GetAsync(command.RouteId);
            if (route is null)
            {
                throw new RouteNotFoundException(identity.Id, command.RouteId);
            }
            
            if (!Enum.TryParse<Status>(command.Status, true, out var newStatus))
            {
                throw new InvalidRouteStatusException(identity.Id, Status.Unknown);
            }

            switch (newStatus)
            {
                case Status.Accepted:
                    route.Accept(identity.Id);
                    break;
                
                case Status.Rejected:
                    route.Reject(identity.Id);
                    break;
                
                case Status.Removed: 
                    route.Remove();
                    break;
                
                case Status.Unknown:
                    throw new InvalidRouteStatusException(identity.Id, Status.Unknown);
                
                case Status.New:
                    throw new InvalidRouteStatusException(identity.Id, Status.New);
                
                default:
                    throw new InvalidRouteStatusException(identity.Id, Status.Unknown);
            }
            
            await _routeRepository.UpdateAsync(route);
            var events = _eventMapper.MapAll(route.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}