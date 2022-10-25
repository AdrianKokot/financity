using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Common;

namespace Financity.Application.Common.DetailsQuery;

public interface IEntityQuery<out TEntity> : IQuery<TEntity> where TEntity : Entity
{
    public Guid EntityId { get; set; }
}

public class EntityQuery<TEntity> : IEntityQuery<TEntity> where TEntity : Entity
{
    protected EntityQuery(string entityId)
    {
        EntityId = Guid.Parse(entityId);
    }

    public Guid EntityId { get; set; }
}