using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class ResignWalletAccessCommand : ICommand<Unit>
{
    public Guid WalletId { get; set; } = Guid.Empty;
}

public sealed class ResignWalletAccessCommandHandler : ICommandHandler<ResignWalletAccessCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public ResignWalletAccessCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(ResignWalletAccessCommand command, CancellationToken ct)
    {
        var wallet = await _dbContext.GetDbSet<Wallet>()
                                     .Include(x => x.UsersWithSharedAccess)
                                     .FirstOrDefaultAsync(x => x.Id == command.WalletId, ct);

        if (wallet is null)
            throw ValidationExceptionFactory.For(nameof(command.WalletId), "The given wallet doesn't exist");

        var userWithSharedAccess =
            wallet.UsersWithSharedAccess.FirstOrDefault(x => x.Id == _dbContext.UserService.UserId);

        if (userWithSharedAccess is null)
            throw ValidationExceptionFactory.For(nameof(command.WalletId), "The given wallet doesn't exist");

        wallet.UsersWithSharedAccess.Remove(userWithSharedAccess);

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}