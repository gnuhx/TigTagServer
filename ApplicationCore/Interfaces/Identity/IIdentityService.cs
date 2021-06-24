using ApplicationCore.Entities.Identity;
using ApplicationCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string name,
            int age, string address, string phoneNumber, string role, bool isActive,
                    string tigtagUrl,
                    string facebookUrl,
                    string tiktok,
                    bool isBanned,
                    string passwordHashCode,
                    int accessCount,
                    string userLoginName,
                    string whatsApp,
                    string instagram,
                    string youtube,
                    byte[] avatar);
        Task<AuthenticationResult> LoginAsync(string email, string password, string role = "user");
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<LogicResult<object>> SendRecoverLinkAsync(string email);
        Task<LogicResult<object>> VerifyRecoverLinkAsync(string email, string token);
        Task<LogicResult<object>> ResetPasswordAsync(string email, string token, string newPassword);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<List<User>> GetAllPaging(int pageSize, int pageIndex);
        Task<User> UpdatePasswordAsync(User user, string currentPassword, string newPassword);
        Task<User> UpdateEmailAsync(User user, string newEmail, string token);
        Task<bool> DeleteUserAsync(User user);
        Task<int> GetUserIdByEmail(string email);
        Task<string> GetUserRoleByEmail(string email);
        Task<string> GetUserRoleById(int id);
        Task<List<int>> GetAllStudentIds();
        Task<User> UpdateUserRole(User user, string oldRole, string newRole);

        Task<User> AddUserRole(User user, string role);
        Task<List<int>> GetAllUsersIds();
        Task<List<int>> GetAllUsersNullRoleIds();
        Task<List<User>> GetAllAdminPaging(int pageSize, int pageIndex);
        Task<bool> CheckIsActive(int id);
        Task<List<User>> GetAllSearchPaging(int pageSize, int pageIndex, string keySearch);
        Task<User> GetSearchName(string keySearch);
        Task<User> GetSearchEmail(string keySearch);
        Task<User> GetSearchPhoneNunmber(string keySearch);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUrl(string url);
    }
}
