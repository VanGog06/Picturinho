using Picturinho.Common.Services;
using Picturinho.Models.Image;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IImageService : IScopedService
    {
        Task<ImageModel> GetImageAsync(int imageId);
    }
}