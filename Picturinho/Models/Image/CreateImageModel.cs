using Microsoft.AspNetCore.Http;

namespace Picturinho.Models.Image
{
    public class CreateImageModel
    {
        public string AlbumId { get; set; }

        public IFormFile Image { get; set; }
    }
}