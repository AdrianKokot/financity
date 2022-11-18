using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Label : Entity, IBelongsToWallet
{
    public string Name { get; set; } = string.Empty;
    public Appearance Appearance { get; set; } = new();

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
}