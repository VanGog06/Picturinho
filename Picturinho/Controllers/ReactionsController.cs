using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<int>> GetAlbumLikesAsync([FromRoute] int albumId)
        {
            int likesCount = await reactionsService.GetLikesAsync(albumId);

            return Ok(likesCount);
        }

        [HttpPost("like/{albumId:int}")]
        public async Task<ActionResult<int>> LikeAsync([FromRoute] int albumId)
        {
            int likesCount = await reactionsService.LikeAsync(albumId);

            return Ok(likesCount);
        }

        [HttpPost("love/{albumId:int}")]
        public async Task<ActionResult> LoveAsync([FromRoute] int albumId)
        {
            return Ok();
        }
    }
}