using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Functions;

public class LoginRequestValidation : AbstractValidator<Customer>
{
    public LoginRequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is a required field.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is a required field.")
            .EmailAddress().WithMessage("A valida email address is required.");
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("PasswordHash is a required field.");
    }
}
