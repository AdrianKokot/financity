using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Common.Queries.FilteredQuery;

public interface IFilteredEntitiesListQuery<TEntity> : IQuery<IEnumerable<TEntity>> where TEntity : class
{
    public QuerySpecification<TEntity> QuerySpecification { get; set; }
}

public abstract class FilteredEntitiesQuery<TEntity> : IFilteredEntitiesListQuery<TEntity> where TEntity : class
{
    protected FilteredEntitiesQuery(QuerySpecification<TEntity> querySpecification)
    {
        QuerySpecification = querySpecification;
    }

    public QuerySpecification<TEntity> QuerySpecification { get; set; }
}