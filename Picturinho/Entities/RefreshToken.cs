using Microsoft.EntityFrameworkCore;
using System;

namespace Picturinho.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;

        public DateTimeOffset Created { get; set; }

        public string CreatedByIp { get; set; }

        public DateTimeOffset? Revoked { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;
    }
}