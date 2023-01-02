using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Commands;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : ICommand<CreateWalletCommandResult>, IMapTo<Wallet>
{
    public string Name { get; set; } = string.Empty;
    public string CurrencyId { get; set; } = string.Empty;
    public decimal StartingAmount { get; set; } = 0;
    public Guid UserId { get; set; } = Guid.Empty;

    public static void CreateMap(Profile profile)
    {
        profile.CreateMap<CreateWalletCommand, Wallet>()
               .ForMember(x => x.OwnerId, x => x.MapFrom(y => y.UserId));
    }
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

        DbContext.GetDbSet<Wallet>().Add(entity);

        await SaveChanges(cancellationToken);

        await DbContext.GenerateDefaultCategories(entity.Id, cancellationToken);

        return Mapper.Map<CreateWalletCommandResult>(entity);
    }
}

public sealed record CreateWalletCommandResult(Guid Id) : IMapFrom<Wallet>;