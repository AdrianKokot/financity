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
        TrackedCategories.SelectMany(x => x.Transactions)
                         .Where(x => x.TransactionType == TransactionType.Expense &&
                                     x.TransactionDate.Month == DateTime.Now.ToUniversalTime().Month &&
                                     x.TransactionDate.Year == DateTime.Now.ToUniversalTime().Year &&
                                     x.Wallet.CurrencyId == CurrencyId)
                         .Sum(x => x.Amount * x.ExchangeRate);

    public ICollection<Category> TrackedCategories { get; set; } = new List<Category>();

    public Guid UserId { get; set; }
    public User User { get; set; }
}