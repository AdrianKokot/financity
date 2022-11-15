using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Wallet : Entity
{
    public string Name { get; set; } = string.Empty;

    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }

    public ICollection<WalletAccess> UsersWithAccess { get; set; } = new List<WalletAccess>();

    public ICollection<Label> Labels { get; set; } = new List<Label>();
    public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}