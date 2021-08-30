using System;
using System.Collections.Generic;
using System.Linq;
using Services.Route.Core.Entities;
using Services.Route.Core.Events;
using Services.Route.Core.Exceptions;
using Services.Route.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.Unit.Core.Entities
{
    public class RemoveRouteTests
    {
        private void Act(Route.Core.Entities.Route route) => route.Remove();

        [Fact]
        public void given_route_with_accepted_status_should_remove_the_route()
        {
            var entryState = Status.Accepted;
            var expectedState = Status.Removed;
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);

            Act(route);
            
            route.Status.ShouldBe(expectedState);
            route.Events.Count().ShouldBe(1);
            route.Events.Single().ShouldBeOfType<RouteStatusChanged>();
        }
        [Fact]
        public void given_route_with_not_accepted_status_should_throw_an_exception()
        {
            var entryState = Status.Unknown;
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);
            
            var exception = Record.Exception(() => Act(route));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotRemoveRouteWithThisStatusException>();
        }
    }
}