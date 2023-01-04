using Financity.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Financity.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Wallet> SharedWallets { get; set; } = new List<Wallet>();
    public ICollection<Wallet> OwnedWallets { get; set; } = new List<Wallet>();

    public override string Email { get; set; }
}