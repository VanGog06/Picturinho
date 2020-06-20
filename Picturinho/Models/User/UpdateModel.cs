using Picturinho.Common.Mapping;

namespace Picturinho.Models
{
    public class UpdateModel : IMapTo<Entities.User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}