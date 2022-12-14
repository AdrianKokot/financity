using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class GiveWalletAccessCommand : ICommand<GiveWalletAccessCommandResult>
{
    public Guid WalletId { get; set; } = Guid.Empty;
    public string UserEmail { get; set; } = string.Empty;
}

public sealed class
    GiveWalletAccessCommandHandler : ICommandHandler<GiveWalletAccessCommand, GiveWalletAccessCommandResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _userService;

    public GiveWalletAccessCommandHandler(IApplicationDbContext dbContext,
                                          ICurrentUserService userService, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userService = userService;
        _userManager = userManager;
    }

    public async Task<GiveWalletAccessCommandResult> Handle(GiveWalletAccessCommand command, CancellationToken ct)
    {
        var wallet = await _dbContext.GetDbSet<Wallet>()
                                     .Include(x => x.UsersWithSharedAccess)
                                     .FirstOrDefaultAsync(x => x.Id == command.WalletId, ct);

        if (wallet is null)
            throw ValidationExceptionFactory.For(nameof(command.WalletId), "The given wallet doesn't exist");

        if (wallet.OwnerId != _userService.UserId)
            throw new AccessDeniedException();

        var normalizedEmail = _userManager.NormalizeEmail(command.UserEmail);
        var user = await _dbContext.GetDbSet<User>()
                                   .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, ct);

        if (user is null)
            return new GiveWalletAccessCommandResult();

        if (wallet.UsersWithSharedAccess.Any(x => x.Id == user.Id))
            throw ValidationExceptionFactory.For(nameof(command.UserEmail),
                "User with given email already has access to the given wallet.");

        wallet.UsersWithSharedAccess.Add(user);

        await _dbContext.SaveChangesAsync(ct);

        return new GiveWalletAccessCommandResult();
    }
}

public sealed record GiveWalletAccessCommandResult;