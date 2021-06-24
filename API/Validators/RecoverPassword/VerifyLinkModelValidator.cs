using FluentValidation;
using WebAPI.Models.Account;

namespace WebAPI.Validators.RecoverPassword
{
    public class VerifyLinkModelValidator:AbstractValidator<VerifyLink>
    {
        public VerifyLinkModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
