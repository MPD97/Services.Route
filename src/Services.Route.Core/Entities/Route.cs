using System;
using System.Collections.Generic;
using Services.Route.Core.Exceptions;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Core.Entities
{
    public class Route: AggregateRoot
    {
        private ISet<Point> _points = new HashSet<Point>();

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public Difficulty Difficulty { get; private set; }

        public ActivityKind ActivityKind  { get; private set; }
        
        public int Length { get; private set; }
        
        public Status Status { get; private set; }

        public IEnumerable<Point> Points
        {
            get => _points;
            private set => _points = new HashSet<Point>(value);
        }

        public Route(Guid id, Guid userId, string name, string description, Difficulty difficulty, 
             Status status, IEnumerable<Point> points)
        {
            Id = id;
            Name = IsValidName(name) ? name : throw new InvalidRouteNameException(name);
            Description = IsValidDescription(description)
                ? description
                : throw new InvalidRouteDescriptionException(description);
            Difficulty = difficulty;
            Status = status;
            Points = points;
        }
        
        public Route(Guid id, Guid userId, string name, string description, Difficulty difficulty, 
             Status status, IEnumerable<Point> points, params ActivityKind[] activityKinds)
        : this(id, userId, name, description, difficulty, status, points)
        {
            AddActivityKind(activityKinds);
        }

        private static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidRouteNameException(name);

            const int maxLength = 100;
            const int minLength = 6;
            return name.Length switch
            {
                > maxLength => throw new RouteNameTooLongException(name, maxLength),
                < minLength => throw new RouteNameTooShortException(name, minLength),
                _ => true
            };
        }
        
        private static bool IsValidDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return true;

            const int maxLength = 1024;
            if (description.Length > maxLength)
                throw new RouteDescriptionTooLongException(description, maxLength);

            return true;
        }
        
        public void ChangeActivityKind(ActivityKind kind)
            => ActivityKind = kind;

        public void AddActivityKind(params ActivityKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                ActivityKind |= kind;
            }
        }
        
        public void RemoveActivityKind(params ActivityKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                ActivityKind &= ~kind;
            }
        }
    }
}