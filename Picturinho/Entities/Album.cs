using System.Collections.Generic;

namespace Picturinho.Entities
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

        public ICollection<UserLike> Likes { get; set; } = new List<UserLike>();

        public ICollection<UserLove> Loves { get; set; } = new List<UserLove>();
    }
}