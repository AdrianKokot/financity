namespace Financity.Domain.Common;

public interface IEntity
{
    public Guid Id { get; }
}
public abstract class Entity : IEntity
{
    protected Entity() : this(Guid.NewGuid())
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType() || obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}