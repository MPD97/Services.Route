using System;
using Services.Route.Core.Exceptions;
using Services.Route.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.Unit.Core.ValueObjects
{
    public class CreatePointTests
    {
        [Fact]
        public void given_valid_parameters_point_should_be_created()
        {
            var id = Guid.NewGuid();
            var order = 0;
            var latitude = 0.0M;
            var longitude = 0.0M;
            var radius = 10;

            var point = new Point(id, order, latitude, longitude, radius);
            
            point.Id.ShouldBe(id);
            point.Order.ShouldBe(order);
            point.Latitude.ShouldBe(latitude);
            point.Longitude.ShouldBe(longitude);
            point.Radius.ShouldBe(radius);
        }
        
        [Fact]
        public void given_order_less_than_0_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var order = -1;
            var latitude = 0.0M;
            var longitude = 0.0M;
            var radius = 10;

            var exception = Record.Exception(() => new Point(id, order, latitude, longitude, radius));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPointOrderException>();
        }
        
        [Fact]
        public void given_invalid_latitude_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var order = 0;
            var latitude = -91.0M;
            var longitude = 0.0M;
            var radius = 10;

            var exception = Record.Exception(() => new Point(id, order, latitude, longitude, radius));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLatitudeException>();
        }
        
        [Fact]
        public void given_invalid_longitude_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var order = 0;
            var latitude = 0.0M;
            var longitude = -181.0M;
            var radius = 10;

            var exception = Record.Exception(() => new Point(id, order, latitude, longitude, radius));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLongitudeException>();
        }
        
        [Fact]
        public void given_radius_less_than_10_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var order = 0;
            var latitude = 0.0M;
            var longitude = 0.0M;
            var radius = 9;

            var exception = Record.Exception(() => new Point(id, order, latitude, longitude, radius));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRadiusException>();
        }
        
        [Fact]
        public void given_radius_greater_than_100_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var order = 0;
            var latitude = 0.0M;
            var longitude = 0.0M;
            var radius = 101;

            var exception = Record.Exception(() => new Point(id, order, latitude, longitude, radius));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidRadiusException>();
        }
    }
}