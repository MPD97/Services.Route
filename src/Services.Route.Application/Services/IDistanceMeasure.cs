namespace Services.Route.Application.Services
{
    public interface IDistanceMeasure
    {
        double GetDistanceBetween(decimal lat1, decimal lon1, decimal lat2, decimal lon2);
    }
}