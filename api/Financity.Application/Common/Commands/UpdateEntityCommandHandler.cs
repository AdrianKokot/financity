using EntityFramework.Exceptions.Common;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Common;
using MediatR;

namespace Financity.Application.Common.Commands;

public abstract class UpdateEntityCommandHandler<TCommand, TEntity> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
    where TEntity : Entity
{
    protected readonly IApplicationDbContext DbContext;

    protected UpdateEntityCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
    {
        return await SaveChangesWithExceptionHandle(cancellationToken);
    }

    private async Task<Unit> SaveChangesWithExceptionHandle(CancellationToken cancellationToken)
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

        return Unit.Value;
    }
}