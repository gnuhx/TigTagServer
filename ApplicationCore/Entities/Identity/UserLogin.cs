using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class UserLogin: IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
