using Convey.CQRS.Queries;
using Services.Route.Application.DTO;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Queries
{
    public class SearchRoutes : PagedQueryBase, IQuery<PagedResult<RouteDto>>
    {
        public Difficulty? Difficulty { get; set; } 
        public ActivityKind? ActivityKind { get; set; }
        public decimal? TopLeftLatitude { get; set; }
        public decimal? TopLeftLongitude { get; set; }
        public decimal? BottomRightLatitude { get; set; }
        public decimal? BottomRightLongitude { get; set; }

        public SearchRoutes(Difficulty? difficulty, ActivityKind? activityKind, decimal? topLeftLatitude, decimal? topLeftLongitude, decimal? bottomRightLatitude, decimal? bottomRightLongitude)
        {
            Difficulty = difficulty;
            ActivityKind = activityKind;
            TopLeftLatitude = topLeftLatitude;
            TopLeftLongitude = topLeftLongitude;
            BottomRightLatitude = bottomRightLatitude;
            BottomRightLongitude = bottomRightLongitude;
        }
    }
}