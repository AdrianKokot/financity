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
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    protected CreateEntityCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TEntity>(command);

        _dbContext.GetDbSet<TEntity>().Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TResult>(entity);
    }
}