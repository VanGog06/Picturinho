using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Models.Reaction;
using Picturinho.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Picturinho.Services.Implementations
{
    public class ReactionsService : IReactionsService
    {
        private readonly DataContext db;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReactionsService(DataContext db, ILogger<ReactionsService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TotalReactionsModel> GetLikesAsync(int albumId)
        {
            this.logger.LogInformation($"Retriving the total number of likes for {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            Album album = await db.Albums
                .Include(a => a.Likes)
                .Include(a => a.Loves)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                return new TotalReactionsModel
                {
                    Likes = 0,
                    Loves = 0
                };
            }

            return new TotalReactionsModel
            {
                Likes = album.Likes.Count,
                Loves = album.Loves.Count
            };
        }

        public async Task LikeAsync(int albumId)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);

            this.logger.LogInformation($"Checking to see if {nameof(User)} with {nameof(User.Id)} = {userId} already loved {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            UserLove alreadyExistingLove = await db.UserLoves
                .AsNoTracking()
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.AlbumId == albumId);

            if (alreadyExistingLove != null)
            {
                this.logger.LogInformation($"{nameof(User)} with {nameof(User.Id)} already reacted to {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
                db.UserLoves.Remove(alreadyExistingLove);
                await db.SaveChangesAsync();
            }

            this.logger.LogInformation($"Checking to see if {nameof(User)} with {nameof(User.Id)} = {userId} already liked {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            UserLike alreadyExistingLike = await db.UserLikes
                .AsNoTracking()
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.AlbumId == albumId);

            if (alreadyExistingLike == null)
            {
                this.logger.LogInformation($"{nameof(User)} with {nameof(User.Id)} = {userId} liked {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
                UserLike like = new UserLike
                {
                    AlbumId = albumId,
                    UserId = userId
                };

                await db.UserLikes.AddAsync(like);
                await db.SaveChangesAsync();
            }
        }

        public async Task LoveAsync(int albumId)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);

            this.logger.LogInformation($"Checking to see if {nameof(User)} with {nameof(User.Id)} = {userId} already liked {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            UserLike alreadyExistingLike = await db.UserLikes
                .AsNoTracking()
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.AlbumId == albumId);

            if (alreadyExistingLike != null)
            {
                this.logger.LogInformation($"{nameof(User)} with {nameof(User.Id)} already reacted to {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
                db.UserLikes.Remove(alreadyExistingLike);
                await db.SaveChangesAsync();
            }

            this.logger.LogInformation($"Checking to see if {nameof(User)} with {nameof(User.Id)} = {userId} already loved {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            UserLove alreadyExistingLove = await db.UserLoves
                .AsNoTracking()
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.AlbumId == albumId);

            if (alreadyExistingLove == null)
            {
                this.logger.LogInformation($"{nameof(User)} with {nameof(User.Id)} = {userId} loved {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
                UserLove love = new UserLove
                {
                    AlbumId = albumId,
                    UserId = userId
                };

                await db.UserLoves.AddAsync(love);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteUserLikesAsync(int userId)
        {
            this.logger.LogInformation($"Retrieving {nameof(UserLike)} for {nameof(User)} with {nameof(User.Id)} = {userId}");
            IEnumerable<UserLike> userLikes = db.UserLikes.Where(ul => ul.UserId == userId).ToList();

            db.UserLikes.RemoveRange(userLikes);
            await db.SaveChangesAsync();
        }

        public async Task DeleteUserLovesAsync(int userId)
        {
            this.logger.LogInformation($"Retrieving {nameof(UserLove)} for {nameof(User)} with {nameof(User.Id)} = {userId}");
            IEnumerable<UserLove> userLoves = db.UserLoves.Where(ul => ul.UserId == userId).ToList();

            db.UserLoves.RemoveRange(userLoves);
            await db.SaveChangesAsync();
        }
    }
}