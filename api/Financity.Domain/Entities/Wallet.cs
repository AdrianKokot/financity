using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Wallet : Entity
{
    public string Name { get; set; } = string.Empty;

    public Guid CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    public ICollection<WalletAccess>? UsersWithAccess { get; set; }

    public ICollection<Label>? Labels { get; set; }
    public ICollection<Recipient>? Recipients { get; set; }
    public ICollection<Category>? Categories { get; set; }
}