using Financity.Application.Recipients.Commands;
using FluentValidation;

namespace Financity.Application.Recipients.Validators;

public sealed class CreateRecipientValidator : AbstractValidator<CreateRecipientCommand>
{
    public CreateRecipientValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.WalletId).NotEmpty();
    }
}