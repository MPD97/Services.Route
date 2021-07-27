using System;
using System.Linq;
using Services.Route.Application.DTO;
using Services.Route.Core.Entities;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Core.Entities.Route AsEntity(this RouteDocument document)
            => new Core.Entities.Route(document.Id, document.UserId, document.AcceptedBy, document.Name, document.Description,
                document.Difficulty, document.Status, document.Length, 
                document.Points.Select(p => 
                    new Point(p.Id, p.Order, p.Latitude, p.Longitude, p.Radius)), document.Latitude, document.Longitude, document.ActivityKind);

        public static RouteDocument AsDocument(this Core.Entities.Route entity)
            => new RouteDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AcceptedBy = entity.AcceptedById,
                Name = entity.Name,
                Description = entity.Description,
                Difficulty = entity.Difficulty,
                Status = entity.Status,
                Length = entity.Length,
                ActivityKind = entity.ActivityKind,
                Points = entity.Points.Select(p => new PointDocument()
                {
                    Id = p.Id,
                    Order = p.Order,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Radius = p.Radius
                }),
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
            };

        public static RouteDto AsDto(this RouteDocument document)
            => new RouteDto
            {
                Id = document.Id,
                UserId = document.UserId,
                AcceptedBy = document.AcceptedBy,
                Name = document.Name,
                Description = document.Description,
                Difficulty = document.Difficulty.ToString(),
                Status = document.Status.ToString(),
                Length = document.Length,
                ActivityKind = document.ActivityKind,
                Points = document.Points.Select(p => new PointDto()
                {
                    Id = p.Id,
                    Order = p.Order,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Radius = p.Radius
                })
            };
        
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.State);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                State = entity.State
            };
    }
}