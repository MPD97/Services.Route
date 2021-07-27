using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Route.Application;
using Services.Route.Application.Commands;
using Services.Route.Application.Events.External;
using Services.Route.Application.Services;
using Services.Route.Core.Repositories;
using Services.Route.Infrastructure.Contexts;
using Services.Route.Infrastructure.Decorators;
using Services.Route.Infrastructure.Exceptions;
using Services.Route.Infrastructure.Logging;
using Services.Route.Infrastructure.Mongo.Documents;
using Services.Route.Infrastructure.Mongo.Repositories;
using Services.Route.Infrastructure.Services;

namespace Services.Route.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IUserRepository, UserMongoRepository>();
            builder.Services.AddTransient<IRouteRepository, RouteMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IDistanceMeasure, DistanceMesure>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            
            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<UserDocument, Guid>("users")
                .AddMongoRepository<RouteDocument, Guid>("routes")
                .AddWebApiSwaggerDocs()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
                .UsePublicContracts<ContractAttribute>()
                .UseMetrics()
                .UseRabbitMq()
                .SubscribeCommand<CreateRoute>()
                .SubscribeCommand<ChangeRouteStatus>()
                .SubscribeEvent<UserCreated>()
                .SubscribeEvent<UserStateChanged>();
            
            return app;         
        } 
        
        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
        {
            var headers = accessor.HttpContext?.Request.Headers;
            if (headers != null && headers.TryGetValue("Correlation-Context", out var json))
            {
                return JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault());
            }

            return null;
        }
        
        internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                ? null
                : new Dictionary<string, object>
                {
                    [sagaHeader] = saga
                };
        }
        
        internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
        public static IEnumerable<T> GetUniqueFlags<T>(this T flags)
            where T : Enum  
        {
            foreach (Enum value in Enum.GetValues(flags.GetType()))
                if (flags.HasFlag(value))
                    yield return (T)value;
        }
    }
}