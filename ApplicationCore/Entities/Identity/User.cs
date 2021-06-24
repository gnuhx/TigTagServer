using System;
using System.Collections.Generic;
using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Entities.RelationAggregate;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public virtual List<Notification> Notifications { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string TigtagUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string Tiktok { get; set; }
        public bool IsBanned { get; set; }
        public string PasswordHashCode { get; set; }
        public int AccessCount { get; set; }
        public string UserLoginName { get; set; }
        public string WhatsApp { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public virtual List<Relation> Relations { get; set; }
        public byte[] Avatar { get; set; }
    }
}
