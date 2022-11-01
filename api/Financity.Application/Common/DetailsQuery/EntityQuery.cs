using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Common.DetailsQuery;

public interface IEntityQuery<out TEntity> : IQuery<TEntity> where TEntity : class
{
    public Guid EntityId { get; }
}