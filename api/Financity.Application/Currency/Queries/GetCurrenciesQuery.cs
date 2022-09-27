using Financity.Application.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Currency.Queries;

public class GetCurrenciesQuery : IRequest<IEnumerable<CurrencyDto>>
{
}

public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IEnumerable<CurrencyDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetCurrenciesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Currencies.Select(c => new CurrencyDto(c.Code)).ToListAsync(cancellationToken);
    }
}

public class CurrencyDto
{
    public string Code { get; set; }

    public CurrencyDto(string code)
    {
        Code = code;
    }
}