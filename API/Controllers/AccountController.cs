using ApplicationCore.Entities.Identity;
using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models.Account;
using WebAPI.Validators.RecoverPassword;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly IService _service;
        public AccountController(IService service)
        {
            _service = service;
        }

        #region ACTIVE
        [HttpPut("update/active")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateActiveUser([FromBody] IDs ids)
        {
            try
            {
                var users = new List<User>();
                //get all user from ids list 
                foreach (var id in ids.Ids)
                {
                    var user = await _service.IdentityService.GetByIdAsync(id);
                    user.ActiveDate = DateTime.Now;
                    user.IsActive = true;
                    users.Add(user);
                    await _service.IdentityService.UpdateUserAsync(user);
                }

                return SuccessResult("Users Activate successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }

        [HttpPut("update/deactive")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDeactiveUser([FromBody] IDs ids)
        {
            try
            {
                var users = new List<User>();
                //get all user from ids list 
                foreach (var id in ids.Ids)
                {
                    var user = await _service.IdentityService.GetByIdAsync(id);
                    user.IsActive = false;
                    users.Add(user);
                }


                //change data of user
                foreach (var user in users)
                {
                    try
                    {
                        await _service.IdentityService.UpdateUserAsync(user);

                        var notiOnServer = new Notification()
                        {
                            Content = "Tài khoản đã hết hạn.",
                            IsChecked = false,
                            Type = 2,
                            //testId = model.TestId,
                            //TestId = test.Id,
                            UserId = user.Id
                        };
                        await _service.NotificationService.AddAsync(notiOnServer);
                    }
                    catch (Exception)
                    {
                        //TODO
                    }

                }

                return SuccessResult("Users Deactive Activate successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }
        #endregion


        #region admin service
        [HttpPost("admin/login")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest model)
        {
            try
            {
                var result = await _service.IdentityService.LoginAsync(model.User, model.Password,"admin");
                if (!result.IsSuccess)
                {
                    return FailResult(result.ErrorMessages, "failed at verify");
                }
                var user = await _service.IdentityService.GetByIdAsync(result.UserId);

                //check role
                string role = "";
                try
                {
                    role = await _service.IdentityService.GetUserRoleByEmail(user.Email);
                }
                catch (Exception e)
                {
                    return FailResult("This account is not admin or super-admin account", e.ToString());
                }

                if (!role.ToLower().Contains("admin"))
                {
                    return FailResult("This is " + role + " account not Admin Account", "superadmin login fail");
                }

                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = user.Id,
                    Role = role.ToLower()
                });
            }
            catch (Exception e)
            {
                //var log = new Log()
                //{
                //    Status = false,
                //    Action = "admin login failed at " + DateTime.Now.ToString()
                //};
                //await _service.LogService.AddAsync(log);
                return FailResult(e.ToString(), "failed at try catch");
            }
        }

        [HttpPost("superadmin/register")]
        [AllowAnonymous]//
        public async Task<IActionResult> SuperAdminRegister([FromBody] AccountModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return FailResult(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList(), "failed to register");
                }
                var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                    model.Name,
                    model.Age,
                    model.Address,
                    model.PhoneNumber,
                    "superadmin",
                     model.IsActive,
                    model.TigtagUrl,
                    model.FacebookUrl,
                    model.Tiktok,
                    model.IsBanned,
                    MD5Hash(model.Password),
                    model.AccessCount,
                    model.UserLoginName,
                    model.WhatsApp,
                    model.Instagram,
                    model.Youtube,
                    model.Avatar);
                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);
                string role = await _service.IdentityService.GetUserRoleById(userId);

                if (!result.IsSuccess)
                {
                    var log = new Log()
                    {
                        Status = false,
                        Action = "superadmin register failed at " + DateTime.Now.ToString() + " " + result.ErrorMessages.ToString()
                    };
                    await _service.LogService.AddAsync(log);
                    return FailResult(result.ErrorMessages, "failed to verify login");
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId,
                    Role = role.ToLower()
                });
            }
            catch (Exception e)
            {
                var log = new Log()
                {
                    Status = false,
                    Action = "superadmin register failed at " + DateTime.Now.ToString()
                };
                await _service.LogService.AddAsync(log);
                return FailResult(e.ToString(), "failed at try catch");
            }
        }

        /// <summary>
        /// register api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("admin/create/{role}")]
        [AllowAnonymous]//
        public async Task<IActionResult> Register([FromBody] AccountModel model, [FromRoute] string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return FailResult(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList(), "failed at verify");
                }
                if (role.ToLower() != "superadmin")
                {
                    return FailResult("current role is : " + role.ToLower() + " not superadmin", "failed at role verify");
                }

                var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                    model.Name,
                    model.Age,
                    model.Address,
                    model.PhoneNumber,
                    "admin",
                    model.IsActive,
                    model.TigtagUrl,
                    model.FacebookUrl,
                    model.Tiktok,
                    model.IsBanned,
                    MD5Hash(model.Password),
                    model.AccessCount,
                    model.UserLoginName,
                    model.WhatsApp,
                    model.Instagram,
                    model.Youtube,
                    model.Avatar);

                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);
                if (!result.IsSuccess)
                {
                    return FailResult(result.ErrorMessages, "failed at verify");
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    //Token = result.Token,
                    //RefreshToken = result.RefreshToken,
                    UserId = userId
                }, "create new admin successfully with email = " + model.Email + " and password = " + model.Password);
            }
            catch (Exception e)
            {
                return FailResult(e.ToString(), "failed at try catch");
            }
        }
        #endregion

        /// <summary>
        /// register api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister([FromBody] AccountModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return FailResult(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList(), "failed at verify");
                }
                var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                    model.Name,
                    model.Age,
                    model.Address,
                    model.PhoneNumber,
                    "admin",
                    model.IsActive,
                    model.TigtagUrl,
                    model.FacebookUrl,
                    model.Tiktok,
                    model.IsBanned,
                    MD5Hash(model.Password),
                    model.AccessCount,
                    model.UserLoginName,
                    model.WhatsApp,
                    model.Instagram,
                    model.Youtube,
                    model.Avatar);

                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);
                string role = await _service.IdentityService.GetUserRoleById(userId);


                //create expire notification -> "vui long kich hoat tai khoan de su dung dich vu"
                var expireRequestNotification = new Notification()
                {
                    UserId = userId,
                    Title = "Vui Lòng Kích Hoạt Tài Khoản Để Sử Dụng Dịch Vụ!",
                    Content = "Hoặc đến gặp Tâm để được hỗ trợ trực tiếp, xin cám ơn.",
                    Type = 3
                };
                await _service.NotificationService.AddAsync(expireRequestNotification);


                if (!result.IsSuccess)
                {
                    return FailResult(result.ErrorMessages, "failed at verify");
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId,
                    Role = role.ToLower()
                });
            }
            catch (Exception e)
            {
                return FailResult("Error Information.", "failed at try catch");
            }
        }
        /// <summary>
        /// login api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var result = await _service.IdentityService.LoginAsync(model.User, model.Password,"user");

                if (!result.IsSuccess)
                {
                    return FailResult(result.ErrorMessages, "failed at verify");
                }

                var user = await _service.IdentityService.GetByIdAsync(result.UserId);

                string role = "";
                try
                {

                    user = await _service.IdentityService.GetUserByEmail(model.User);
                    
                    #region Check Role 
                    try
                    {
                        role = await _service.IdentityService.GetUserRoleById(user.Id);
                    }
                    catch (Exception e)
                    {
                        return FailResult("This account is not user account", e.ToString());
                    }
                    if (role.ToLower() != "user")
                    {
                        return FailResult("This is not user account", "failed at verify role");
                    }
                    #endregion
                    if (user != null)
                    {
                    }
                }
                catch (Exception e)
                {
                    //TODO
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = user.Id,
                    Role = role.ToLower(),
                    IsActive = user.IsActive
                });
            }
            catch (Exception e)
            {
                return FailResult(e.ToString(), "failed at try catch");
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            try
            {
                var result = await _service.IdentityService.RefreshTokenAsync(model.Token, model.RefreshToken);
                if (!result.IsSuccess)
                {
                    return FailResult(result.ErrorMessages, "failed at verify");
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                });
            }
            catch (Exception e)
            {
                return FailResult(e.ToString(), "failed at try catch");
            }
        }

        //update user detail
        [AllowAnonymous]//
        [HttpPost("update/detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserDetail([FromBody] AccountModel model)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(model.Id);
                //change data of user
                if (model.Name != null)
                {
                    userOnServer.Name = model.Name;
                }
                if (model.Age.ToString() != null)
                {
                    userOnServer.Age = model.Age;
                }

                if (model.Address != null)
                {
                    userOnServer.Address = model.Address;
                }

                if (model.PhoneNumber != null)
                {
                    userOnServer.PhoneNumber = model.PhoneNumber;
                }
                ////////////////////////--------------------------------------------////////////////////
                //IsActive = model.IsActive,
                //IsBanned = model.IsBanned,
                //WhatsApp = model.WhatsApp,
                if (model.WhatsApp != null && model.WhatsApp!= "")
                {
                    userOnServer.WhatsApp = model.WhatsApp;
                }
                //FacebookUrl = model.FacebookUrl,
                if (model.FacebookUrl != null && model.FacebookUrl != "")
                {
                    userOnServer.FacebookUrl = model.FacebookUrl;
                }
                //Instagram = model.Instagram,
                if (model.Instagram != null && model.Instagram != "")
                {
                    userOnServer.Instagram = model.Instagram;
                }
                //TigtagUrl = model.TigtagUrl,
                if (model.TigtagUrl != null && model.TigtagUrl != "")
                {
                    userOnServer.TigtagUrl = model.TigtagUrl;
                }
                //Tiktok = model.Tiktok,
                if (model.Tiktok != null && model.Tiktok != "")
                {
                    userOnServer.Tiktok = model.Tiktok;
                }
                //UserLoginName = model.UserLoginName,
                if (model.UserLoginName != null && model.UserLoginName != "")
                {
                    userOnServer.UserLoginName = model.UserLoginName;
                }
                //Youtube = model.Youtube
                if (model.Youtube != null && model.Youtube != "")
                {
                    userOnServer.Youtube = model.Youtube;
                }

                if (model.Avatar != null)
                {
                    userOnServer.Avatar = model.Avatar;
                }
                var queryRes = await _service.IdentityService.UpdateUserAsync(userOnServer);

                //map entity to response model
                var resAccountModel = UserToAccountModel(queryRes,"unknown");
                return SuccessResult(resAccountModel, "Update User Detail successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at trycatch");
            }
        }

        //update user password
        [AllowAnonymous]//
        [HttpPut("update/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordModel model)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(model.UserId);

                //check password
                string modelPasswordHash = MD5Hash(model.CurrentPassword);

                if (userOnServer.PasswordHashCode != modelPasswordHash)
                {
                    return FailResult("Not correct current password.");
                }

                string modelNewPasswordHash = MD5Hash(model.NewPassword);
                if (userOnServer.PasswordHashCode == modelNewPasswordHash)
                {
                    return FailResult("New password couldnot be the same with current password.");
                }

                //update user password
                var queryRes = await _service.IdentityService.UpdatePasswordAsync(userOnServer, model.CurrentPassword, model.NewPassword);
                var resAccountModel = UserToAccountModel(queryRes, "unknown");


                return SuccessResult(resAccountModel, "Update User Password successfully.");
            }
            catch (Exception e)
            {
                return FailResult("update passord error", "failed at try catch");
            }
        }


        ////update user email
        //[AllowAnonymous]//
        //[HttpPut("update/email")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmailModel model)
        //{
        //    try
        //    {
        //        var userOnServer = await _service.IdentityService.GetByIdAsync(model.UserId);
        //        //update user password
        //        var queryRes = await _service.IdentityService.UpdateEmailAsync(userOnServer, model.NewEmail, model.CurrentToken);
        //        var resAccountModel = UserToAccountModel(queryRes,"unknown");

        //        return SuccessResult(resAccountModel, "Update User Email successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message, "failed at try catch");
        //    }
        //}

        //delete user
        [AllowAnonymous]//
        [HttpDelete("delete/user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromBody] AccountId UserId)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(UserId.userId);
                //update user password
                var queryRes = await _service.IdentityService.DeleteUserAsync(userOnServer);
                if (queryRes)
                {
                    return SuccessResult(queryRes, "Delete User successfully.");
                }
                else
                {
                    return FailResult("Failed to delete User has Id =" + UserId.userId.ToString() + " .", "failed at Id");
                }
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }

        //get by tigtag url
        [AllowAnonymous]//
        [HttpGet("{url}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByUrl([FromRoute] int id = 0, [FromRoute] string url ="")
        {
            try
            {

                string role = "";
                try
                {
                    var userOnServer = await _service.IdentityService.GetByIdAsync(id);
                    if (userOnServer != null)
                    {
                        role = await _service.IdentityService.GetUserRoleById(id);
                    }
                }
                catch (Exception)
                {

                }

                //-> get user by url
                try
                {
                    var returnUser = await _service.IdentityService.GetUserByUrl(url);
                    if (returnUser == null)
                    {
                        return FailResult("user = null", "user not found.");
                    }
                    //access count +1
                    if (role == "user" )
                    {
                        returnUser.AccessCount = returnUser.AccessCount + 1;
                        await _service.IdentityService.UpdateUserAsync(returnUser);
                    }
                    var returnModel = UserToAccountModel(returnUser);

                    return SuccessResult(returnModel, "get user success!");
                }
                catch (Exception e)
                {
                    return FailResult(e.ToString(),"user not found.");
                }
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }





        ////get all user paging
        //[AllowAnonymous]//
        //[HttpPost("get/all/{pageIndex}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetAllPaging([FromRoute] int pageIndex)
        //{
        //    try
        //    {
        //        var queryRes = await _service.IdentityService.GetAllPaging(50, pageIndex);
        //        var newUsers = new List<AccountModel>();
        //        foreach (var res in queryRes)
        //        {
        //            var role = await _service.IdentityService.GetUserRoleById(res.Id);
        //            var newUser = UserToAccountModel(res,role);
        //            newUsers.Add(newUser);
        //        }
        //        return SuccessResult(newUsers, "Get all Users with paging successfully.");
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message.ToString());
        //    }
        //}


        [HttpPost("get/all/{keySearch}/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSearchPaging([FromRoute] int pageIndex, [FromRoute] string keySearch, [FromBody] AccountCheckRole accountCheckRole)
        {
            try
            {
                string role = "";
                try
                {
                    role = await _service.IdentityService.GetUserRoleById(accountCheckRole.userId);
                }
                catch (Exception e)
                {
                    //to do
                }
                if (role.ToLower() != "superadmin" || role == "" || role == null)
                {
                    return FailResult("role is not superadmin");
                }
                var queryRes = await _service.IdentityService.GetAllSearchPaging(50, pageIndex, keySearch);
                var newUsers = new List<AccountModel>();
                foreach (var res in queryRes)
                {
                    var roleRes = await _service.IdentityService.GetUserRoleById(res.Id);
                    var newUser = UserToAccountModel(res, role);
                    newUsers.Add(newUser);
                }
                return SuccessResult(newUsers, "Get all Users with paging successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message.ToString());
            }
        }



        [AllowAnonymous]//
        [HttpPost("get/all/admin/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdminPaging([FromRoute] int pageIndex, [FromBody] AccountCheckRole accountCheckRole)
        {
            try
            {

                string role = "";
                try
                {
                    role = await _service.IdentityService.GetUserRoleById(accountCheckRole.userId);
                }
                catch (Exception e)
                {
                    //to do
                }
                if (role.ToLower() != "superadmin" || role == "" || role == null)
                {
                    return FailResult("role is not superadmin");
                }
                var queryRes = await _service.IdentityService.GetAllAdminPaging(50, pageIndex);
                var newUsers = new List<AccountModel>();
                foreach (var res in queryRes)
                {
                    var roleRes = await _service.IdentityService.GetUserRoleById(res.Id);
                    var newUser = UserToAccountModel(res, role);
                    newUsers.Add(newUser);
                }
                return SuccessResult(newUsers, "Get all admins - superadmin with paging successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message.ToString());
            }
        }

        ////get user role
        //[AllowAnonymous]//
        //[HttpGet("role/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetUserRole([FromRoute] int id)
        //{
        //    try
        //    {
        //        var userOnServer = await _service.IdentityService.GetByIdAsync(id);
        //        if (userOnServer != null)
        //        {
        //            string result = await _service.IdentityService.GetUserRoleById(id);
        //            return SuccessResult(result);
        //        }
        //        else
        //        {
        //            return FailResult("Failed to get User has Id =" + id.ToString() + " .", "failed at id");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return FailResult(e.Message, "failed at try catch");
        //    }
        //}

        [AllowAnonymous]//
        [HttpGet("get/details/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDetails([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                var userOnServer = await _service.IdentityService.GetByIdAsync(id);
                if (userOnServer != null)
                {
                    string role = await _service.IdentityService.GetUserRoleById(userOnServer.Id);
                    //map to detail
                    var result = UserToAccountModel(userOnServer, role);
                    return SuccessResult(result, "Get user detail successfully !");
                }
                else
                {
                    return FailResult("Failed to get User has Id =" + id.ToString() + " .", "failed at id");
                }
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }


        [AllowAnonymous]
        [HttpPut("update/role/{userId}/{role}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNullRole([FromRoute] int userId, [FromRoute] string role)
        {
            try
            {
                var user = await _service.IdentityService.GetByIdAsync(userId);

                if (user == null)
                {
                    return FailResult("user not found.", "Failed to get user with id = " + userId.ToString());
                }

                string oldRole = "";
                try
                {
                    oldRole = await _service.IdentityService.GetUserRoleById(userId);
                    if (oldRole.ToLower() == "superadmin")
                    {
                        return FailResult("Cannot update role of superadmin.");
                    }
                }
                catch (Exception e)
                {
                    //do some thing
                }

                await _service.IdentityService.UpdateUserRole(user, oldRole.ToLower(), role.ToLower());
                var resUser = UserToAccountModel(user);
                resUser.Role = role;
                return SuccessResult(resUser, "update null role success");
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }


        [AllowAnonymous]
        [HttpPut("force/update/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AdminForceUpdateUserPassword([FromBody] UpdatePasswordModel model)
        {
            try
            {

                var userOnServer = await _service.IdentityService.GetByIdAsync(model.UserId);
                string modelPasswordHash = MD5Hash(model.CurrentPassword);

                //update user password
                var queryRes = await _service.IdentityService.UpdatePasswordAsync(userOnServer, model.CurrentPassword, model.NewPassword);
                var resAccountModel = UserToAccountModel(queryRes);
                return SuccessResult(resAccountModel, "Update User Password successfully.");
            }
            catch (Exception e)
            {
                return FailResult(e.Message, "failed at try catch");
            }
        }

        #region Mapping
        private User AccountModelToUser(AccountModel model)
        {
            var user = new User()
            {
                Id = model.Id,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                AccessCount = model.AccessCount,
                IsActive = model.IsActive,
                WhatsApp = model.WhatsApp,
                FacebookUrl = model.FacebookUrl,
                Instagram = model.Instagram,
                IsBanned = model.IsBanned,
                TigtagUrl = model.TigtagUrl,
                Tiktok = model.Tiktok,
                UserLoginName = model.UserLoginName,
                Youtube = model.Youtube,
                Avatar = model.Avatar
            };
            return user;
        }

        private AccountModel UserToAccountModel(User user, string role ="user")
        {
            var userRes = new AccountModel()
            {
                Address = user.Address,
                Age = user.Age,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                AccessCount = user.AccessCount,
                IsActive = user.IsActive,
                WhatsApp = user.WhatsApp,
                FacebookUrl = user.FacebookUrl,
                Instagram = user.Instagram,
                IsBanned = user.IsBanned,
                TigtagUrl = user.TigtagUrl,
                Tiktok = user.Tiktok,
                UserLoginName = user.UserLoginName,
                Youtube = user.Youtube,
                Role = role,
                Avatar = user.Avatar
            };
            return userRes;
        }

        public static string MD5Hash(string input)
        {
            string hash = input + "md5MD5";
            return hash.ToString();
        }
        #endregion
    }
}

/*
                     var log = new Log()
                    {
                        Status = ,
                        Action = 
                    };
                    await _service.LogService.AddAsync(log);
 */

