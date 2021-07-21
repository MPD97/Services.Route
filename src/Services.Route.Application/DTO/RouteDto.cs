using System;
using System.Collections.Generic;
using Services.Route.Core.Entities;

namespace Services.Route.Application.DTO
{
    public class RouteDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public int Length { get; set; }
        public ActivityKind ActivityKind  { get; set; }
        public IEnumerable<PointDto> Points { get; set; }

        public RouteDto()
        {
            
        }

        public RouteDto(Guid id, Guid userId, string name, string description, string difficulty, int length, ActivityKind activityKind, IEnumerable<PointDto> points)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Length = length;
            ActivityKind = activityKind;
            Points = points;
        }
    }
}