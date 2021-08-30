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
    public class CreateRouteTests
    {
        private Route.Core.Entities.Route Act(Guid id, Guid userId, string name, string description,
            Difficulty difficulty, int length, List<Point> points, params ActivityKind[] activityKinds) =>
            Route.Core.Entities.Route.Create(id, userId, name, description, difficulty, length,
                points, activityKinds);

        [Fact]
        public void given_valid_parameters_route_should_be_created()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = "routeName";
            var description = "route description";
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>
            {
                new (Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
            };
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var route = Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2);

            route.ShouldNotBeNull();
            route.Id.ShouldBe(id);
            route.UserId.ShouldBe(userId);
            route.Name.ShouldBe(name);
            route.Description.ShouldBe(description);
            route.Difficulty.ShouldBe(difficulty);
            route.Length.ShouldBe(length);
            route.Points.ShouldBe(points);
            route.ActivityKind.ShouldBe(activityKind1 | activityKind2);
            route.Events.Count().ShouldBe(1);
            route.Events.Single().ShouldBeOfType<RouteCreated>();
        }
        [Fact]
        public void given_whitespace_in_name_route_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = " ";
            var description = "route description";
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>
            {
                new (Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
            };
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var exception = Record.Exception(() =>
                Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteNameException>();
        }
        [Fact]
        public void given_too_short_name_route_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = new string('a', 5);
            var description = "route description";
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>
            {
                new (Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
            };
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var exception = Record.Exception(() =>
                Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RouteNameTooShortException>();
        }
        [Fact]
        public void given_too_long_name_route_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = new string('a', 101);
            var description = "route description";
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>
            {
                new (Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
            };
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var exception = Record.Exception(() =>
                Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RouteNameTooLongException>();
        }
        [Fact]
        public void given_only_whitespaces_in_description_route_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = "routeName";
            var description = new string(' ', 20);
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>
            {
                new (Guid.NewGuid(), 0, 0.0M, 0.0M, 10),
            };
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var exception = Record.Exception(() =>
                Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRouteDescriptionException>();
        }
        [Fact]
        public void given_empty_point_list_route_should_throw_an_exception()
        {
            var id = new AggregateId();
            var userId = Guid.NewGuid();
            var name = "routeName";
            var description = new string(' ', 20);
            var difficulty = Difficulty.Green;
            var length = 1000;
            var points = new List<Point>();
            var activityKind1 = ActivityKind.Cycling;
            var activityKind2 = ActivityKind.Hike;

            var exception = Record.Exception(() =>
                Act(id, userId, name, description, difficulty, length, points, activityKind1, activityKind2));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRoutePointsCountException>();
        }
    }
}