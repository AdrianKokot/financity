using Financity.Application.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Currencies.Queries;

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
        return await _dbContext.Currencies.Select(c => new CurrencyDto {Code = c.Code, Id = c.Id})
            .ToListAsync(cancellationToken);
    }
}

public class CurrencyDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
}