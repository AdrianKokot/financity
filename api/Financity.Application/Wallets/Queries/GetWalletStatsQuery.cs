using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletStatsQuery : IQuery<WalletStats>
{
    public Guid WalletId { get; set; } = Guid.Empty;
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
}

public sealed class GetWalletStatsQueryHandler : IQueryHandler<GetWalletStatsQuery, WalletStats>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletStatsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletStats> Handle(GetWalletStatsQuery request, CancellationToken cancellationToken)
    {
        var walletQueryable = _dbContext.GetDbSet<Wallet>()
                                        .AsNoTracking()
                                        .Where(x => _dbContext.UserService.UserWalletIds.Contains(x.Id) &&
                                                    x.Id == request.WalletId);



        var expensesByCategory = await walletQueryable
                                       .SelectMany(x => x.Transactions.Where(t =>
                                           t.TransactionType == TransactionType.Expense &&
                                           (request.From == null || t.TransactionDate >= request.From) &&
                                           (request.To == null || t.TransactionDate <= request.To)))
                                       .GroupBy(t => new
                                       {
                                           Id = t.CategoryId,
                                           Name = t.Category == null ? string.Empty : t.Category.Name
                                       })
                                       .Select(x => new CategoryExpenses(x.Key.Id, x.Key.Name,
                                           x.Sum(t => t.Amount * t.ExchangeRate)))
                                       .ToListAsync(cancellationToken);

        var currencyId = await walletQueryable.Select(x => x.CurrencyId).FirstAsync(cancellationToken);

        return new WalletStats(expensesByCategory, currencyId);
    }
}

public sealed record CategoryExpenses(Guid? Id, string Name, decimal Expenses);

public sealed record WalletStats(List<CategoryExpenses> ExpensesByCategory, string CurrencyId);