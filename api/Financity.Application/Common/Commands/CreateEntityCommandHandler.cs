using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;

namespace Financity.Application.Common.Commands;

public abstract class CreateEntityCommandHandler<TCommand, TResult, TEntity> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>, IMapTo<TEntity>
    where TEntity : Entity
    where TResult : IMapFrom<TEntity>
{
    protected readonly IApplicationDbContext DbContext;
    protected readonly IMapper Mapper;

    protected CreateEntityCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = mapper;
    }

    public virtual async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<TEntity>(command);

        DbContext.GetDbSet<TEntity>().Add(entity);

        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<TResult>(entity);
    }
}