using System;
using System.Collections.Generic;
using System.Linq;
using Services.Route.Core.Events;
using Services.Route.Core.Exceptions;
using Services.Route.Core.ValueObjects;

namespace Services.Route.Core.Entities
{
    public class Route: AggregateRoot
    {
        private static readonly int MaxPossibleValue = Enum.GetValues(typeof(ActivityKind)).Cast<int>().Sum();
        
        private ISet<Point> _points = new HashSet<Point>();
        public Guid UserId { get; private set; }
        public Guid? AcceptedById { get; private set; }
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

        public Route(Guid id, Guid userId, Guid? acceptedById, string name, string description, Difficulty difficulty, 
             Status status, int length, IEnumerable<Point> points)
        {
            Id = id;
            UserId = userId;
            AcceptedById = acceptedById;
            Name = IsValidName(name) ? name : throw new InvalidRouteNameException(name);
            Description = IsValidDescription(description)
                ? description
                : throw new InvalidRouteDescriptionException(description);
            Difficulty = difficulty;
            Status = status;
            Length = length;
            Points = points;
        }
        
        public Route(Guid id, Guid userId, Guid? acceptedById, string name, string description, Difficulty difficulty, 
             Status status, int length, IEnumerable<Point> points, params ActivityKind[] activityKinds)
        : this(id, userId, acceptedById, name, description, difficulty, status, length, points)
        {
            AddActivityKind(activityKinds);
        }

        public void Accept(Guid userId)
        {
            if (Status != Status.New)
                throw new CannotAcceptRouteWithThisStatusException(Status, Status.New);
            
            AcceptedById = userId;
            Status = Status.Accepted;
            AddEvent(new RouteAccepted(this));
        }

        public void Reject(Guid userId)
        {
            if (Status != Status.New)
                throw new CannotRejectRouteWithThisStatusException(Status, Status.New);
            
            SetStatus(Status.Rejected);
        }
        
        public void Remove(Guid userId)
        {
            if (Status != Status.Accepted)
                throw new CannotRemoveRouteWithThisStatusException(Status, Status.Accepted);
            
            SetStatus(Status.Rejected);
        }
        
        private void SetStatus(Status status)
        {
            var previousStatus = Status;
            Status = status;
            AddEvent(new RouteStatusChanged(this, previousStatus));
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
                if ((int)kind > MaxPossibleValue || (int)kind < 0)
                {
                    throw new InvalidRouteActivityKindException((int) kind, 0, MaxPossibleValue);
                }
                ActivityKind |= kind;
            }
        }
        
        public void RemoveActivityKind(params ActivityKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                if ((int)kind > MaxPossibleValue || (int)kind < 0)
                {
                    throw new InvalidRouteActivityKindException((int) kind, 0, MaxPossibleValue);
                }
                ActivityKind &= ~kind;
            }
        }
    }
}