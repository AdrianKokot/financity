namespace Financity.Application.Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, Guid id) : base(
        $"{entityName} with id {id.ToString()} doesn't exist.")
    {
    }
}