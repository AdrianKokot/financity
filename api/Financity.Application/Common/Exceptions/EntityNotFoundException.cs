namespace Financity.Application.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, Guid entityId) : base(
        $"{entityName} with id {entityId.ToString()} doesn't exist.")
    {
        EntityId = entityId;
    }

    public Guid EntityId { get; }
}