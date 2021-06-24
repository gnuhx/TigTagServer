using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class RoleClaim: IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
