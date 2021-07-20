namespace Services.Route.Core.Exceptions
{
    public class InvalidPointOrderException : DomainException
    {
        public override string Code { get; } = "invalid_point_order";
        public int Order { get; }

        public InvalidPointOrderException(int order) 
            : base($"Point order: {order} is not valid.")
        {
            Order = order;
        }
    }
}