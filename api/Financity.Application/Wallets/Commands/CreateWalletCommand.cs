using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Commands;

public sealed record CreateWalletCommand(string Name, Guid CurrencyId) : ICommand<CreateWalletCommandResult>;

public sealed class CreateWalletCommandHandler : ICommandHandler<CreateWalletCommand, CreateWalletCommandResult>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateWalletCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateWalletCommandResult> Handle(CreateWalletCommand request,
                                                        CancellationToken cancellationToken)
    {
        Wallet wallet = new()
        {
            Name = request.Name,
            CurrencyId = request.CurrencyId
        };

        _dbContext.Wallets.Add(wallet);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateWalletCommandResult(wallet.Id);
    }
}

public sealed record CreateWalletCommandResult(Guid Id);