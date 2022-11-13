using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Budgets.Commands;

public sealed record DeleteBudgetCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteBudgetCommandHandler : ICommandHandler<DeleteBudgetCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteBudgetCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteBudgetCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<Budget>()
                                     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Budget), request.Id);

        _dbContext.GetDbSet<Budget>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}