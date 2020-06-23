using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picturinho.Models.Album;
using Picturinho.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAsync([FromRoute] int userId)
        {
            IEnumerable<AlbumModel> userAlbums = await albumService.GetUserAlbumsAsync(userId);

            return Ok(userAlbums);
        }

        [HttpPost]
        public async Task<ActionResult<AlbumModel>> CreateAsync([FromBody] AlbumModel model)
        {
            try
            {
                AlbumModel createdAlbum = await albumService.CreateAlbumAsync(model);

                return Ok(createdAlbum);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromBody] int albumId)
        {
            await albumService.DeleteAlbumAsync(albumId);

            return Ok();
        }
    }
}