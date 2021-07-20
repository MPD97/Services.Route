using System;

namespace Services.Route.Application.DTO
{
    public class PointDto
    {
        public Guid PointId { get; set; }
        public int Order { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }

        public PointDto()
        {
            
        }

        public PointDto(Guid pointId, int order, decimal latitude, decimal longitude, int radius)
        {
            PointId = pointId;
            Order = order;
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }
    }
}