using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Currencies.Queries;

public sealed class GetCurrenciesQuery : FilteredEntitiesQuery<CurrencyListItem>
{
    public GetCurrenciesQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetCurrenciesQueryHandler : FilteredEntitiesQueryHandler<GetCurrenciesQuery, Currency, CurrencyListItem>

{
    public GetCurrenciesQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record CurrencyListItem(Guid Id, string? Code, string? Name);