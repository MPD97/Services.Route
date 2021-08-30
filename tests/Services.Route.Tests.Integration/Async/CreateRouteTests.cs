using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Route.Api;
using Services.Route.Application.Commands;
using Services.Route.Core.Entities;
using Services.Route.Infrastructure.Mongo.Documents;
using Services.Route.Tests.Shared.Factories;
using Services.Route.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.Integration.Async
{
    public class CreateRouteTests : IDisposable, IClassFixture<AppliactionFactory<Program>>
    {
        private Task Act(CreateRoute command) => _rabbitMqFixture.PublishAsync(command, Exhange);

        [Fact]
        public async Task create_route_command_should_add_document_to_database()
        {
            var command = new CreateRoute(Guid.NewGuid(), _userDocument.Id, "route name", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 1, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 2, 0.003M, 0.003M, 10),
                    new(Guid.NewGuid(), 3, 0.004M, 0.004M, 10),
                });

            var tcs = _rabbitMqFixture.SubscribeAndGet<Application.Events.RouteCreated, RouteDocument>(
                exchange: Exhange,
                _routeMongoDbFixture.GetAsync, command.RouteId);

            await Act(command);

            var document = await tcs.Task;
            document.ShouldNotBeNull();
            document.Id.ShouldBe(command.RouteId);
            document.Name.ShouldBe(command.Name);
            document.Description.ShouldBe(command.Description);
            document.Difficulty.ShouldBe(Difficulty.Green);
            document.ActivityKind.ShouldBe(ActivityKind.Cycling);
        }

        #region Arrange

        private const string Exhange = "routes";
        private readonly MongoDbFixture<RouteDocument, Guid> _routeMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _userMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly UserDocument _userDocument;


        public CreateRouteTests(AppliactionFactory<Program> factory)
        {
            _userDocument = new UserDocument
                {Id = new Guid("acf5055d-ff9d-47b9-bb6f-3d0a85b018f7"), State = State.Valid};
            _userMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            _userMongoDbFixture.InsertAsync(_userDocument).Wait();
            _routeMongoDbFixture = new MongoDbFixture<RouteDocument, Guid>("routes");
            _rabbitMqFixture = new RabbitMqFixture();
            factory.Server.AllowSynchronousIO = true;
        }

        public void Dispose()
        {
            _routeMongoDbFixture.Dispose();
            _userMongoDbFixture.Dispose();
        }

        #endregion
    }
}