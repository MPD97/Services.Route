namespace Services.Route.Core.Exceptions
{
    public class InvalidRouteDescriptionException : DomainException
    {
        public override string Code { get; } = "invalid_route_description";
        public string Description { get; }

        public InvalidRouteDescriptionException(string description) 
            : base($"Route description: {description} is not valid.")
        {
            Description = description;
        }
    }
}