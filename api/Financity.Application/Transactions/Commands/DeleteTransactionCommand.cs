using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var entity = await _dbContext.GetDbSet<Transaction>()
                                     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Transaction), request.Id);

        _dbContext.GetDbSet<Transaction>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}