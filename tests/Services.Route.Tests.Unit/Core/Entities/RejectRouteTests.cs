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
    public class RejectRouteTests
    {
        private void Act(Route.Core.Entities.Route route, Guid rejectBy) => route.Reject(rejectBy);

        [Fact]
        public void given_route_with_new_status_should_reject_the_route()
        {
            var entryState = Status.New;
            var expectedState = Status.Rejected;
            var rejectedBy = Guid.NewGuid();
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);

            Act(route, rejectedBy);
            
            route.Status.ShouldBe(expectedState);
            route.RejcetedById.ShouldBe(rejectedBy);
            route.Events.Count().ShouldBe(1);
            route.Events.Single().ShouldBeOfType<RouteStatusChanged>();
        }
        [Fact]
        public void given_route_with_not_new_status_should_throw_an_exception()
        {
            var entryState = Status.Unknown;
            var rejectedBy = Guid.NewGuid();
            var route = new Route.Core.Entities.Route(Guid.NewGuid(), Guid.NewGuid(), null, null, "routeName", "",
                Difficulty.Green, entryState, 1000, new List<Point>
                {
                    new(Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
                }, ActivityKind.Walking);
            
            var exception = Record.Exception(() => Act(route, rejectedBy));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotRejectRouteWithThisStatusException>();
        }
    }
}