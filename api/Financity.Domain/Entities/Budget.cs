using Financity.Domain.Common;

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
    public decimal CurrentPeriodExpenses => 0;

    public ICollection<Category> TrackedCategories { get; set; } = new List<Category>();

    public Guid UserId { get; set; }
    public User User { get; set; }
}