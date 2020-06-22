using Picturinho.Common.Mapping;

namespace Picturinho.Models.Album
{
    public class AlbumModel : IMapFrom<Entities.Album>, IMapTo<Entities.Album>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
    }
}