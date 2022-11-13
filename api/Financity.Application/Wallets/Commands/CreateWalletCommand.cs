using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : ICommand<CreateWalletCommandResult>, IMapTo<Wallet>
{
    public string? Name { get; set; }
    public Guid CurrencyId { get; set; }
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

    public override Task<CreateWalletCommandResult> Handle(CreateWalletCommand command,
                                                           CancellationToken cancellationToken)
    {
        if (command.UserId == Guid.Empty) command.UserId = _userService.UserId;

        return base.Handle(command, cancellationToken);
    }
}

public sealed record CreateWalletCommandResult(Guid Id) : IMapFrom<Wallet>;