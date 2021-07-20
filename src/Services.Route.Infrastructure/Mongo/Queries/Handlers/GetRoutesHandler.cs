using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Services.Route.Application.DTO;
using Services.Route.Application.Queries;
using Services.Route.Infrastructure.Mongo.Documents;

namespace Services.Route.Infrastructure.Mongo.Queries.Handlers
{
    public class GetRoutesHandler: IQueryHandler<GetRoutes, IEnumerable<RouteDto>>
    {
        private readonly IMongoRepository<RouteDocument, Guid> _routeDocument;

        public GetRoutesHandler(IMongoRepository<RouteDocument, Guid> routeDocument)
        {
            _routeDocument = routeDocument;
        }

        public async Task<IEnumerable<RouteDto>> HandleAsync(GetRoutes query)
        {
            var routes = await _routeDocument.FindAsync(_ => true);

            return routes.Select(r => Documents.Extensions.AsDto(r));
        }
    }
}