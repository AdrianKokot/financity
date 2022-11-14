using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Exceptions;
using Financity.Application.Common.Helpers;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Commands;

public sealed class GiveWalletAccessCommand : ICommand<GiveWalletAccessCommandResult>
{
    public Guid WalletId { get; set; } = Guid.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public WalletAccessLevel WalletAccessLevel { get; set; } = WalletAccessLevel.Shared;
}

public sealed class
    GiveWalletAccessCommandHandler : ICommandHandler<GiveWalletAccessCommand, GiveWalletAccessCommandResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public GiveWalletAccessCommandHandler(IApplicationDbContext dbContext, IMapper mapper,
                                          ICurrentUserService userService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<GiveWalletAccessCommandResult> Handle(GiveWalletAccessCommand command, CancellationToken ct)
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

        if (wallet.UsersWithAccess.Any(x => x.UserId == user.Id))
            throw ValidationExceptionFactory.For(nameof(command.UserEmail),
                "User with given email already has access to the given wallet.");

        if (wallet.UsersWithAccess.All(x => x.UserId != _userService.UserId))
            throw new AccessDeniedException();

        var walletAccess = new WalletAccess
        {
            UserId = user.Id,
            WalletId = wallet.Id,
            WalletAccessLevel = command.WalletAccessLevel
        };

        wallet.UsersWithAccess.Add(walletAccess);

        await _dbContext.SaveChangesAsync(ct);

        return _mapper.Map<GiveWalletAccessCommandResult>(walletAccess);
    }
}

public sealed record GiveWalletAccessCommandResult(Guid WalletId, Guid UserId) : IMapFrom<WalletAccess>;