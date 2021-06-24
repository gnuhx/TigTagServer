using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Account
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string TigtagUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string Tiktok { get; set; }
        public bool IsBanned { get; set; }
        public int AccessCount { get; set; }
        public string UserLoginName { get; set; }
        public string WhatsApp { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public byte[] Avatar { get; set; }
    }

    public class AccountCheckRole
    {
        public int userId { get; set; }
    }
    public class AccountId
    {
        public int userId { get; set; }
    }

    public class IDs
    {
        public List<int> Ids { get; set; }
    }
    public class AccountCompetitorRequest
    {
        public List<int> Ids { get; set; }
    }
    public class AccountActivePhoneNumberRequest
    {
        public string PhoneNumber { get; set; }
    }
    public class AccountActiveNameRequest
    {
        public string Name { get; set; }
    }
    public class AccountActiveEmailRequest
    {
        public string Email { get; set; }
    }

    public class UpdateDetailModel
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdatePasswordModel
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class UpdateEmailModel
    {
        public int UserId { get; set; }
        [EmailAddress]
        public string NewEmail { get; set; }
        public string CurrentToken { get; set; }
    }

    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompetitor { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
    }

    public class RecoverEmail
    {
        [Required]
        public string Email { get; set; }
    }

    public class VerifyLink
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class GetUserDetailsRequest
    {
        public string Role { get; set; }
        public int Id { get; set; }
    }
}
