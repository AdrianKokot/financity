using Financity.Application.Auth.Commands;
using FluentValidation;

namespace Financity.Application.Auth.Validators;

public sealed class RequestResetPasswordCommandValidator : AbstractValidator<RequestResetPasswordCommand>
{
    public RequestResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}