using AutoMapper;

namespace Picturinho.Common.Mapping
{
    public interface IMapExplicitly
    {
        public void RegisterMappings(IProfileExpression profile);
    }
}