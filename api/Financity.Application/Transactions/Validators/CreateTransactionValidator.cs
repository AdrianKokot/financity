using Financity.Application.Transactions.Commands;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
}