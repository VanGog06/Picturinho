using Picturinho.Common.Services;
using System.Threading.Tasks;

namespace Picturinho.Services.Contracts
{
    public interface IReactionsService : IScopedService
    {
        Task<int> GetLikesAsync(int albumId);

        Task<int> LikeAsync(int albumId);
    }
}