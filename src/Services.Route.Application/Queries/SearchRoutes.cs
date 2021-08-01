using Convey.CQRS.Queries;
using Services.Route.Application.DTO;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Queries
{
    public class SearchRoutes : PagedQueryBase, IQuery<PagedResult<RouteDto>>
    {
        public Difficulty? Difficulty { get; set; } 
        public ActivityKind? ActivityKind { get; set; }
        public bool OnlyAccepted { get; set; } = true;
        public decimal? TopLeftLatitude { get; set; }
        public decimal? TopLeftLongitude { get; set; }
        public decimal? BottomRightLatitude { get; set; }
        public decimal? BottomRightLongitude { get; set; }
        public decimal? NorthEastLatitude { get; set; }
        public decimal? NorthEastLongitude { get; set; }
        public decimal? SouthWestLatitude { get; set; }
        public decimal? SouthWestLongitude { get; set; }
    }
}