using Financity.Application.Abstractions.Data;
using Financity.Application.Common.FilteredQuery;
using Financity.Domain.Entities;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : FilteredEntitiesQuery<Transaction>
{
    public GetTransactionsQuery(QuerySpecification querySpecification, string? walletId) : base(querySpecification)
    {
        WalletId = string.IsNullOrEmpty(walletId) ? null : Guid.Parse(walletId);
    }

    public Guid? WalletId { get; set; }
}

public sealed class GetTransactionQueryHandler : FilteredEntitiesQueryHandler<GetTransactionsQuery, Transaction>
{
    public GetTransactionQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
    {
    }


    public new async Task<IEnumerable<Transaction>> Handle(GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        return await AccessAsync(q =>
                q.Where(x => x.WalletId == request.WalletId)
                    .Paginate(request.QuerySpecification.PaginationSpecification),
            cancellationToken);
    }
}