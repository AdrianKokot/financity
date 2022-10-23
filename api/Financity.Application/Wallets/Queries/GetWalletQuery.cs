using Financity.Application.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public sealed class GetWalletQuery : IRequest<WalletDetails>
{
    public Guid Id { get; set; }

    public GetWalletQuery(string id)
    {
        Id = Guid.Parse(id);
    }
}

public sealed class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, WalletDetails>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<WalletDetails> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _dbContext.Wallets.FirstAsync(x => x.Id.Equals(request.Id), cancellationToken);

        return new WalletDetails()
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