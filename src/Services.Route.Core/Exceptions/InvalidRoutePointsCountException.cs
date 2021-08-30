namespace Services.Route.Core.Exceptions
{
    public class InvalidRoutePointsCountException : DomainException
    {
        public override string Code { get; } = "invalid_route_points_count";

        public InvalidRoutePointsCountException() 
            : base($"Route points count is not valid")
        {
        }
    }
}