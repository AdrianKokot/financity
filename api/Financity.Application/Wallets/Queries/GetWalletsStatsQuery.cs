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
                         .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
                         .ToDictionaryAsync(x => $"{x.Key.Year}-{x.Key.Month}", x => x.Sum(y => y.Amount *
                             y.ExchangeRate *
                             (y.TransactionType == TransactionType.Expense
                                 ? -1
                                 : 1)), cancellationToken);

        var startingAmount = await q.SumAsync(x => x.StartingAmount, cancellationToken);

        var reducedValues = new Dictionary<string, decimal>();
        var orderedKeys = dict.Keys.ToList().Order().ToList();

        if (orderedKeys.Count > 0)
        {
            reducedValues.Add(orderedKeys[0], dict[orderedKeys[0]] + startingAmount);

            for (var i = 1; i < orderedKeys.Count; i++)
            {
                reducedValues.Add(orderedKeys[i], dict[orderedKeys[i]] + dict[orderedKeys[i - 1]]);
            }
        }

        return new WalletStats(dict, reducedValues);
    }
}

public sealed record WalletStats(IDictionary<string, decimal> ExpenseStats, IDictionary<string, decimal> ChartData);