namespace Services.Route.Core.Exceptions
{
    public class RouteNameTooLongException : DomainException
    {
        public override string Code { get; } = "route_name_too_long";
        public string Name { get; }
        public int MaxLength { get; }

        public RouteNameTooLongException(string name, int maxLength) 
            : base($"Route name: {name} is too long. Max length is: {maxLength}.")
        {
            Name = name;
            MaxLength = maxLength;
        }
    }
}