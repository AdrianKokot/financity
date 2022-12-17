using System.Net.Http.Json;
using Financity.Application.Abstractions.Data;

namespace Financity.Infrastructure.Services;

sealed record ExchangeRateApiResponse(decimal Result);

public sealed class ExchangeRateApiException : Exception
{
    public ExchangeRateApiException(string message) : base(message)
    {
    }
}

public sealed class ExchangeRateService : IExchangeRateService
{
    public async Task<decimal> GetExchangeRate(string from, string to,
                                               CancellationToken ct = default)
    {
        using var client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.exchangerate.host")
        };

        var response = await client.GetAsync($"convert?from={from}&to={to}", ct);

        if (!response.IsSuccessStatusCode)
            throw new ExchangeRateApiException((await response.Content.ReadFromJsonAsync<object>(cancellationToken: ct))
                ?.ToString());

        var exchangeRate = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>(cancellationToken: ct);

        if (exchangeRate is null)
            throw new ExchangeRateApiException("Response from exchangerate api was null.");

        return exchangeRate.Result;
    }
}