﻿using System.Globalization;
using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Currencies.Queries;

public sealed class GetCurrenciesQuery : FilteredEntitiesQuery<CurrencyListItem>
{
    public GetCurrenciesQuery(QuerySpecification<CurrencyListItem> querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetCurrenciesQueryHandler : FilteredEntitiesQueryHandler<GetCurrenciesQuery, Currency, CurrencyListItem>

{
    public GetCurrenciesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    protected override IQueryable<Currency> ExecuteSearch(IQueryable<Currency> query, string search)
    {
        search = search.ToLower(CultureInfo.InvariantCulture);
        return query.Where(x => x.Id.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
    }
}

public sealed record CurrencyListItem(string Id, string Name) : IMapFrom<Currency>;