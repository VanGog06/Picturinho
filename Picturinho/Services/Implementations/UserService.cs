using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Picturinho.Common.Services;
using Picturinho.Entities;
using Picturinho.Entities.Enums;
using Picturinho.Helpers;
using Picturinho.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Picturinho.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext db;
        private readonly IReactionsService reactionsService;
        private readonly ILogger logger;

        public UserService(DataContext db, ILogger<UserService> logger, IReactionsService reactionsService)
        {
            this.db = db;
            this.logger = logger;
            this.reactionsService = reactionsService;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            User user = await db.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (db.Users.Any(u => u.Username == user.Username))
            {
                throw new AppException($"Username \"{user.Username}\" is already taken");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == Roles.User);

            if (userRole == null)
            {
                throw new AppException($"User role was not found");
            }

            user.RoleId = userRole.Id;

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(User userParam, string password = null)
        {
            User user = await db.Users.FindAsync(userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }

            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                if (db.Users.Any(u => u.Username == userParam.Username))
                {
                    throw new AppException($"Username {user.Username} is already taken");
                }

                user.Username = userParam.Username;
            }

            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            {
                user.FirstName = userParam.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
            {
                user.LastName = userParam.LastName;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            db.Users.Update(user);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await reactionsService.DeleteUserLikesAsync(id);
            await reactionsService.DeleteUserLovesAsync(id);

            User user = await db.Users.FindAsync(id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (HMACSHA512 hmac = new HMACSHA512(storedSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}