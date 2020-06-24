using AutoMapper;
using Picturinho.Common.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Picturinho.Models.Album
{
    public class AlbumWithImagesModel : IMapFrom<Entities.Album>, IMapExplicitly
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public IEnumerable<int> ImageIds { get; set; }

        public void RegisterMappings(IProfileExpression profile)
        {
            profile.CreateMap<Entities.Album, AlbumWithImagesModel>()
                .ForMember(ai => ai.ImageIds, opts => opts.MapFrom(a => a.Images.Select(a => a.Id).ToList()));
        }
    }
}