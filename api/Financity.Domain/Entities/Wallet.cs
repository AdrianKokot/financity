using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Wallet : Entity
{
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public decimal StartingAmount { get; set; }

    public decimal CurrentState =>
        Transactions.Sum(x => (x.TransactionType == TransactionType.Income ? 1 : -1) * x.Amount * x.ExchangeRate) +
        StartingAmount;

    public ICollection<User> UsersWithSharedAccess { get; set; } = new List<User>();

    public ICollection<Label> Labels { get; set; } = new List<Label>();
    public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}