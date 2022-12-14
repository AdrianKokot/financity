using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Recipients.Commands;
using FluentValidation;

namespace Financity.Application.Recipients.Validators;

public sealed class CreateRecipientValidator : AbstractValidator<CreateRecipientCommand>
{
    public CreateRecipientValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.WalletId).NotEmpty().HasUserAccessToWallet(dbContext);
    }
}