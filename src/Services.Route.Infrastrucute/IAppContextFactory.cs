using Services.Route.Application;

namespace Services.Route.Infrastrucute
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}