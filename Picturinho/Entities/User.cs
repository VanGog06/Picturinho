﻿using System.Collections.Generic;

namespace Picturinho.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public ICollection<Album> Albums { get; set; } = new List<Album>();

        public ICollection<UserLike> Likes { get; set; } = new List<UserLike>();

        public ICollection<UserLove> Loves { get; set; } = new List<UserLove>();
    }
}