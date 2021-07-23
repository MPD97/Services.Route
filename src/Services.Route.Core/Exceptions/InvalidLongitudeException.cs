namespace Services.Route.Core.Exceptions
{
    public class InvalidLongitudeException : DomainException
    {
        public override string Code { get; } = "invalid_longitude";
        
        public decimal Longitude { get; }

        public InvalidLongitudeException( decimal longitude) 
            : base($"Point has invalid longitude: {longitude}.")
        {
            Longitude = longitude;
        }
    }
}