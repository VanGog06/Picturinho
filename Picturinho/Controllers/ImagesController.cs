using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Picturinho.Models.Image;
using Picturinho.Services.Contracts;
using System.Threading.Tasks;

namespace Picturinho.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImagesController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [HttpGet("{imageId:int}")]
        public async Task<ActionResult<ImageModel>> GetImageAsync([FromRoute] int imageId)
        {
            ImageModel model = await imageService.GetImageAsync(imageId);

            return Ok(model);
        }
    }
}