﻿namespace Picturinho.Entities
{
    public class UserLike
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}