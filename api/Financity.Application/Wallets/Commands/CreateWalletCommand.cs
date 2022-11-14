using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : ICommand<CreateWalletCommandResult>, IMapTo<Wallet>
{
    public string Name { get; set; } = string.Empty;
    public Guid CurrencyId { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}

public sealed class
    CreateWalletCommandHandler : CreateEntityCommandHandler<CreateWalletCommand, CreateWalletCommandResult, Wallet>
{
    private readonly ICurrentUserService _userService;

    public CreateWalletCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService userService)
        : base(dbContext, mapper)
    {
        _userService = userService;
    }

    public override async Task<CreateWalletCommandResult> Handle(CreateWalletCommand command,
                                                                 CancellationToken cancellationToken)
    {
        if (command.UserId == Guid.Empty) command.UserId = _userService.UserId;

        var entity = Mapper.Map<Wallet>(command);

        entity.UsersWithAccess ??= new List<WalletAccess>();
        entity.UsersWithAccess.Add(new WalletAccess
        {
            WalletId = entity.Id,
            WalletAccessLevel = WalletAccessLevel.Owner,
            UserId = command.UserId
        });

        DbContext.GetDbSet<Wallet>().Add(entity);

        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<CreateWalletCommandResult>(entity);
    }
}

public sealed record CreateWalletCommandResult(Guid Id) : IMapFrom<Wallet>;