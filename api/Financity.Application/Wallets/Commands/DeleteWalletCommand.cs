using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
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

    public async Task<Unit> Handle(DeleteWalletCommand request, CancellationToken ct)
    {
        var deletedCount = await _dbContext.DeleteWalletAsync(request.Id, ct);

        // if (deletedCount ) throw new EntityNotFoundException(nameof(Wallet), request.Id);

        return Unit.Value;
    }
}