using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Application.Common.Mappings;
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

public sealed class TransactionListItem : IMapFrom<Transaction>
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public string WalletName { get; init; }
}