using FluentValidation;

namespace Ecommerce.Functions;

public class SignInRequestValidation : AbstractValidator<CustomerSignInRequest>
{
    public SignInRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is a required field.")
            .EmailAddress().WithMessage("A valid email address is required.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is a required field.");
    }
}
