using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Services.Route.Application;
using Services.Route.Application.Commands;
using Services.Route.Application.Commands.Handlers;
using Services.Route.Application.Exceptions;
using Services.Route.Application.Services;
using Services.Route.Core.Entities;
using Services.Route.Core.Repositories;
using Services.Route.Core.ValueObjects;
using Services.Route.Infrastructure.Contexts;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.Unit.Application.Handlers
{
    public class ChangeRouteStatusHandlerTests
    {
        private Task Act(ChangeRouteStatus command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_not_authorized_user_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "accepted");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), "User", false,
                new Dictionary<string, string>()));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UnauthorizedRouteStatusChangeAccessException>();
        }

        [Fact]
        public async Task given_user_is_not_admin_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "accepted");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), "User", true,
                new Dictionary<string, string>()));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UnauthorizedRouteStatusChangeAccessException>();
        }

        [Fact]
        public async Task given_not_existing_route_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "accepted");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), "Admin", true,
                new Dictionary<string, string>()));
            _routeRepository.GetAsync(command.RouteId).Returns(Task.FromResult<Route.Core.Entities.Route>(null));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RouteNotFoundException>();
        }

        [Fact]
        public async Task given_invalid_route_status_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "SomeUnknownStatus");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), "Admin", true,
                new Dictionary<string, string>()));
            _routeRepository.GetAsync(command.RouteId).Returns(Task.FromResult(
                new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                    Difficulty.Green, Status.New, 1000, new List<Point>
                    {
                        new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10)
                    }, ActivityKind.Cycling)));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteStatusException>();
        }

        [Fact]
        public async Task given_new_rotue_status_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "new");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), "Admin", true,
                new Dictionary<string, string>()));
            _routeRepository.GetAsync(command.RouteId).Returns(Task.FromResult(
                new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                    Difficulty.Green, Status.New, 1000, new List<Point>
                    {
                        new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10)
                    }, ActivityKind.Cycling)));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteStatusException>();
        }

        [Fact]
        public async Task given_unknown_rotue_status_should_throw_an_exception()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "unknown");

            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"), 
                "Admin", true,new Dictionary<string, string>()));
            _routeRepository.GetAsync(command.RouteId).Returns(Task.FromResult(
                new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, 
                    null, "routeName", "",
                    Difficulty.Green, Status.New, 1000, new List<Point>
                    {
                        new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10)
                    }, ActivityKind.Cycling)));

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteStatusException>();
        }

        [Fact]
        public async Task given_valid_command_parameters_should_change_status()
        {
            var command = new ChangeRouteStatus(Guid.NewGuid(), "accepted");
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null,
                null, "routeName", "", Difficulty.Green, Status.New, 1000, 
                new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10)
                }, ActivityKind.Cycling);
            _appContext.Identity.Returns(new IdentityContext(Guid.NewGuid().ToString("N"),
                "Admin", true,
                new Dictionary<string, string>()));
            _routeRepository.GetAsync(command.RouteId).Returns(Task.FromResult(route));

            await Act(command);

            route.Status.ShouldBe(Status.Accepted);
            await _routeRepository.Received().UpdateAsync(route);
        }

        #region Arrange

        private readonly ChangeRouteStatusHandler _handler;
        private readonly IRouteRepository _routeRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;

        public ChangeRouteStatusHandlerTests()
        {
            _routeRepository = Substitute.For<IRouteRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _appContext = Substitute.For<IAppContext>();

            _handler = new ChangeRouteStatusHandler(_routeRepository, _eventMapper, _messageBroker, _appContext);
        }

        #endregion
    }
}