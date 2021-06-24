using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class UserToken: IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
