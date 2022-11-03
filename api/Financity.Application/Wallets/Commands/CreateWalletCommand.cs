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
}

public sealed class
    CreateWalletCommandHandler : CreateEntityCommandHandler<CreateWalletCommand, CreateWalletCommandResult, Wallet>
{
    public CreateWalletCommandHandler(IApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

public sealed record CreateWalletCommandResult(Guid Id) : IMapFrom<Wallet>;