using Financity.Application.Abstractions.Data;
using Financity.Application.Transactions.Queries;
using FluentValidation;

namespace Financity.Application.Transactions.Validators;

public sealed class GetExchangeRateQueryValidator : AbstractValidator<GetExchangeRateQuery>
{
    public GetExchangeRateQueryValidator()
    {
        RuleFor(x => x.Date)
            .Must(x => x.ToUniversalTime() <= AppDateTime.Now.ToUniversalTime())
            .WithMessage("Couldn't get exchange rate for future date");

        RuleFor(x => x.Date)
            .Must(x => x.ToUniversalTime() >= AppDateTime.Now.AddYears(-10).ToUniversalTime())
            .WithMessage("Couldn't get exchange rate for given date");
    }
}