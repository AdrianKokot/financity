﻿using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class RevokeWalletAccessCommand : ICommand<Unit>
{
    public Guid WalletId { get; set; } = Guid.Empty;
    public string UserEmail { get; set; } = string.Empty;
}

public sealed class RevokeWalletAccessCommandHandler : ICommandHandler<RevokeWalletAccessCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public RevokeWalletAccessCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(RevokeWalletAccessCommand command, CancellationToken ct)
    {
        var wallet = await _dbContext.GetDbSet<Wallet>()
                                     .Include(x => x.UsersWithSharedAccess)
                                     .FirstOrDefaultAsync(x => x.Id == command.WalletId, ct);

        if (wallet is null)
            throw ValidationExceptionFactory.For(nameof(command.WalletId), "The given wallet doesn't exist");

        var user = await _dbContext.GetDbSet<User>()
                                   .FirstOrDefaultAsync(x => x.NormalizedEmail == command.UserEmail.ToUpper(), ct);

        if (user is null)
            throw ValidationExceptionFactory.For(nameof(command.UserEmail), "User with given email doesn't exist");

        var userWithSharedAccess = wallet.UsersWithSharedAccess.FirstOrDefault(x => x.Id == user.Id);

        if (userWithSharedAccess is null)
            throw ValidationExceptionFactory.For(nameof(command.UserEmail),
                "User with given email doesn't have access to the given wallet.");

        wallet.UsersWithSharedAccess.Remove(userWithSharedAccess);

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}