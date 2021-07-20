using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Services.Route.Core.Entities;

namespace Services.Route.Application.Commands
{
    [Contract]
    public class CreateRoute : ICommand
    {
        public Guid RouteId { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Description { get; }
        public string Difficulty { get; }
        public ActivityKind ActivityKind  { get; }
        public IEnumerable<CreatePoint> Points { get; }

        public CreateRoute(Guid routeId, Guid userId, string name, string description, string difficulty, ActivityKind activityKind, IEnumerable<CreatePoint> points)
        {
            RouteId = routeId == Guid.Empty ? Guid.NewGuid() : routeId;
            UserId = userId;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            ActivityKind = activityKind;
            Points = points;
        }
        public class CreatePoint
        {
            public Guid PointId { get; }
            public int Order { get; }
            public decimal Latitude { get; }
            public decimal Longitude { get; }
            public int Radius { get; }

            public CreatePoint(Guid pointId, int order, decimal latitude, decimal longitude, int radius)
            {
                PointId = pointId == Guid.Empty ? Guid.NewGuid() : pointId;
                Order = order;
                Latitude = latitude;
                Longitude = longitude;
                Radius = radius;
            }
        }
    }
}