using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Transactions.Queries;

public sealed record GetExchangeRateQuery(string To = "USD",
                                          string From = "USD") : IQuery<ExchangeRate>
{
    public DateTime Date { get; set; } = AppDateTime.Now;
}

public sealed class GetExchangeRateQueryHandler : IQueryHandler<GetExchangeRateQuery, ExchangeRate>
{
    private readonly IExchangeRateService _exchangeRateService;

    public GetExchangeRateQueryHandler(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<ExchangeRate> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var rate = await _exchangeRateService.GetExchangeRate(request.From, request.To,
            DateOnly.FromDateTime(request.Date.ToUniversalTime()),
            cancellationToken);
        return new ExchangeRate(request.From, request.To, rate);
    }
}

public sealed record ExchangeRate(string From, string To, decimal Rate);