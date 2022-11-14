using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed record DeleteWalletCommand(Guid Id) : ICommand<Unit>;

public sealed class DeleteWalletCommandHandler : ICommandHandler<DeleteWalletCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteWalletCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteWalletCommand request,
                                   CancellationToken cancellationToken)
    {
        var entity = await _dbContext.GetDbSet<Wallet>()
                                     .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null) throw new EntityNotFoundException(nameof(Wallet), request.Id);

        _dbContext.GetDbSet<Wallet>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}