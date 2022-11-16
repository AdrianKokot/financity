using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Transaction : Entity
{
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public Guid? RecipientId { get; set; }
    public Recipient? Recipient { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public ICollection<Label> Labels { get; set; } = new List<Label>();

    public TransactionType TransactionType { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
}