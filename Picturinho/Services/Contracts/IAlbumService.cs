using Picturinho.Common.Services;
using Picturinho.Models.Album;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IAlbumService : IScopedService
    {
        IEnumerable<AlbumWithImagesModel> GetAlbums();

        Task<AlbumWithImagesModel> GetByIdAsync(int albumId);

        Task<IEnumerable<AlbumModel>> GetUserAlbumsAsync(int userId);

        Task<AlbumModel> CreateAlbumAsync(AlbumModel model);

        Task DeleteAlbumAsync(int albumId);
    }
}