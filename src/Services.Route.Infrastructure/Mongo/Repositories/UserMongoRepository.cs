using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Services.Route.Core.Repositories;
using Services.Route.Infrastructure.Mongo.Documents;

namespace Services.Route.Infrastructure.Mongo.Repositories
{
    public class UserMongoRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserMongoRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await _repository.ExistsAsync(u => u.Id == id);

        public async Task<Core.Entities.User> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(u => u.Id == id);

            return user?.AsEntity();
        }

        public Task AddAsync(Core.Entities.User user) => _repository.AddAsync(user.AsDocument());
        public Task UpdateAsync(Core.Entities.User user) => _repository.UpdateAsync(user.AsDocument());
    }
}