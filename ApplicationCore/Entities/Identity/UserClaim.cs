using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class UserClaim: IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}
