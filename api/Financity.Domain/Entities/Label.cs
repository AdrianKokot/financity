using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Label : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public Appearance Appearance { get; set; } = new();
    public Guid WalletId { get; set; }
    public Wallet? Wallet { get; set; }

    public ICollection<Transaction>? Transactions { get; set; }
}