namespace Financity.Application.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public Guid EntityId { get; }

    public EntityNotFoundException(string entityName, Guid entityId) : base(
        $"{entityName} with id {entityId.ToString()} doesn't exist.")
    {
        EntityId = entityId;
    }
}