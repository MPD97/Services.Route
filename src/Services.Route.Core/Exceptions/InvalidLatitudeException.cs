namespace Services.Route.Core.Exceptions
{
    public class InvalidLatitudeException : DomainException
    {
        public override string Code { get; } = "invalid_latitude";
        
        public decimal Latitude { get; }

        public InvalidLatitudeException( decimal latitude) 
            : base($"Point has invalid latitude: {latitude}.")
        {
            Latitude = latitude;
        }
    }
}