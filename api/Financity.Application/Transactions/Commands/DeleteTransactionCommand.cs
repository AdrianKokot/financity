using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

namespace Financity.Application.Transactions.Commands;

public sealed record DeleteTransactionCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteTransactionCommandHandler : ICommandHandler<DeleteTransactionCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteTransactionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request,
                                   CancellationToken cancellationToken)
    {
        var deletedCount = await _dbContext.DeleteFromSetAsync<Transaction>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Transaction), request.Id);

        return Unit.Value;
    }
}