using System;
using System.Threading.Tasks;
using Services.Route.Core.Entities;

namespace Services.Route.Core.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task<User> GetAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}