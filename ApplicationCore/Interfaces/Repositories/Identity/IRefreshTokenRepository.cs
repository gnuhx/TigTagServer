using System.Threading.Tasks;
using ApplicationCore.Entities.Identity;

namespace ApplicationCore.Interfaces.Repositories.Identity
{
    public interface IRefreshTokenRepository : IRepositoryAsync<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
