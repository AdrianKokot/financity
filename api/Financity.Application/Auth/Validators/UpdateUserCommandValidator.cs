using Financity.Application.Auth.Commands;
using FluentValidation;

namespace Financity.Application.Auth.Validators;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}