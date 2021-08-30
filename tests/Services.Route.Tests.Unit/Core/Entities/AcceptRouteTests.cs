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
    public class AcceptRouteTests
    {
        private void Act(Route.Core.Entities.Route route, Guid acceptBy) => route.Accept(acceptBy);
        
        [Fact]
        public void given_route_with_new_status_should_accept_the_route()
        {
            var entryState = Status.New;
            var expectedState = Status.Accepted;
            var acceptedBy = Guid.NewGuid();
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);

            Act(route, acceptedBy);
            
            route.Status.ShouldBe(expectedState);
            route.AcceptedById.ShouldBe(acceptedBy);
            route.Events.Count().ShouldBe(1);
            route.Events.Single().ShouldBeOfType<RouteAccepted>();
        }
        [Fact]
        public void given_route_with_not_new_status_should_throw_an_exception()
        {
            var entryState = Status.Unknown;
            var acceptedBy = Guid.NewGuid();
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);

            var exception = Record.Exception(() => Act(route, acceptedBy));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotAcceptRouteWithThisStatusException>();
        }
    }
}