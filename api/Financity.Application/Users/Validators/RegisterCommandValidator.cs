using Financity.Application.Common.Extensions;
using Financity.Application.Users.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Financity.Application.Users.Validators;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IOptions<IdentityOptions> options)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .Password(options.Value.Password);
    }
}