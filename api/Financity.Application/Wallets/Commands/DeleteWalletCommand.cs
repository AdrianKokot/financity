using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Domain.Entities;
using MediatR;

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
        var deletedCount = await _dbContext.DeleteFromSetAsync<Wallet>(request.Id, cancellationToken);

        if (deletedCount == 0) throw new EntityNotFoundException(nameof(Wallet), request.Id);

        return Unit.Value;
    }
}