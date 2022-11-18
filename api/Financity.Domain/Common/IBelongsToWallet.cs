using Financity.Domain.Entities;

namespace Financity.Domain.Common;

public interface IBelongsToWallet
{
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; }
}