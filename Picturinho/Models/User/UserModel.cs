using Picturinho.Common.Mapping;

namespace Picturinho.Models.User
{
    public class UserModel : IMapFrom<Entities.User>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }
    }
}