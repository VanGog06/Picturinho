using Picturinho.Common.Services;
using Picturinho.Models.Reaction;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IReactionsService : IScopedService
    {
        Task<TotalReactionsModel> GetLikesAsync(int albumId);

        Task LikeAsync(int albumId);

        Task LoveAsync(int albumId);
    }
}