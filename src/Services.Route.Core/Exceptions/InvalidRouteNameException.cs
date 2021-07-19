namespace Services.Route.Core.Exceptions
{
    public class InvalidRouteNameException : DomainException
    {
        public override string Code { get; } = "invalid_route_name";
        public string Name { get; }

        public InvalidRouteNameException(string name) 
            : base($"Route name: {name} is not valid.")
        {
            Name = name;
        }
    }
}