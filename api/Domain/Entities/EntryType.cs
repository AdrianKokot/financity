using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public class EntryType : AuditableEntity
{
    public string Name { get; set; }
}