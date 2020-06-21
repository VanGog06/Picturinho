using Picturinho.Common.Services;
using Picturinho.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IUserService : IScopedService
    {
        Task<User> AuthenticateAsync(string username, string password);

        IEnumerable<User> GetAll();

        Task<User> GetByIdAsync(int id);

        Task<User> CreateAsync(User user, string password);

        Task UpdateAsync(User userParam, string password = null);

        Task DeleteAsync(int id);
    }
}