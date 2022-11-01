using Financity.Application.Labels.Commands;
using FluentValidation;

namespace Financity.Application.Labels.Validators;

public sealed class CreateLabelValidator : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.WalletId).NotEmpty();
    }
}