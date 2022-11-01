using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
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
        return await _dbContext.GetDbSet<TEntity>()
            .Where(x => x.Id == request.EntityId)
            .Project<TEntity, TMappedEntity>()
            .FirstAsync(cancellationToken);
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
        return await _dbContext.GetDbSet<TEntity>()
            .FirstAsync(x => x.Id == request.EntityId, cancellationToken);
    }
}