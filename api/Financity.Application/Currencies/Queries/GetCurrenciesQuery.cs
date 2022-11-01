using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Application.Common.Mappings;
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

public sealed class CurrencyListItem : IMapFrom<Currency>
{
    public Guid Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
}