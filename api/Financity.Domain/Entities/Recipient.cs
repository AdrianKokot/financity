using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Recipient : AuditableEntity
{
    public string Name { get; set; }
    
    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
    
    public Recipient() : base(Guid.NewGuid())
    {
    }
}