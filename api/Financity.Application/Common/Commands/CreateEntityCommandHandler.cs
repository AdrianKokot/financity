using AutoMapper;
using EntityFramework.Exceptions.Common;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
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

        await SaveChanges(cancellationToken);

        return Mapper.Map<TResult>(entity);
    }

    protected async Task SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException uniqueConstraintException)
        {
            var propertyName = (uniqueConstraintException.InnerException?.GetType()
                                                         .GetProperty("ConstraintName")
                                                         ?.GetValue(uniqueConstraintException.InnerException)?
                                                         .ToString() ?? string.Empty).Split("_")
                .Last();

            throw new EntityAlreadyExistsException(typeof(TEntity).Name, propertyName);
        }
    }
}