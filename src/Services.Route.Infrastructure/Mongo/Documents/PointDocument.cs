using System;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Infrastructure.Mongo.Documents
{
    public sealed class PointDocument
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
    }
}