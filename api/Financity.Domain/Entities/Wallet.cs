using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Wallet : Entity
{
    public string? Name { get; set; }

    public Guid CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Label>? Labels { get; set; }
    public ICollection<Recipient>? Recipients { get; set; }
    public ICollection<Category>? Categories { get; set; }
}