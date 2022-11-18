using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Labels.Commands;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Labels.Validators;

public sealed class UpdateLabelValidator : AbstractValidator<UpdateLabelCommand>
{
    public UpdateLabelValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Appearance).ChildRules(x =>
        {
            x.RuleFor(y => y.Color).MaximumLength(64);
            x.RuleFor(y => y.IconName).MaximumLength(64);
        });


        RuleFor(x => x.Id).ExistsForCurrentUser<UpdateLabelCommand, Label>(dbContext);
    }
}