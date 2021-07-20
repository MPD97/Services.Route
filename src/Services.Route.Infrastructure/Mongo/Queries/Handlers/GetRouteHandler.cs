using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Services.Route.Application.DTO;
using Services.Route.Application.Queries;
using Services.Route.Infrastructure.Mongo.Documents;

namespace Services.Route.Infrastructure.Mongo.Queries.Handlers
{
    public class GetRouteHandler: IQueryHandler<GetRoute, RouteDto>
    {
        private readonly IMongoRepository<RouteDocument, Guid> _routeDocument;

        public GetRouteHandler(IMongoRepository<RouteDocument, Guid> routeDocument)
        {
            _routeDocument = routeDocument;
        }

        public async Task<RouteDto> HandleAsync(GetRoute query)
        {
            var document = await _routeDocument.GetAsync(p => p.Id == query.RouteId);

            return document?.AsDto();
        }
    }
}