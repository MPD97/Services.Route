using System;
using System.Threading.Tasks;

namespace Services.Route.Core.Repositories
{
    public interface IRouteRepository
    {
        Task<Entities.Route> GetAsync(Guid id);
        Task AddAsync(Entities.Route route);
        Task UpdateAsync(Entities.Route route);
    }
}