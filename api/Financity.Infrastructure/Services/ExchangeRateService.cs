using System.Net.Http.Json;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Financity.Infrastructure.Services;

internal sealed record ExchangeRateApiResponse(decimal Result);

internal sealed record ExchangeRateApiSymbolDTO(string Description, string Code);

internal sealed record ExchangeRateApiSymbolsResponse(IDictionary<string, ExchangeRateApiSymbolDTO> Symbols);

public sealed class ExchangeRateApiException : Exception
{
    public ExchangeRateApiException(string message) : base(message)
    {
    }
}

public sealed class ExchangeRateService : IExchangeRateService
{
    private readonly Uri _baseAddress = new("https://api.exchangerate.host");
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(ILogger<ExchangeRateService> logger)
    {
        _logger = logger;
    }

    public async Task<decimal> GetExchangeRate(string from, string to,
                                               CancellationToken ct = default)
    {
        return await GetExchangeRate(from, to, new DateOnly(), ct);
    }

    public async Task<decimal> GetExchangeRate(string from, string to, DateOnly date,
                                               CancellationToken ct = default)
    {
        using var client = new HttpClient
        {
            BaseAddress = _baseAddress
        };

        var requestUri = $"convert?from={from}&to={to}&date={date.ToString("yyyy-MM-dd")}";

        var response = await client.GetAsync(requestUri, ct);

        _logger.LogInformation("Proxy request to: {Name}", _baseAddress + requestUri);

        if (!response.IsSuccessStatusCode)
            throw new ExchangeRateApiException((await response.Content.ReadFromJsonAsync<object>(cancellationToken: ct))
                ?.ToString() ?? string.Empty);

        var exchangeRate = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>(cancellationToken: ct);

        if (exchangeRate is null)
            throw new ExchangeRateApiException("Response from exchangerate api was null.");

        return exchangeRate.Result;
    }

    public async Task<IEnumerable<Currency>> GetCurrencies(CancellationToken ct = default)
    {
        using var client = new HttpClient
        {
            BaseAddress = _baseAddress
        };

        var requestUri = "symbols";

        var response = await client.GetAsync(requestUri, ct);

        _logger.LogInformation("Call {Name}", _baseAddress + requestUri);

        if (!response.IsSuccessStatusCode)
            throw new ExchangeRateApiException((await response.Content.ReadFromJsonAsync<object>(cancellationToken: ct))
                ?.ToString() ?? string.Empty);

        var exchangeRate =
            await response.Content.ReadFromJsonAsync<ExchangeRateApiSymbolsResponse>(cancellationToken: ct);

        if (exchangeRate is null)
            throw new ExchangeRateApiException("Response from exchangerate api was null.");

        return exchangeRate.Symbols.Values.Select(x => new Currency { Id = x.Code, Name = x.Description });
    }
}