using System.Collections.Generic;
using Convey.CQRS.Queries;
using Services.Route.Application.DTO;

namespace Services.Route.Application.Queries
{
    public class GetRoutes: IQuery<IEnumerable<RouteDto>>
    {

    }
}