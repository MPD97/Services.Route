using System;
using System.Collections.Generic;
using Convey.Types;
using Services.Route.Core.Entities;

namespace Services.Route.Infrastructure.Mongo.Documents
{
    public sealed class RouteDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? AcceptedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public ActivityKind ActivityKind  { get; set; }
        public int Length { get; set; }
        public Status Status { get; set; }
        public IEnumerable<PointDocument> Points { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}