using AutoMapper;
using Microsoft.Extensions.Logging;
using Picturinho.Entities;
using Picturinho.Helpers;
using Picturinho.Models.Image;
using Picturinho.Services.Contracts;
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
    }
}