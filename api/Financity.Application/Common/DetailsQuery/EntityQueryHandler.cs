using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Application.Common.Extensions;
using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.DetailsQuery;

public class EntityQueryHandler<TQuery, TEntity, TMappedEntity> : IQueryHandler<TQuery, TMappedEntity>
    where TEntity : Entity
    where TMappedEntity : class
    where TQuery : IEntityQuery<TMappedEntity>
{
    private readonly IApplicationDbContext _dbContext;

    protected EntityQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TMappedEntity> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<TEntity>()
            .Where(x => x.Id == request.EntityId)
            .Project<TEntity, TMappedEntity>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(TEntity), request.EntityId);

        return entity;
    }
}

public class
    EntityQueryHandler<TQuery, TEntity> : IQueryHandler<TQuery, TEntity>
    where TEntity : Entity
    where TQuery : IEntityQuery<TEntity>
{
    private readonly IApplicationDbContext _dbContext;

    protected EntityQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == request.EntityId, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(TEntity), request.EntityId);

        return entity;
    }
}