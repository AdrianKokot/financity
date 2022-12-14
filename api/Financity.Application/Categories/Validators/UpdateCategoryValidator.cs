using Financity.Application.Abstractions.Data;
using Financity.Application.Categories.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Entities;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Appearance).ChildRules(x =>
        {
            x.RuleFor(y => y.Color).MaximumLength(64);
            x.RuleFor(y => y.IconName).MaximumLength(64);
        });

        RuleFor(x => x.Id).HasUserAccess<UpdateCategoryCommand, Category>(dbContext);
    }
}