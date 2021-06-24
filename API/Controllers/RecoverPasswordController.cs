using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Account;
using WebAPI.Validators.RecoverPassword;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecoverPasswordController : BaseApiController
    {
        private readonly IService _service;
        private readonly UserManager<User> _userManager;
        public RecoverPasswordController(IService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> SendRecoverLink([FromBody] RecoverEmail model)
        {
            var validator = new RecoverEmailModelValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return ErrorResult(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _service.IdentityService.SendRecoverLinkAsync(model.Email);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.Errors);
            }
            return SuccessResult("Email Sent");
        }

        [HttpPost("verifytoken")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyRecoverLink([FromBody] VerifyLink model)
        {
            var validator = new VerifyLinkModelValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return ErrorResult(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _service.IdentityService.VerifyRecoverLinkAsync(model.Email, model.Token);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.Errors);
            }
            return SuccessResult("Link is valid");
        }

        [HttpPut("reset")]
        //[AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var validator = new ResetPasswordRequestModelValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return ErrorResult(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var result = await _service.IdentityService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.Errors);
            }
            return SuccessEmptyResult("Password is updated.");
        }

    }
}