using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Wallet : AuditableEntity
{
    public string Name { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public Guid DefaultCurrencyId { get; set; }
    public Currency DefaultCurrency { get; set; }

    public Wallet() : base(Guid.NewGuid())
    {
    }
}