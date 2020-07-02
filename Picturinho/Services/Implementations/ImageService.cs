using AutoMapper;
using Microsoft.Extensions.Logging;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Models.Image;
using Picturinho.Services.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Picturinho.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly DataContext db;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ImageService(DataContext db, IMapper mapper, ILogger<ImageService> logger)
        {
            this.db = db;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ImageModel> GetImageAsync(int imageId)
        {
            this.logger.LogInformation($"Retrieving {nameof(Image)} with {nameof(Image.Id)} = {imageId}");
            Image image = await this.db.Images.FindAsync(imageId);

            return mapper.Map<ImageModel>(image);
        }

        public async Task<ImageModel> AddImageToAlbumAsync(CreateImageModel model)
        {
            int albumId = int.Parse(model.AlbumId);
            Album album = await db.Albums.FindAsync(albumId);

            if (album == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                await model.Image.CopyToAsync(ms);
                Image image = new Image
                {
                    AlbumId = album.Id,
                    Data = ms.ToArray(),
                    Name = model.Image.FileName
                };

                await db.Images.AddAsync(image);
                await db.SaveChangesAsync();

                return mapper.Map<ImageModel>(image);
            }
        }

        public async Task<int> DeleteImageAsync(int imageId)
        {
            this.logger.LogInformation($"Removing {nameof(Image)} with {nameof(Image.Id)} = {imageId}");
            db.Images.Remove(new Image { Id = imageId });
            await db.SaveChangesAsync();

            return imageId;
        }
    }
}