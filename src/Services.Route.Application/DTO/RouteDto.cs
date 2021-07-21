using System;
using System.Collections.Generic;
using Services.Route.Core.Entities;

namespace Services.Route.Application.DTO
{
    public class RouteDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? AcceptedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public int Length { get; set; }
        public string Status { get; set; }
        public ActivityKind ActivityKind  { get; set; }
        public IEnumerable<PointDto> Points { get; set; }

        public RouteDto()
        {
            
        }

        public RouteDto(Guid id, Guid userId, Guid? acceptedBy, string name, string description, string difficulty,
            int length, string status, ActivityKind activityKind, IEnumerable<PointDto> points)
        {
            Id = id;
            UserId = userId;
            AcceptedBy = acceptedBy;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Length = length;
            Status = status;
            ActivityKind = activityKind;
            Points = points;
        }
    }
}