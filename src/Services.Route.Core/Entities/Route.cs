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
        private static readonly int MaxPossibleActivityKindValue = Enum.GetValues(typeof(ActivityKind)).Cast<int>().Sum();
        
        private ISet<Point> _points = new HashSet<Point>();
        public Guid UserId { get; private set; }
        public Guid? AcceptedById { get; private set; }
        public Guid? RejcetedById { get; private set; }
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
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        
        public Route(Guid id, Guid userId, Guid? acceptedById, Guid? rejcetedById, string name, string description, Difficulty difficulty, 
            Status status, int length, List<Point> points, params ActivityKind[] activityKinds)
            : this(id, userId, acceptedById, rejcetedById, name, description, difficulty, status, length, points)
        {
            AddActivityKind(activityKinds);
        }
        
        public Route(Guid id, Guid userId, Guid? acceptedById, Guid? rejcetedById, string name, string description, Difficulty difficulty, 
             Status status, int length, List<Point> points)
        {
            if (points.Count == 0)
                throw new InvalidRoutePointsCountException();              
            
            Id = id;
            UserId = userId;
            AcceptedById = acceptedById;
            RejcetedById = rejcetedById;
            Name = IsValidName(name) ? name : throw new InvalidRouteNameException(name);
            Description = IsValidDescription(description)
                ? description
                : throw new InvalidRouteDescriptionException(description);
            Difficulty = difficulty;
            Status = status;
            Length = length;
            Points = points;
            Latitude = points[0].Latitude;
            Longitude = points[0].Longitude;
        }
        private static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

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
            if (string.IsNullOrEmpty(description))
                return true;

            if (description.All(char.IsWhiteSpace))
                return false;

            const int maxLength = 1024;
            if (description.Length > maxLength)
                throw new RouteDescriptionTooLongException(description, maxLength);

            return true;
        }
        
        public static Route Create(Guid id, Guid userId, string name, string description,
            Difficulty difficulty, int length, List<Point> points,
            params ActivityKind[] activityKinds)
        {
            var route = new Route(id, userId, null, null, name, description, difficulty, Status.New, length,
                points, activityKinds);
            
            route.AddEvent(new RouteCreated(route));
            return route;
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
            
            RejcetedById = userId;
            SetStatus(Status.Rejected);
        }
        
        public void Remove()
        {
            if (Status != Status.Accepted)
                throw new CannotRemoveRouteWithThisStatusException(Status, Status.Accepted);
            
            SetStatus(Status.Removed);
        }
        
        private void SetStatus(Status status)
        {
            var previousStatus = Status;
            Status = status;
            AddEvent(new RouteStatusChanged(this, previousStatus));
        }
        
        private void ChangeActivityKind(ActivityKind kind)
            => ActivityKind = kind;

        private void AddActivityKind(params ActivityKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                if ((int)kind > MaxPossibleActivityKindValue || (int)kind < 0)
                {
                    throw new InvalidRouteActivityKindException((int) kind, 0, MaxPossibleActivityKindValue);
                }
                ActivityKind |= kind;
            }
        }
        
        private void RemoveActivityKind(params ActivityKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                if ((int)kind > MaxPossibleActivityKindValue || (int)kind < 0)
                {
                    throw new InvalidRouteActivityKindException((int) kind, 0, MaxPossibleActivityKindValue);
                }
                ActivityKind &= ~kind;
            }
        }
    }
}