namespace Financity.Domain.Shared;

public class AuditableEntity : Entity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    
    public AuditableEntity(Guid id) : base(id)
    {
    }
}