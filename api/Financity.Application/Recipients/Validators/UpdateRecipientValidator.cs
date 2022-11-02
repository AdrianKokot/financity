using Financity.Application.Recipients.Commands;
using FluentValidation;

namespace Financity.Application.Recipients.Validators;

public sealed class UpdateRecipientValidator : AbstractValidator<UpdateRecipientCommand>
{
    public UpdateRecipientValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
    }
}