namespace Financity.Domain.Common;

public abstract class Entity
{
    protected Entity() : this(Guid.NewGuid())
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}