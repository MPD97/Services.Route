using Services.Route.Application;

namespace Services.Route.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}