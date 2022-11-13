using AutoMapper;
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
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    protected EntityQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TMappedEntity> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<TEntity>()
                                     .Where(x => x.Id == request.EntityId)
                                     .ProjectTo<TMappedEntity>(_mapper.ConfigurationProvider)
                                     .FirstOrDefaultAsync(cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(TEntity), request.EntityId);

        return entity;
    }
}

public abstract class
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