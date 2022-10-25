using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Account : Entity
{
    public Account() : base(Guid.NewGuid())
    {
    }

    public ICollection<Wallet> Wallets { get; set; }
    public ICollection<Label> Labels { get; set; }
    public ICollection<Recipient> Recipients { get; set; }
    public ICollection<Category> Categories { get; set; }
}