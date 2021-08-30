using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Route.Api;
using Services.Route.Application.Commands;
using Services.Route.Core.Entities;
using Services.Route.Infrastructure.Mongo.Documents;
using Services.Route.Tests.Shared.Factories;
using Services.Route.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;

namespace Services.Route.Tests.EndToEnd.Sync
{
    public class CreateRouteTests : IDisposable, IClassFixture<AppliactionFactory<Program>>
    {
        private Task<HttpResponseMessage> Act(CreateRoute command) =>
            _httpClient.PostAsync("routes", GetContent(command));

        [Fact]
        public async Task create_resource_endpoint_should_return_http_status_code_created()
        {
            var command = new CreateRoute(Guid.NewGuid(), _userDocument.Id, "route name", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 1, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 2, 0.003M, 0.003M, 10),
                    new(Guid.NewGuid(), 3, 0.004M, 0.004M, 10),
                });

            var response = await Act(command);

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }
        
        [Fact]
        public async Task create_resource_endpoint_should_create_document_in_database()
        {
            var command = new CreateRoute(Guid.NewGuid(), _userDocument.Id, "route name", "",
                "green", ActivityKind.Cycling, new List<CreateRoute.CreatePoint>
                {
                    new(Guid.NewGuid(), 0, 0.001M, 0.001M, 10),
                    new(Guid.NewGuid(), 1, 0.002M, 0.002M, 10),
                    new(Guid.NewGuid(), 2, 0.003M, 0.003M, 10),
                    new(Guid.NewGuid(), 3, 0.004M, 0.004M, 10),
                });
            
            await Act(command);

            var document = await _routeMongoDbFixture.GetAsync(command.RouteId);
            document.ShouldNotBeNull();
            document.Id.ShouldBe(command.RouteId);
            document.Name.ShouldBe(command.Name);
            document.Description.ShouldBe(command.Description);
            document.Difficulty.ShouldBe(Difficulty.Green);
            document.ActivityKind.ShouldBe(ActivityKind.Cycling);
        }
        
        #region Arrange

        private readonly HttpClient _httpClient;
        private readonly MongoDbFixture<RouteDocument, Guid> _routeMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _userMongoDbFixture;
        private readonly UserDocument _userDocument;

        public CreateRouteTests(AppliactionFactory<Program> factory)
        {
            _userDocument = new UserDocument
                {Id = new Guid("f871a198-5e4a-4543-8689-c15086cbff67"), State = State.Valid};
            _routeMongoDbFixture = new MongoDbFixture<RouteDocument, Guid>("routes");
            _userMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _httpClient = factory.CreateClient();
            
            _userMongoDbFixture.InsertAsync(_userDocument).Wait();
        }

        public void Dispose()
        {
            _routeMongoDbFixture.Dispose();
            _userMongoDbFixture.Dispose();
        }
        
        private static StringContent GetContent(object value)
            => new(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

        #endregion
    }
}