using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Label : AuditableEntity
{
    public string Name { get; set; }

    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
}