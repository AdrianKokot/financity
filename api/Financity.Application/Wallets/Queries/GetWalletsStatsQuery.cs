using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletsStatsQuery : IQuery<WalletStats>
{
    public HashSet<Guid> WalletIds { get; set; } = new();
    public DateOnly From { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
    public DateOnly To { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string CurrencyId { get; set; } = string.Empty;
}

public sealed class GetWalletsStatsQueryHandler : IQueryHandler<GetWalletsStatsQuery, WalletStats>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletsStatsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletStats> Handle(GetWalletsStatsQuery request, CancellationToken cancellationToken)
    {
        var q = _dbContext.GetDbSet<Wallet>().AsNoTracking();

        if (request.WalletIds.Count > 0)
        {
            q = q.Where(x => request.WalletIds.Contains(x.Id));
        }

        var dict = await q
                         .Where(x => x.CurrencyId == request.CurrencyId)
                         .SelectMany(x =>
                             x.Transactions.Where(t =>
                                 t.TransactionDate >= request.From && t.TransactionDate <= request.To))
                         .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month, x.TransactionType })
                         .ToDictionaryAsync(x => x.Key, x => x.Sum(y => y.Amount * y.ExchangeRate), cancellationToken);

        var resultDict = new Dictionary<TransactionType, Dictionary<string, decimal>>()
        {
            {TransactionType.Expense, new Dictionary<string, decimal>()},
            {TransactionType.Income, new Dictionary<string, decimal>()}
        };

        var currDate = request.From;

        while (currDate < request.To)
        {
            dict.TryGetValue(new
            {
                currDate.Year,
                currDate.Month,
                TransactionType = TransactionType.Expense
            }, out var expenseValue);

            dict.TryGetValue(new
            {
                currDate.Year,
                currDate.Month,
                TransactionType = TransactionType.Income
            }, out var incomeValue);

            var stringKey = $"{currDate.Year}-{currDate.Month:00}";
            resultDict[TransactionType.Expense].Add(stringKey, expenseValue);
            resultDict[TransactionType.Income].Add(stringKey, incomeValue);

            currDate = currDate.AddMonths(1);
        }

        return new WalletStats(resultDict[TransactionType.Expense], resultDict[TransactionType.Income]);
    }
}

public sealed record WalletStats(IDictionary<string, decimal> ExpenseStats, IDictionary<string, decimal> IncomeStats);