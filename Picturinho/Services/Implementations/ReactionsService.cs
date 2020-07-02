using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Services.Contracts;
using System;
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

        public async Task<int> GetLikesAsync(int albumId)
        {
            this.logger.LogInformation($"Retriving the total number of likes for {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            Album album = await db.Albums
                .Include(a => a.Likes)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                return 0;
            }

            return album.Likes.Count;
        }

        public async Task<int> LikeAsync(int albumId)
        {
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);

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

            return await this.GetLikesAsync(albumId);
        }
    }
}