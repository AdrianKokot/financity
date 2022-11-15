using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

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
        var deletedCount = await _dbContext.DeleteFromSetAsync<Budget>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Budget), request.Id);

        return Unit.Value;
    }
}