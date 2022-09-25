using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public class Account : AuditableEntity
{
    public string Name { get; set; }

    public int? DefaultCurrencyId { get; set; }
    public Currency? DefaultCurrency { get; set; }

    private ICollection<Entry> Entries { get; set; }
}