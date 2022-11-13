using Financity.Domain.Common;

namespace Financity.Domain.Entities;

public sealed class Budget : Entity
{
    public string? Name { get; set; }
    public decimal Amount { get; set; }

    public Guid UserId { get; set; }
    public Guid? User { get; set; }

    public ICollection<Category>? TrackedCategories { get; set; }
}