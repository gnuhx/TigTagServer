using FluentValidation;
using WebAPI.Models.Account;

namespace WebAPI.Validators.RecoverPassword
{
    public class RecoverEmailModelValidator:AbstractValidator<RecoverEmail>
    {
        public RecoverEmailModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
