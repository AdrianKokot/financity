using Financity.Application.Abstractions.Data;
using Financity.Application.Categories.Commands;
using Financity.Application.Common.Extensions;
using FluentValidation;

namespace Financity.Application.Categories.Validators;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator(IApplicationDbContext dbContext, ICurrentUserService userService)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.WalletId).NotEmpty().HasAccessToWallet(dbContext, userService);
        RuleFor(x => x.Appearance).ChildRules(x =>
        {
            x.RuleFor(y => y.Color).MaximumLength(64);
            x.RuleFor(y => y.IconName).MaximumLength(64);
        });
    }
}