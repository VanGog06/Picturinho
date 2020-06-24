using Picturinho.Common.Mapping;

namespace Picturinho.Models.Image
{
    public class ImageModel : IMapFrom<Entities.Image>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}