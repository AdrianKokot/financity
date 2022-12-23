using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Transaction : Entity, IBelongsToWallet
{
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateOnly TransactionDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public decimal ExchangeRate { get; set; } = 1;

    public Guid? RecipientId { get; set; }
    public Guid? RecipientWalletId { get; set; }
    public Recipient? Recipient { get; set; }

    public ICollection<Label> Labels { get; set; } = new List<Label>();

    public TransactionType TransactionType { get; set; }

    public Guid? CategoryId { get; set; }
    public Guid? CategoryWalletId { get; set; }
    public TransactionType? CategoryTransactionType { get; set; }
    public Category? Category { get; set; }

    public string CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
}