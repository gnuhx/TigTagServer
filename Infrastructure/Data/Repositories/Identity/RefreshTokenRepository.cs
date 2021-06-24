using System.Threading.Tasks;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces.Repositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Identity
{
    public class RefreshTokenRepository : EfRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(NoisContext context) : base(context)
        {
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
           return await DbSet.FirstOrDefaultAsync(p=>p.Token == token);
        }
    }
}
