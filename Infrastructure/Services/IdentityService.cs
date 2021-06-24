using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Repositories.Identity;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Email;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailService _emailService;
        private readonly Settings _settings;
        private readonly RoleManager<Role> _roleManager;
        public IdentityService(UserManager<User> userManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository,
            IEmailService emailService, Settings settings,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _emailService = emailService;
            _settings = settings;
            _roleManager = roleManager;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string name,
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
                    byte[] avatar)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User is already exists" }
                };
            }
            var hashCodePassword = MD5Hash(password);
            var newUser = new User
            {
                Email = email,
                UserName = email,
                Name = name,
                Address = address,
                Age = age,
                PhoneNumber = phoneNumber,
                IsActive = false,
                TigtagUrl = tigtagUrl,
                FacebookUrl = facebookUrl,
                Tiktok = tiktok,
                IsBanned = isBanned,
                AccessCount = 0,
                PasswordHashCode = hashCodePassword
            };
            if (avatar != null)
            {
                newUser.Avatar = avatar;
            }

            var result = await _userManager.CreateAsync(newUser, password);

            //get user Id
            var userId = newUser.Id;
            //add role : 1-> student, 2->admin
            //check is role existing
            var isExistingRole = await _roleManager.FindByNameAsync(role);
            if (isExistingRole == null)
            {
                var newRole = new Role()
                {
                    Name = role,
                    NormalizedName = role
                };
                //create new role
                await _roleManager.CreateAsync(newRole);
            }

            //add to this role
            await _userManager.AddToRoleAsync(newUser, role);


            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = result.Errors.Select(p => p.Description).ToList()
                };
            }

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password,string role = "user")
        {
            var user = new User();
            //check if email?
            if (email.Contains("@gmai") == true)
            {
                user = await _userManager.FindByEmailAsync(email);
            }
            else
            {
                var users = await _userManager.GetUsersInRoleAsync(role);
                if (email.ToCharArray().Any(char.IsDigit)) //check if name?
                {
                    user = users.Where(x => x.UserLoginName == email).FirstOrDefault();
                }
                else  //check if phone number?
                {
                    user = users.Where(x => x.PhoneNumber == email).FirstOrDefault();
                }
            }

            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User does not exist" }
                };
            }
            var hasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!hasValidPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User or password combination is wrong" }
                };
            }
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "Invalid token" }
                };
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This token hasn't expired yet" }
                };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storeRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storeRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token does not exist" }
                };
            }

            if (DateTime.UtcNow > storeRefreshToken.ExpiredOnUtc)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token has expired" }
                };
            }

            if (storeRefreshToken.Invalidated)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token has been invalidated" }
                };
            }

            if (storeRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token does not match JWT" }
                };
            }

            storeRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateAsync(storeRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync((validatedToken.Claims.Single(x => x.Type == "id").Value));
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<LogicResult<object>> SendRecoverLinkAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var sendTo = new List<string> { existingUser.Email };
            var subject = "Password Recovery Requested - TIGTAG.VN";
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var body =
            $@"
            <div>
                Hello,
                <br>
                <br>
                To reset your account password, please visit the following link within 24 hours:
                <br>
                <br>
                <a href='{_settings.Domain}/auth/recover-password?email={existingUser.Email}&token={token}'
                   target='_blank'>{_settings.Domain}/auth/recover-password?email={existingUser.Email}&token={token}</a>
                <br>
                <br>
                Regards,
                <br>
                <br>
                TigTag team.
            </div>
            ";
            await _emailService.SendEmailAsync(sendTo, subject, body);
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<LogicResult<object>> VerifyRecoverLinkAsync(string email, string token)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var verifyingData = await _userManager.VerifyUserTokenAsync(existingUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
            if (!verifyingData)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "Token is invalid" }
                };
            }
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<LogicResult<object>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var resetRequest = await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
            if (!resetRequest.Succeeded)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "Token is not correct" }
                };
            }
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiredOnUtc = DateTime.UtcNow.AddDays(30),
                Token = Guid.NewGuid().ToString()
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                IsSuccess = true,
                UserId = user.Id
            };
        }

        /// <summary>
        /// get by Id async
        /// </summary>
        /// <param id="user Id"></param>
        /// <returns></returns>
        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        }

        /// <summary>
        /// Update async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
            return user;
        }

        /// <summary>
        /// get all paging
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllPaging(int pageSize, int pageIndex)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            //query 
            var result = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x=>x.ConcurrencyStamp).ToList();
            return result;
        }

        public async Task<List<User>> GetAllSearchPaging(int pageSize, int pageIndex,string keySearch)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            var returnResult = users.Where(x => x.Name.Contains(keySearch) || x.PhoneNumber.Contains(keySearch)||
            x.Email.Contains(keySearch));
            //query 
            var result = returnResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.ConcurrencyStamp).ToList();
            return result;
        }

        public async Task<User> GetSearchName(string keySearch)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            var returnResult = users.Where(x => x.Name == keySearch).FirstOrDefault();
            //query 
            var result = returnResult;
            return result;
        }
        public async Task<User> GetSearchEmail(string keySearch)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            var returnResult = users.Where(x => x.Email == keySearch).FirstOrDefault();
            //query 
            var result = returnResult;
            return result;
        }
        public async Task<User> GetSearchPhoneNunmber(string keySearch)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            var returnResult = users.Where(x => x.PhoneNumber == keySearch).FirstOrDefault();
            //query 
            var result = returnResult;
            return result;
        }


        public async Task<List<User>> GetAllAdminPaging(int pageSize, int pageIndex)
        {
            //get all users
            var admins = await _userManager.GetUsersInRoleAsync("admin");
            var superAdmins = await _userManager.GetUsersInRoleAsync("superadmin");
            //jjoin 2 group
            var users = new List<User>();
            
            foreach (var admin in admins)
            {
                users.Add(admin);
            }

            foreach (var superAdmin in superAdmins)
            {
                users.Add(superAdmin);
            }
            //query 
            var result = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.ConcurrencyStamp).ToList();
            return result;
        }
        public async Task<List<int>> GetAllUsersIds()
        {
            var users = await _userManager.GetUsersInRoleAsync("user");
            //var return list Ids
            return users.Select(x => x.Id).ToList();
        }

        public async Task<List<int>> GetAllAccountsIds()
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("user");
            var usersAdmins = await _userManager.GetUsersInRoleAsync("admin");
            var usersSuperAdmins = await _userManager.GetUsersInRoleAsync("superadmin");
            //var return list Ids
            var ids = new List<int>();
            //query 
            foreach (var user in users)
            {
                ids.Add(user.Id);
            }
            foreach (var user in usersAdmins)
            {
                ids.Add(user.Id);
            }
            foreach (var user in usersSuperAdmins)
            {
                ids.Add(user.Id);
            }
            return ids;
        }

        public async Task<List<int>> GetAllUsersNullRoleIds()
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("");
            //var return list Ids
            var ids = new List<int>();
            //query 
            foreach (var user in users)
            {
                ids.Add(user.Id);
            }
            return ids;
        }



        /// <summary>
        /// update new password with hash code
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <param name="isValidated"></param>
        /// <returns></returns>
        public async Task<User> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        {
            //update user password hash
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            user.PasswordHashCode = MD5Hash(newPassword);
            await _userManager.UpdateAsync(user);
            return user;
        }

        /// <summary>
        /// change email async
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newEmail"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<User> UpdateEmailAsync(User user, string newEmail, string token)
        {
            //update user password hash
            await _userManager.ChangeEmailAsync(user, newEmail, token);
            return user;
        }

        /// <summary>
        /// Delete user async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                //update user password hash
                await _userManager.DeleteAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetUserIdByEmail(string email)
        {
            try
            {
                //get user -> get id
                var user = await _userManager.FindByEmailAsync(email);
                return user.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                //get user -> get id
                var user = await _userManager.FindByEmailAsync(email);
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> GetUserRoleByEmail(string email)
        {
            //get user -> get id
            var user = await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            return role.FirstOrDefault().ToString();
        }

        public async Task<string> GetUserRoleById(int id)
        {
            //get user -> get id
            var user = await _userManager.FindByIdAsync(id.ToString());
            var role = await _userManager.GetRolesAsync(user);
            string roleRes = role.FirstOrDefault();
            if(roleRes == null)
            {
                roleRes = "";
            }
            return roleRes;
        }

        public async Task<User> AddUserRole(User user,string role)
        {
            await _userManager.AddToRoleAsync(user,role);
            return user;
        }
        public async Task<User> UpdateUserRole(User user,string oldRole, string newRole)
        {
            if (oldRole != null && oldRole != "")
            {
                await _userManager.RemoveFromRoleAsync(user, oldRole);
            }
            await _userManager.AddToRoleAsync(user, newRole);
            return user;
        }
        public static string MD5Hash(string input)
        {
            string hash = input + "md5MD5";
            return hash.ToString();
        }

        public async Task<bool> CheckIsActive(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()); 
            if (user.IsActive== true)
            {
                return true;
            }
            return false;
        }

        public Task<List<int>> GetAllStudentIds()
        {
            throw new NotImplementedException();
        }


        public async Task<User> GetUserByUrl(string url)
        {
            var users = await _userManager.GetUsersInRoleAsync("user");
            return users.Where(x => x.TigtagUrl == url).FirstOrDefault();
        }
    }
}
