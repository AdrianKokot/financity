using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletQuery : IQuery<WalletDetails>
{
    public GetWalletQuery(string id)
    {
        Id = Guid.Parse(id);
    }

    public Guid Id { get; set; }
}

public sealed class GetWalletQueryHandler : IQueryHandler<GetWalletQuery, WalletDetails>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletDetails> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _dbContext.Wallets.FirstAsync(x => x.Id.Equals(request.Id), cancellationToken);

        return new WalletDetails
        {
            Id = wallet.Id,
            Name = wallet.Name
        };
    }
}

public sealed class WalletDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}