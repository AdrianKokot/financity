using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Extensions;
using Financity.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.FilteredQuery;

public abstract class
    FilteredEntitiesQueryHandler<TQuery, TEntity, TMappedEntity> : IQueryHandler<TQuery,
        IEnumerable<TMappedEntity>>
    where TEntity : class
    where TMappedEntity : class
    where TQuery : IFilteredEntitiesListQuery<TMappedEntity>
{
    private readonly IApplicationDbContext _dbContext;

    protected FilteredEntitiesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private DbSet<TEntity> Set => _dbContext.GetDbSet<TEntity>();

    public async Task<IEnumerable<TMappedEntity>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await FilterAndMapAsync(request, cancellationToken);
    }

    protected async Task<IEnumerable<TMappedEntity>> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).ToListAsync(cancellationToken);
    }

    protected async Task<IEnumerable<TMappedEntity>> FilterAndMapAsync(TQuery request,
        CancellationToken cancellationToken = default)
    {
        return await AccessAsync(q =>
                q.Paginate(request.QuerySpecification.PaginationSpecification).Project<TEntity, TMappedEntity>(),
            cancellationToken);
    }
}

public abstract class
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