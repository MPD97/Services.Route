using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convey;
using Convey.MessageBrokers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services.Route.Infrastrucute.Contexts;

namespace Services.Route.Infrastrucute
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
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
    }
}