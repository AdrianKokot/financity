using Financity.Application.Abstractions.Data;
using Financity.Application.Categories.Commands;
using Financity.Application.Common.Extensions;
using Financity.Domain.Enums;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.WalletId).NotEmpty().HasUserAccessToWallet(dbContext);
        RuleFor(x => x.TransactionType).IsEnumName(typeof(TransactionType), false);
        RuleFor(x => x.Appearance).ChildRules(x =>
        {
            x.RuleFor(y => y.Color).MaximumLength(64);
            x.RuleFor(y => y.IconName).MaximumLength(64);
        });
    }
}