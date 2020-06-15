using Picturinho.Entities;
using Picturinho.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);

        IEnumerable<User> GetAll();

        Task<User> GetByIdAsync(int id);

        Task<User> CreateAsync(User user, string password);

        Task UpdateAsync(User userParam, string password = null);

        Task DeleteAsync(int id);
    }
}