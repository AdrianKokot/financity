using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Currencies.Queries;

public sealed class GetCurrenciesQuery : FilteredEntitiesQuery<Currency>
{
    public GetCurrenciesQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class GetCurrenciesQueryHandler : FilteredEntitiesQueryHandler<GetCurrenciesQuery, Currency>

{
    public GetCurrenciesQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}