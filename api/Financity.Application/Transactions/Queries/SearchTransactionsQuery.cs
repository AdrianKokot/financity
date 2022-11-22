using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Extensions;
using Financity.Application.Common.Queries;
using Financity.Application.Common.Queries.FilteredQuery;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Queries;

public sealed class SearchTransactionsQuery : FilteredEntitiesQuery<TransactionListItem>
{
    public SearchTransactionsQuery(QuerySpecification<TransactionListItem> querySpecification) : base(
        querySpecification)
    {
    }

    public Guid? WalletId { get; set; } = null;
    public string SearchTerm { get; set; } = string.Empty;
}

public sealed class
    SearchTransactionsQueryHandler : FilteredEntitiesQueryHandler<SearchTransactionsQuery, Transaction,
        TransactionListItem>
{
    public SearchTransactionsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<TransactionListItem>> Handle(SearchTransactionsQuery query,
                                                                        CancellationToken cancellationToken)
    {
        return await DbContext.SearchUserTransactions(DbContext.UserService.UserId, query.SearchTerm, query.WalletId)
                              .AsNoTracking()
                              .Where(x => DbContext.UserService.UserWalletIds.Contains(x.WalletId))
                              .ApplyQuerySpecification(query.QuerySpecification)
                              .ProjectTo<TransactionListItem>(Mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
    }
}