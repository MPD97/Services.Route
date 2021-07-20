using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Services.Route.Application;
using Services.Route.Application.Commands;
using Services.Route.Application.DTO;
using Services.Route.Application.Queries;
using Services.Route.Infrastructure;

namespace Services.Route.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetRoute, RouteDto>("routes/{routeId}")
                        .Get<GetRoutes, IEnumerable<RouteDto>>("routes")
                        .Post<CreateRoute>("routes",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"routes/{cmd.RouteId}"))))
                .UseLogging()
                .UseVault();
    }
}