using Financity.Application.Auth.Commands;
using Financity.Application.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Financity.Application.Auth.Validators;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator(IOptions<IdentityOptions> options)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.Password)
            .Password(options.Value.Password);
    }
}