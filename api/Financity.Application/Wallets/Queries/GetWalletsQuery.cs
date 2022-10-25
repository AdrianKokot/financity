using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Wallets.Queries;

public class GetWalletsQuery : IQuery<IEnumerable<WalletListItem>>
{
}

public sealed class GetWalletsQueryHandler : IQueryHandler<GetWalletsQuery, IEnumerable<WalletListItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetWalletsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WalletListItem>> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Wallets.Include(x => x.DefaultCurrency).Select(x => new WalletListItem
        {
            Id = x.Id,
            Name = x.Name,
            CurrencyId = x.DefaultCurrency.Id,
            CurrencyCode = x.DefaultCurrency.Code,
            CurrencyName = x.DefaultCurrency.Name,
            Balance = 1000
        }).Take(20).ToListAsync(cancellationToken);
    }
}

public sealed class WalletListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CurrencyId { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Balance { get; set; }
}