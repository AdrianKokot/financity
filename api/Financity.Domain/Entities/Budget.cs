using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Budget : Entity, IBelongsToUser
{
    public string Name { get; set; } = string.Empty;
    public string CurrencyId { get; set; } = string.Empty;
    public Currency Currency { get; set; }

    /// <summary>
    ///     Budget figure
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///     The sum of transactions in current period (this month) for tracked categories
    /// </summary>
    public decimal CurrentPeriodExpenses =>
        TrackedCategories.Where(x => x.TransactionType == TransactionType.Expense && x.Wallet.CurrencyId == CurrencyId)
                         .Sum(c => c.Transactions
                                    .Where(t => t.TransactionDate.Year == DateTime.UtcNow.Year &&
                                                t.TransactionDate.Month == DateTime.UtcNow.Month)
                                    .Sum(t => t.Amount * t.ExchangeRate));

    public ICollection<Category> TrackedCategories { get; set; } = new List<Category>();

    public Guid UserId { get; set; }
    public User User { get; set; }
}