using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities.Identity
{
    public class RefreshToken :BaseEntity, IAggregateRoot
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpiredOnUtc { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
