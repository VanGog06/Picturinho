using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Migrations;
using Picturinho.Models.Album;
using Picturinho.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picturinho.Services.Implementations
{
    public class AlbumService : IAlbumService
    {
        private readonly DataContext db;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public AlbumService(DataContext db, ILogger<Albums> logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<AlbumWithImagesModel> GetByIdAsync(int albumId)
        {
            this.logger.LogInformation($"Retrieving {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            Album album = await db.Albums.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                this.logger.LogInformation($"{nameof(Album)} with {nameof(Album.Id)} = {albumId} was not found");
                return null;
            }

            return mapper.Map<AlbumWithImagesModel>(album);
        }

        public async Task<IEnumerable<AlbumModel>> GetUserAlbumsAsync(int userId)
        {
            this.logger.LogInformation($"Getting all {nameof(Album)} records for {nameof(User)} with {nameof(User.Id)} = {userId}");
            IEnumerable<Album> albums = await db.Albums
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return mapper.Map<IEnumerable<AlbumModel>>(albums);
        }

        public async Task<AlbumModel> CreateAlbumAsync(AlbumModel model)
        {
            this.logger.LogInformation($"Creating new {nameof(Album)} for {nameof(User)} with {nameof(User.Id)} = {model.UserId}");
            Album album = mapper.Map<Album>(model);

            await db.Albums.AddAsync(album);
            await db.SaveChangesAsync();

            return mapper.Map<AlbumModel>(album);
        }

        public async Task DeleteAlbumAsync(int albumId)
        {
            this.logger.LogInformation($"Removing {nameof(Album)} with {nameof(Album.Id)} = {albumId}");
            db.Albums.Remove(new Album { Id = albumId });
            await db.SaveChangesAsync();
        }
    }
}