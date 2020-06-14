﻿using Picturinho.Entities;
using System.Text.Json.Serialization;

namespace Picturinho.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string JwtToken { get; set; }

        // Returned in http only cookie
        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}