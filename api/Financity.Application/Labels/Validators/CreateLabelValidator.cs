using Financity.Application.Labels.Commands;
using FluentValidation;

namespace Financity.Application.Labels.Validators;

public sealed class CreateLabelValidator : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.Appearance).ChildRules(x =>
        {
            x.RuleFor(y => y.Color).MaximumLength(64);
            x.RuleFor(y => y.IconName).MaximumLength(64);
        });
    }
}