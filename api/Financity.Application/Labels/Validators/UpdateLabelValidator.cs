using Financity.Application.Labels.Commands;
using FluentValidation;

namespace Financity.Application.Labels.Validators;

public sealed class UpdateLabelValidator : AbstractValidator<UpdateLabelCommand>
{
    public UpdateLabelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Color).MaximumLength(64);
        RuleFor(x => x.IconName).MaximumLength(64);
    }
}