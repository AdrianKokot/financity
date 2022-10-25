using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.FilteredQuery;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationSpecification specification)
    {
        return query.Take(specification.Take).Skip(specification.Skip);
    }
}

public class
    FilteredEntitiesQueryHandler<TQuery, TEntity> : IQueryHandler<TQuery, IEnumerable<TEntity>>
    where TEntity : class
    where TQuery : IFilteredEntitiesListQuery<TEntity>
{
    private readonly IApplicationDbContext _dbContext;

    protected FilteredEntitiesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private DbSet<TEntity> Set => _dbContext.GetDbSet<TEntity>();

    public async Task<IEnumerable<TEntity>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await AccessAsync(q => q.Paginate(request.QuerySpecification.PaginationSpecification),
            cancellationToken);
    }

    protected async Task<IEnumerable<TEntity>> AccessAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression,
        CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).ToListAsync(cancellationToken);
    }
}