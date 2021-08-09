using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Services.Route.Application.DTO;
using Services.Route.Application.Queries;
using Services.Route.Core.Entities;
using Services.Route.Infrastructure.Mongo.Documents;

namespace Services.Route.Infrastructure.Mongo.Queries.Handlers
{
    public class SearchRoutesHandler : IQueryHandler<SearchRoutes, PagedResult<RouteDto>>
    {
        private readonly IMongoRepository<RouteDocument, Guid> _repository;
        private static readonly int MaxPossibleActivityKind = Enum.GetValues(typeof(ActivityKind)).Cast<int>().Sum();

        public SearchRoutesHandler(IMongoRepository<RouteDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<RouteDto>> HandleAsync(SearchRoutes query)
        {
            Expression<Func<RouteDocument, bool>> predicate = _ => true ;

            if (query.Difficulty.HasValue)
                predicate = predicate.And(r => r.Difficulty == query.Difficulty.Value);

            if (query.ActivityKind.HasValue && query.ActivityKind > 0 && (int)query.ActivityKind <= MaxPossibleActivityKind)
                predicate = predicate.And(r => r.ActivityKind == query.ActivityKind);

            if (query.OnlyNew.HasValue && query.OnlyNew.Value)
                predicate = predicate.And(r => r.Status == Status.New);
            else if (query.OnlyAccepted)
                predicate = predicate.And(r => r.Status == Status.Accepted);

            if (query.NorthEastLatitude.HasValue && query.NorthEastLongitude.HasValue
                                                 && query.SouthWestLatitude.HasValue && query.SouthWestLongitude.HasValue)
            {
                predicate = predicate.And(r 
                    => r.Latitude >= query.SouthWestLatitude && r.Latitude <= query.NorthEastLatitude);
                predicate = predicate.And(r
                    => r.Longitude >= query.SouthWestLongitude && r.Longitude <= query.NorthEastLongitude);
            }
            else if (query.TopLeftLatitude.HasValue && query.BottomRightLatitude.HasValue 
                && query.TopLeftLongitude.HasValue && query.BottomRightLongitude.HasValue)
            {
                predicate = predicate.And(r 
                    => r.Latitude <= query.TopLeftLatitude && r.Latitude >= query.BottomRightLatitude);

                predicate = predicate.And(r
                    => r.Longitude >= query.TopLeftLongitude && r.Longitude <= query.BottomRightLongitude);
            }
            
            var pagedResult = await _repository.BrowseAsync(predicate, query);
            return pagedResult?.Map(d => d.AsDto());        
        }
    }
}