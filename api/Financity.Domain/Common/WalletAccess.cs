using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Domain.Common;

public sealed class WalletAccess
{
    public Guid WalletId { get; set; }
    public Wallet? Wallet { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public WalletAccessLevel WalletAccessLevel { get; set; }
}