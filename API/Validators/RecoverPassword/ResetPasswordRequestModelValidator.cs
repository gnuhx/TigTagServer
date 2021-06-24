using FluentValidation;
using WebAPI.Models.Account;

namespace WebAPI.Validators.RecoverPassword
{
    public class ResetPasswordRequestModelValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
