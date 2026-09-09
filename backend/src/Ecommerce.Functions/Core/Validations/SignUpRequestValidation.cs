using FluentValidation;

namespace Ecommerce.Functions;

public class SignUpRequestValidation : AbstractValidator<CustomerSignUpRequest>
{
    public SignUpRequestValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is a required field.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is a required field.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is a required field.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is a required field.")
            .Matches(@"^\d{10}$").WithMessage("PhoneNumber must be exactly 10 digits.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is a required field.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}