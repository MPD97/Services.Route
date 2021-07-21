namespace Services.Route.Core.Exceptions
{
    public class InvalidRouteActivityKindException : DomainException
    {
        public override string Code { get; } = "invalid_route_activity_kind";
        public int ActivityKind { get; }
        public int MinValue { get; }
        public int MaxValue { get; }

        public InvalidRouteActivityKindException(int activityKind, int minValue, int maxValue) 
            : base($"Route activity kind: {activityKind} is not a valid kind." +
                   $" Minimum value is: {minValue} and max value is: {maxValue}.")
        {
            ActivityKind = activityKind;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}