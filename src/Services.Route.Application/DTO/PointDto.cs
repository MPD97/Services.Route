using System;

namespace Services.Route.Application.DTO
{
    public class PointDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }

        public PointDto()
        {
            
        }

        public PointDto(Guid id, int order, decimal latitude, decimal longitude, int radius)
        {
            Id = id;
            Order = order;
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }
    }
}