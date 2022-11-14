using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using FluentValidation;
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
    private readonly IMapper _mapper;

    public RevokeWalletAccessCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RevokeWalletAccessCommand command, CancellationToken ct)
    {
        var wallet = await _dbContext.GetDbSet<Wallet>().Include(x => x.UsersWithAccess)
                                     .FirstOrDefaultAsync(x => x.Id == command.WalletId, ct);

        if (wallet is null)
            throw ValidationExceptionFactory.For(nameof(command.WalletId), "The given wallet doesn't exist");

        var user = await _dbContext.GetDbSet<User>()
                                   .FirstOrDefaultAsync(x => x.NormalizedEmail == command.UserEmail.ToUpper(), ct);

        if (user is null)
            throw ValidationExceptionFactory.For(nameof(command.UserEmail), "User with given email doesn't exist");

        wallet.UsersWithAccess ??= new List<WalletAccess>();

        var walletAccess = wallet.UsersWithAccess.FirstOrDefault(x => x.UserId == user.Id);

        if (walletAccess is null)
        {
            throw ValidationExceptionFactory.For(nameof(command.UserEmail),
                "User with given email doesn't have access to the given wallet.");
        }

        if (walletAccess.WalletAccessLevel == WalletAccessLevel.Owner)
            throw ValidationExceptionFactory.For(nameof(command.UserEmail),
                "You cannot revoke access from the wallet owner.");

        wallet.UsersWithAccess.Remove(walletAccess);

        await _dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}