using Financity.Domain.Enums;
using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Category : AuditableEntity
{
    public string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public TransactionType? TransactionType { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public Category() : base(Guid.NewGuid())
    {
    }
}