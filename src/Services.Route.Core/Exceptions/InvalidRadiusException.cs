using System;

namespace Services.Route.Core.Exceptions
{
    public class InvalidRadiusException : DomainException
    {
        public override string Code { get; } = "invalid_radius";
        
        public decimal Radius { get; }

        public InvalidRadiusException(decimal radius) 
            : base($"Point has invalid radius: {radius}.")
        {
            Radius = radius;
        }
    }
}