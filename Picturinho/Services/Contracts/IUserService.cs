using Picturinho.Entities;
using Picturinho.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);

        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);

        Task<bool> RevokeTokenAsync(string token, string ipAddress);

        IEnumerable<User> GetAll();

        Task<User> GetByIdAsync(int id);
    }
}