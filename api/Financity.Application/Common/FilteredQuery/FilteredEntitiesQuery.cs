using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Common.FilteredQuery;

public interface IFilteredEntitiesListQuery<out TEntity> : IQuery<IEnumerable<TEntity>> where TEntity : class
{
    public QuerySpecification QuerySpecification { get; set; }
}

public class FilteredEntitiesQuery<TEntity> : IFilteredEntitiesListQuery<TEntity> where TEntity : class
{
    protected FilteredEntitiesQuery(QuerySpecification querySpecification)
    {
        QuerySpecification = querySpecification;
    }

    public QuerySpecification QuerySpecification { get; set; }
}