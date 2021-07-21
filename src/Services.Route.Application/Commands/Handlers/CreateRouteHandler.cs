using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Services.Route.Application.Events;
using Services.Route.Application.Exceptions;
using Services.Route.Application.Services;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Application.Commands.Handlers
{
    public class CreateRouteHandler : ICommandHandler<CreateRoute>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDistanceMeasure _distanceMeasure;

        public CreateRouteHandler(IRouteRepository routeRepository, IUserRepository userRepository,
             IMessageBroker messageBroker, IDistanceMeasure distanceMeasure)
        {
            _routeRepository = routeRepository;
            _userRepository = userRepository;
            _messageBroker = messageBroker;
            _distanceMeasure = distanceMeasure;
        }

        public async Task HandleAsync(CreateRoute command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
                throw new UserNotFoundException(command.UserId);

            if (user.State != State.Valid)
                throw new InvalidUserStateException(command.UserId, user.State);

            if (await _routeRepository.ExistsByNameAsync(command.Name))
                throw new RouteAlreadyExistsException(command.UserId, command.Name);
            
            if (!Enum.TryParse<Difficulty>(command.Difficulty, true, out var difficulty))
                throw new InvalidRouteDifficultyException(command.UserId, command.Difficulty);

            const int minPointsCount = 4;
            const int maxPointsCount = 35;
            if (command.Points is null)
                throw new InvalidRoutePointsCountException(command.UserId, minPointsCount, maxPointsCount, 0);
            
            var pointsCount = command.Points.Count();
            if (pointsCount is < minPointsCount or > maxPointsCount)
                throw new InvalidRoutePointsCountException(command.UserId, minPointsCount, maxPointsCount, pointsCount);

            var points = new SortedSet<Point>(new ByOrderExtension());
            foreach (var point in command.Points)
            {
                points.Add(new Point(point.PointId, point.Order, point.Latitude, point.Longitude, point.Radius));
            }
            
            var pointsCountAfterSort = points.Count();
            if (pointsCountAfterSort  != pointsCount)
                throw new DuplicatedRoutePointOrdersException(command.UserId);

            
            const int minDistance = 100;
            const int maxDistance = 500;
            double routeLength = 0.0;
            for (int i = 0; i < points.Count; i++)
            {
                var currentPoint = points.ElementAt(i);
                if (currentPoint.Order != i)
                {
                    throw new InvalidPointOrderException(command.UserId, 
                        string.Join(", ",points.Select(p => p.Order)));
                }

                const int minRadius = 10;
                const int maxRadius = 50;
                if (currentPoint.Radius is < minRadius or > maxRadius)
                    throw new InvalidPointRadiusException(command.UserId, currentPoint.Radius,
                        minRadius, maxRadius);
                
                if (i <= 0) continue;
                
                var previousPoint = points.ElementAt(i - 1);
                var distance = _distanceMeasure.GetDistanceBetween(previousPoint.Latitude,
                    previousPoint.Longitude, currentPoint.Latitude, currentPoint.Longitude);

                if (distance is < minDistance or > maxDistance)
                    throw new InvalidDistanceBetweenPointsException(command.UserId, previousPoint,
                        currentPoint, distance, minDistance, maxDistance);

                routeLength += distance;
            }
            
            var route = new Core.Entities.Route(command.RouteId, user.Id, null, command.Name, command.Description,
                difficulty, Status.New, (int)routeLength, points, command.ActivityKind);

            await _routeRepository.AddAsync(route);
            await _messageBroker.PublishAsync(new RouteCreated(route.Id, route.Status.ToString()));
        }
    }
}