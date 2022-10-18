using Financity.Application.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public class GetWalletsQuery : IRequest<IEnumerable<WalletDto>>
{
}

public sealed class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, IEnumerable<WalletDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WalletDto>> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Wallets.Select(x => new WalletDto()
        {
            Id = x.Id,
            Name = x.Name
        }).Take(20).ToListAsync(cancellationToken);
    }
}

public sealed class WalletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}