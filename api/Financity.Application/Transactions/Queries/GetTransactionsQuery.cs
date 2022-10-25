using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Transactions.Queries;

public sealed class GetTransactionsQuery : IQuery<IEnumerable<TransactionListItem>>
{
    public string? WalletId { get; set; } = null;
}

public sealed class GetTransactionQueryHandler : IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionListItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTransactionQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TransactionListItem>> Handle(GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactionsQuery = _dbContext.Transactions
            .Include(x => x.Currency)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.WalletId))
            transactionsQuery = transactionsQuery
                .Where(x => x.WalletId.Equals(Guid.Parse(request.WalletId)))
                .AsQueryable();


        return await transactionsQuery.Take(20).Select(x => new TransactionListItem
        {
            Id = x.Id,
            CurrencyId = x.Currency.Id,
            CurrencyCode = x.Currency.Code,
            CurrencyName = x.Currency.Name
        })
            .ToListAsync(cancellationToken);
    }
}

public sealed class TransactionListItem
{
    public Guid Id { get; set; }
    public Guid CurrencyId { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencyCode { get; set; }
}