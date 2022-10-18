using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Financity.Application.Wallets.Commands;

public sealed class CreateWalletCommand : IRequest<CreateWalletCommandResult>
{
    public string Name { get; set; }
    public string AccountId { get; set; }
    public string CurrencyId { get; set; }
}

public sealed class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletCommandResult>
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

        wallet.AccountId = Guid.Parse(request.AccountId);
        wallet.DefaultCurrencyId = Guid.Parse(request.CurrencyId);

        _dbContext.Wallets.Add(wallet);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new()
        {
            Id = wallet.Id
        };
    }
}

public sealed class CreateWalletCommandResult
{
    public Guid Id { get; set; }
}

public sealed class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(64);
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}