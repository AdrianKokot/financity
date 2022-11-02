using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : FilteredEntitiesQuery<TransactionListItem>
{
    public GetTransactionsQuery(QuerySpecification querySpecification) : base(querySpecification)
    {
    }
}

public sealed class
    GetTransactionQueryHandler : FilteredEntitiesQueryHandler<GetTransactionsQuery, Transaction, TransactionListItem>
{
    public GetTransactionQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

public sealed record TransactionListItem(Guid Id, decimal Amount, string WalletName);