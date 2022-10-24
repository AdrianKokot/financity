using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Currencies.Queries;

public class GetCurrenciesQuery : IQuery<IEnumerable<CurrencyListItem>>
{
}

public class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IEnumerable<CurrencyListItem>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetCurrenciesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CurrencyListItem>> Handle(GetCurrenciesQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Currencies.Select(c => new CurrencyListItem {Code = c.Code, Id = c.Id, Name = c.Name})
            .ToListAsync(cancellationToken);
    }
}

public class CurrencyListItem
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}