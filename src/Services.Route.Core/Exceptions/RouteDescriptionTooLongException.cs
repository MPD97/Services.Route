namespace Services.Route.Core.Exceptions
{
    public class RouteDescriptionTooLongException : DomainException
    {
        public override string Code { get; } = "route_description_too_long";
        public string Description { get; }
        public int MaxLength { get; }

        public RouteDescriptionTooLongException(string description, int maxLength) 
            : base($"Route description: {description} is too long. Max length is: {maxLength}.")
        {
            Description = description;
            MaxLength = maxLength;
        }
    }
}