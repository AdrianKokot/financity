namespace Financity.Application.Abstractions.Data;

public interface IExchangeRateService
{
    public Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency,
                                         CancellationToken cancellationToken = default);
}