namespace Services.Route.Core.Exceptions
{
    public class RouteNameTooShortException : DomainException
    {
        public override string Code { get; } = "route_name_too_short";
        public string Name { get; }
        public int MinLength { get; }

        public RouteNameTooShortException(string name, int minLength) 
            : base($"Route name: {name} is too short. Min length is: {minLength}.")
        {
            Name = name;
            MinLength = minLength;
        }
    }
}