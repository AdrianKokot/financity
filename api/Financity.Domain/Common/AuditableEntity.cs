namespace Financity.Domain.Common;

public class AuditableEntity : Entity
{
    public DateTime? CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}