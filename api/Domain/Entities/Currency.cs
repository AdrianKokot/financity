using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public class Currency : AuditableEntity
{
    public string Code { get; set; }
}