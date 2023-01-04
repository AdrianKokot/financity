using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Currencies.Queries;

public sealed class GetCurrenciesUsedByUserQuery : FilteredEntitiesQuery<CurrencyListItem>
{
    public GetCurrenciesUsedByUserQuery(QuerySpecification<CurrencyListItem> querySpecification) : base(
        querySpecification)
    {
    }
}

public sealed class
    GetCurrenciesUsedByUserQueryHandler : FilteredEntitiesQueryHandler<GetCurrenciesUsedByUserQuery, Currency,
        CurrencyListItem>

{
    public GetCurrenciesUsedByUserQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext,
        mapper)
    {
    }

    public override async Task<IEnumerable<CurrencyListItem>> Handle(GetCurrenciesUsedByUserQuery query,
                                                               CancellationToken cancellationToken)
    {
        return await DbContext.GetDbSet<Wallet>().AsNoTracking()
                              .Where(x => DbContext.UserService.UserWalletIds.Contains(x.Id))
                              .Where(x => x.Transactions.Any())
                              .Select(x => x.Currency)
                              .Distinct()
                              .ApplyQuerySpecification(query.QuerySpecification)
                              .ProjectTo<CurrencyListItem>(Mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
    }
}