using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public class Recipient : AuditableEntity
{
    public string Name { get; set; }
}