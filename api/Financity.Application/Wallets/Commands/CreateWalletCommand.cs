using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : ICommand<CreateWalletCommandResult>, IMapTo<Wallet>
{
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
}

public sealed class CreateWalletCommandHandler : ICommandHandler<CreateWalletCommand, CreateWalletCommandResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateWalletCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CreateWalletCommandResult> Handle(CreateWalletCommand request,
                                                        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Wallet>(request);

        _dbContext.Wallets.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateWalletCommandResult(entity.Id);
    }
}

public sealed record CreateWalletCommandResult(Guid Id);