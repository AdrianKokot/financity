﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.Queries.DetailsQuery;

public abstract class EntityQueryHandler<TQuery, TEntity, TMappedEntity> : IQueryHandler<TQuery, TMappedEntity>
    where TEntity : class, IEntity
    where TMappedEntity : class
    where TQuery : IEntityQuery<TMappedEntity>
{
    protected readonly IApplicationDbContext DbContext;
    protected readonly IMapper Mapper;

    protected EntityQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = mapper;
    }

    protected DbSet<TEntity> Set => DbContext.GetDbSet<TEntity>();

    public virtual async Task<TMappedEntity> Handle(TQuery query, CancellationToken cancellationToken)
    {
        return await FilterAndMapAsync(query, q => q.Where(x => x.Id == query.EntityId), cancellationToken)
               ?? throw new EntityNotFoundException(nameof(TEntity), query.EntityId);
    }

    protected virtual async Task<TMappedEntity?> AccessAsync(
        Func<IQueryable<TEntity>, IQueryable<TMappedEntity>> expression, CancellationToken cancellationToken = default)
    {
        return await expression.Invoke(Set.AsNoTracking()).FirstOrDefaultAsync(cancellationToken);
    }

    protected virtual Task<TMappedEntity?> FilterAndMapAsync(TQuery query,
                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>>
                                                                 expression,
                                                             CancellationToken cancellationToken = default)
    {
        return AccessAsync(
            q => expression.Invoke(q).ProjectTo<TMappedEntity>(Mapper.ConfigurationProvider),
            cancellationToken
        );
    }
}