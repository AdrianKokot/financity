using System.Net.Http.Json;
using Financity.Application.Abstractions.Data;
using Microsoft.Extensions.Logging;

namespace Financity.Infrastructure.Services;

sealed record ExchangeRateApiResponse(decimal Result);

public sealed class ExchangeRateService : IExchangeRateService
{
    public async Task<decimal> GetExchangeRate(string from, string to,
                                               CancellationToken ct = default)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.exchangerate.host");

        var response = await client.GetAsync($"convert?from={from}&to={to}", ct);

        if (!response.IsSuccessStatusCode)
            throw new Exception((await response.Content.ReadFromJsonAsync<object>(cancellationToken: ct))?.ToString());

        var exchangeRate = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>(cancellationToken: ct);

        if (exchangeRate is null)
            throw new Exception("Response from exchangerate api was null.");

        return exchangeRate.Result;
    }
}