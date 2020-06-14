using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Models;
using Picturinho.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Picturinho.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext db;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public UserService(DataContext db, IOptions<AppSettings> appSettings, ILogger<UserService> logger)
        {
            this.db = db;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            this.logger.LogInformation($"Retrieving {nameof(User)} with {nameof(model.Username)} = {model.Username} from the database");
            User user = await this.db.Users.SingleOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                this.logger.LogError($"{nameof(User)} with {nameof(model.Username)} = {model.Username} not found");
                return null;
            }

            this.logger.LogInformation($"Generating JWT token for {nameof(model.Username)} = {model.Username}");
            string jwtToken = this.generateJwtToken(user);

            this.logger.LogInformation($"Generating {nameof(RefreshToken)} for {nameof(model.Username)} = {model.Username}");
            RefreshToken refreshToken = this.generateRefreshToken(ipAddress);

            user.RefreshTokens.Add(refreshToken);

            this.logger.LogInformation($"Updating {nameof(User)} entity");
            this.db.Update(user);
            await this.db.SaveChangesAsync();

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public IEnumerable<User> GetAll()
        {
            return this.db.Users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await this.db.Users.FindAsync(id);
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            this.logger.LogInformation($"Retrieving {nameof(User)} with {nameof(RefreshToken)} = {token}");
            User user = await this.db.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                this.logger.LogError($"{nameof(User)} with with {nameof(RefreshToken)} = {token} not found");
                return null;
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                this.logger.LogError($"Retrieved {nameof(RefreshToken)} is not active");
                return null;
            }

            this.logger.LogInformation($"Generating new {nameof(RefreshToken)} and adding it to {nameof(User)} {nameof(User.RefreshTokens)}");
            RefreshToken newRefreshToken = this.generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTimeOffset.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            this.logger.LogInformation($"Updating {nameof(User)} entity");
            this.db.Update(user);
            await this.db.SaveChangesAsync();

            this.logger.LogInformation("Generating new JWT token");
            string jwtToken = this.generateJwtToken(user);
            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
        {
            User user = await this.db.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                return false;
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            refreshToken.Revoked = DateTimeOffset.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            this.db.Update(user);
            await this.db.SaveChangesAsync();

            return true;
        }

        private string generateJwtToken(User user)
        {
            this.logger.LogInformation($"Generating JWT token for {nameof(User)} with {nameof(user.Username)} = {user.Username}");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(this.appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            this.logger.LogInformation($"Generating {nameof(RefreshToken)} for {nameof(ipAddress)} = {ipAddress}");
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Created = DateTimeOffset.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}