using Financity.Domain.Common;
using Financity.Domain.Enums;

namespace Financity.Domain.Entities;

public sealed class Category : AuditableEntity
{
    public string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public TransactionType? TransactionType { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
}