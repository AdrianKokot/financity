using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.DetailsQuery;

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