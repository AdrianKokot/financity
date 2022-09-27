using Financity.Domain.Shared;

namespace Financity.Domain.Entities;

public sealed class Label : AuditableEntity
{
    public string Name { get; set; }
    
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    
    public ICollection<Transaction> Transactions { get; set; }

    public Label() : base(Guid.NewGuid())
    {
    }
}