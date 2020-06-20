using Picturinho.Common.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Picturinho.Models
{
    public class RegisterModel : IMapTo<Entities.User>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}