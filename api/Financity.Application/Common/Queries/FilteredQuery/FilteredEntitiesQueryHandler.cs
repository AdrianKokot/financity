﻿using AutoMapper;
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
    where TEntity : class
    where TMappedEntity : class
    where TQuery : IFilteredEntitiesListQuery<TMappedEntity>
{
    protected readonly IApplicationDbContext DbContext;
    protected readonly IMapper Mapper;

    protected FilteredEntitiesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = mapper;
    }

    protected DbSet<TEntity> Set => DbContext.GetDbSet<TEntity>();

    public virtual Task<IEnumerable<TMappedEntity>> Handle(TQuery query, CancellationToken cancellationToken)
    {
        return FilterAndMapAsync(query, cancellationToken);
    }

    protected virtual IQueryable<TEntity> ExecuteSearch(IQueryable<TEntity> query, string search)
    {
        throw new NotImplementedException(nameof(ExecuteSearch) + " is missing on given type.");
    }

    protected virtual async Task<IEnumerable<TMappedEntity>> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).ToListAsync(cancellationToken);
    }

    protected virtual IQueryable<TMappedEntity> Project(IQueryable<TEntity> q, TQuery query)
    {
        return q.ProjectTo<TMappedEntity>(Mapper.ConfigurationProvider);
    }

    protected virtual Task<IEnumerable<TMappedEntity>> FilterAndMapAsync(TQuery query,
                                                                         CancellationToken cancellationToken = default)
    {
        return FilterAndMapAsync(query, q => q, cancellationToken);
    }

    protected virtual Task<IEnumerable<TMappedEntity>> FilterAndMapAsync(TQuery query,
                                                                         Func<IQueryable<TEntity>, IQueryable<TEntity>>
                                                                             expression,
                                                                         CancellationToken cancellationToken = default)
    {
        var expr = expression.Invoke;

        if (!string.IsNullOrEmpty(query.QuerySpecification.Search))
            expr = q => ExecuteSearch(expression.Invoke(q), query.QuerySpecification.Search);

        return AccessAsync(q =>
            Project(expr.Invoke(q)
                        .ApplyQuerySpecification(query.QuerySpecification), query), cancellationToken);
    }
}

public abstract class
    FilteredUserEntitiesQueryHandler<TQuery, TEntity, TMappedEntity> : FilteredEntitiesQueryHandler<TQuery, TEntity,
        TMappedEntity>
    where TEntity : Entity, IBelongsToUser
    where TMappedEntity : class
    where TQuery : IFilteredEntitiesListQuery<TMappedEntity>
{
    protected FilteredUserEntitiesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext,
        mapper)
    {
    }

    protected override async Task<IEnumerable<TMappedEntity>> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set
                                       .AsNoTracking()
                                       .Where(x => x.UserId == DbContext.UserService.UserId)
                               )
                               .ToListAsync(cancellationToken);
    }
}

public abstract class
    FilteredUserWalletEntitiesQueryHandler<TQuery, TEntity, TMappedEntity> : FilteredEntitiesQueryHandler<TQuery,
        TEntity,
        TMappedEntity>
    where TEntity : Entity, IBelongsToWallet
    where TMappedEntity : class
    where TQuery : IFilteredEntitiesListQuery<TMappedEntity>
{
    protected FilteredUserWalletEntitiesQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext,
        mapper)
    {
    }

    protected override async Task<IEnumerable<TMappedEntity>> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set
                                       .AsNoTracking()
                                       .Where(x => DbContext.UserService.UserWalletIds.Contains(x.WalletId))
                               )
                               .ToListAsync(cancellationToken);
    }
}