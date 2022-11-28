namespace Financity.Application.Common.Exceptions;

public sealed class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string entityName, string propertyName) : base(
        $"{entityName} with given {propertyName} already exists.")
    {
        PropertyName = propertyName;
        EntityName = entityName;
    }

    public string PropertyName { get; }
    public string EntityName { get; }
}