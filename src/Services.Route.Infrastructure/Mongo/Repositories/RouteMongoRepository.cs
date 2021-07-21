using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Services.Route.Core.Repositories;
using Services.Route.Infrastructure.Mongo.Documents;

namespace Services.Route.Infrastructure.Mongo.Repositories
{
    public class RouteMongoRepository : IRouteRepository
    {
        private readonly IMongoRepository<RouteDocument, Guid> _repository;

        public RouteMongoRepository(IMongoRepository<RouteDocument, Guid> repository)
        {
            _repository = repository;
        }


        public async Task<Core.Entities.Route> GetAsync(Guid id)
        {
            var route = await _repository.GetAsync(o => o.Id == id);

            return route?.AsEntity();
        }

        public async Task<bool> ExistsByNameAsync(string name)
            => await _repository.ExistsAsync(r => r.Name == name);
            

        public async Task<Core.Entities.Route> GetByNameAsync(string name)
        {
            var route = await _repository.FindAsync(r => r.Name == name);

            return route?.Single().AsEntity();
        }

        public Task AddAsync(Core.Entities.Route route) => _repository.AddAsync(route.AsDocument());

        public Task UpdateAsync(Core.Entities.Route route) => _repository.UpdateAsync(route.AsDocument());
    }
}