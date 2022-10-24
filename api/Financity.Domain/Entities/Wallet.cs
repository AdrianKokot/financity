using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Wallet : AuditableEntity
{
    public Wallet() : base(Guid.NewGuid())
    {
    }

    public string Name { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public Guid DefaultCurrencyId { get; set; }
    public Currency DefaultCurrency { get; set; }
}