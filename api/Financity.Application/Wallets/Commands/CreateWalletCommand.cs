using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : ICommand<CreateWalletCommandResult>
{
    public string Name { get; set; }
    public string CurrencyId { get; set; }
}

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
            Name = request.Name
        };

        wallet.CurrencyId = Guid.Parse(request.CurrencyId);

        _dbContext.Wallets.Add(wallet);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateWalletCommandResult
        {
            Id = wallet.Id
        };
    }
}

public sealed class CreateWalletCommandResult
{
    public Guid Id { get; set; }
}