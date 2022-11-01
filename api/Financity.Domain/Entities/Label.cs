using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Label : AuditableEntity
{
    public string Name { get; set; }
    public string? Color { get; set; }
    public string? IconName { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
}