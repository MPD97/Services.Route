using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Services.Route.Application.Commands;
using Services.Route.Application.Commands.Handlers;
using Services.Route.Application.Exceptions;
using Services.Route.Application.Services;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.Unit.Application.Handlers
{
    public class CreateRouteHandlerTests
    {
        private Task Act(CreateRoute command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_not_existing_user_id_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UserNotFoundException>();
        }

        [Fact]
        public async Task given_user_with_not_valid_state_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Unknown);
            _userRepository.GetAsync(command.UserId).Returns(user);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUserStateException>();
        }

        [Fact]
        public async Task given_existing_route_name_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(true);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RouteAlreadyExistsException>();
        }

        [Fact]
        public async Task given_not_valid_difficulty_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "SomeInvalidDifficulty", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteDifficultyException>();
        }

        [Fact]
        public async Task given_null_points_list_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, null);
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRoutePointsCountException>();
        }

        [Fact]
        public async Task given_less_then_4_points_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRoutePointsCountException>();
        }

        [Fact]
        public async Task given_greater_than_50_points_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>(
                    Enumerable.Range(0, 51).Select(x => new CreateRoute.CreatePoint(Guid.NewGuid(), 0, 0.0M, 0.0M, 10)))
            );
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRoutePointsCountException>();
        }

        [Fact]
        public async Task given_duplicated_points_order_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 1, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 2, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);

            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DuplicatedRoutePointOrdersException>();
        }

        [Fact]
        public async Task given_points_in_distance_between_less_than_100_meters_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);
            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);
            _distanceMeasure
                .GetDistanceBetween(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>())
                .Returns(99);
            
            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDistanceBetweenPointsException>();
        }
        
        [Fact]
        public async Task given_points_in_distance_greater_than_100_meters_should_throw_an_exception()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);
            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);
            _distanceMeasure
                .GetDistanceBetween(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>())
                .Returns(501);
            
            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidDistanceBetweenPointsException>();
        }

        [Fact]
        public async Task given_valid_command_parameters_route_should_be_created()
        {
            var command = new CreateRoute(Guid.NewGuid(), Guid.NewGuid(), "routeName", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                    new(Guid.NewGuid(), 1, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 2, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 3, 0.003M, 0.003M, 10),
                });
            var user = new User(Guid.NewGuid(), State.Valid);
            _userRepository.GetAsync(command.UserId).Returns(user);
            _routeRepository.ExistsByNameAsync(command.Name).Returns(false);
            _distanceMeasure
                .GetDistanceBetween(Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<decimal>())
                .Returns(150);
            await Act(command);

            await _routeRepository.Received().AddAsync(Arg.Is<Route.Core.Entities.Route>(r => r.Id == command.RouteId));
        }

        #region Arrange

        private readonly CreateRouteHandler _handler;
        private readonly IRouteRepository _routeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IDistanceMeasure _distanceMeasure;

        public CreateRouteHandlerTests()
        {
            _routeRepository = Substitute.For<IRouteRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _distanceMeasure = Substitute.For<IDistanceMeasure>();

            _handler = new CreateRouteHandler(_routeRepository, _userRepository, _messageBroker, _distanceMeasure,
                _eventMapper);
        }

        #endregion
    }
}