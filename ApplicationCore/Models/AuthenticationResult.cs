using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public AuthenticationResult()
        {
            UserId = 0;
        }
    }
}
