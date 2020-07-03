using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picturinho.Models.Reaction;
using Picturinho.Services.Contracts;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionsService reactionsService;

        public ReactionsController(IReactionsService reactionsService)
        {
            this.reactionsService = reactionsService;
        }

        [HttpGet("likes/{albumId:int}")]
        public async Task<ActionResult<TotalReactionsModel>> GetAlbumLikesAsync([FromRoute] int albumId)
        {
            TotalReactionsModel reactions = await reactionsService.GetLikesAsync(albumId);

            return Ok(reactions);
        }

        [HttpPost("like/{albumId:int}")]
        public async Task<ActionResult<TotalReactionsModel>> LikeAsync([FromRoute] int albumId)
        {
            await reactionsService.LikeAsync(albumId);
            TotalReactionsModel reactions = await reactionsService.GetLikesAsync(albumId);

            return Ok(reactions);
        }

        [HttpPost("love/{albumId:int}")]
        public async Task<ActionResult<TotalReactionsModel>> LoveAsync([FromRoute] int albumId)
        {
            await reactionsService.LoveAsync(albumId);
            TotalReactionsModel reactions = await reactionsService.GetLikesAsync(albumId);

            return Ok(reactions);
        }
    }
}