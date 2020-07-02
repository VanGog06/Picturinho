using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picturinho.Models.Album;
using Picturinho.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumWithImagesModel>> GetAlbums()
        {
            IEnumerable<AlbumWithImagesModel> albums = albumService.GetAlbums();

            return Ok(albums);
        }

        [HttpGet("{albumId:int}")]
        public async Task<ActionResult<AlbumWithImagesModel>> GetByIdAsync([FromRoute] int albumId)
        {
            AlbumWithImagesModel album = await albumService.GetByIdAsync(albumId);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetUserAlbumsAsync([FromRoute] int userId)
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

        [HttpDelete("{albumId:int}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int albumId)
        {
            await albumService.DeleteAlbumAsync(albumId);

            return Ok();
        }
    }
}