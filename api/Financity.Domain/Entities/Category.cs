using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; set; } = string.Empty;

    public Appearance Appearance { get; set; } = new();

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public TransactionType? TransactionType { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}