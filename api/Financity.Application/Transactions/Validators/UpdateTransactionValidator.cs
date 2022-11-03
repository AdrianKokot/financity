using Financity.Application.Transactions.Commands;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(255);
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.LabelIds).NotNull();
    }
}