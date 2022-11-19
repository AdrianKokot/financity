using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Extensions;
using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.Queries.FilteredQuery;

public abstract class
    FilteredEntitiesQueryHandler<TQuery, TEntity, TMappedEntity> : IQueryHandler<TQuery,
        IEnumerable<TMappedEntity>>
    where TEntity : Entity
    where TMappedEntity : class
    where TQuery : IFilteredEntitiesListQuery<TMappedEntity>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    protected FilteredEntitiesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private DbSet<TEntity> Set => _dbContext.GetDbSet<TEntity>();

    public virtual Task<IEnumerable<TMappedEntity>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(request, cancellationToken);
    }

    protected async Task<IEnumerable<TMappedEntity>> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).ToListAsync(cancellationToken);
    }

    protected Task<IEnumerable<TMappedEntity>> FilterAndMapAsync(TQuery request,
                                                                 CancellationToken cancellationToken = default)
    {
        return AccessAsync(q =>
                q.Paginate(request.QuerySpecification.PaginationSpecification)
                 .ProjectTo<TMappedEntity>(_mapper.ConfigurationProvider),
            cancellationToken);
    }

    protected Task<IEnumerable<TMappedEntity>> FilterAndMapAsync(TQuery request,
                                                                 Func<IQueryable<TEntity>, IQueryable<TEntity>>
                                                                     expression,
                                                                 CancellationToken cancellationToken = default)
    {
        return AccessAsync(q =>
                expression.Invoke(q).Paginate(request.QuerySpecification.PaginationSpecification)
                          .ProjectTo<TMappedEntity>(_mapper.ConfigurationProvider),
            cancellationToken);
    }
}

public abstract class
    FilteredEntitiesQueryHandler<TQuery, TEntity> : IQueryHandler<TQuery, IEnumerable<TEntity>>
    where TEntity : Entity
    where TQuery : IFilteredEntitiesListQuery<TEntity>
{
    private readonly IApplicationDbContext _dbContext;

    protected FilteredEntitiesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private DbSet<TEntity> Set => _dbContext.GetDbSet<TEntity>();

    public Task<IEnumerable<TEntity>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return AccessAsync(q => q.Paginate(request.QuerySpecification.PaginationSpecification),
            cancellationToken);
    }

    protected async Task<IEnumerable<TEntity>> AccessAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression,
                                                           CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).ToListAsync(cancellationToken);
    }
}