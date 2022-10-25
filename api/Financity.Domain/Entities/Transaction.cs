using Financity.Domain.Enums;
using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Transaction : AuditableEntity
{
    public Transaction() : base(Guid.NewGuid())
    {
    }

    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public int? RecipientId { get; set; }
    public Recipient? Recipient { get; set; }

    public Guid WalletId { get; set; }
    // public Wallet Wallet { get; set; }

    public ICollection<Label> Labels { get; set; }

    public TransactionType TransactionType { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
}