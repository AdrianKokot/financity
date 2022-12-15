using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers;

sealed class ExchangeRate
{
    public decimal Result { get; set; }
}

[Route("api/exchange-rate")]
public class ExchangeRateController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetExchangeRate(CancellationToken ct, [FromQuery] string from = "EUR",
                                                     [FromQuery] string to = "EUR")
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.exchangerate.host");

        var response = await client.GetAsync($"convert?from={from}&to={to}", ct);

        if (!response.IsSuccessStatusCode)
            return BadRequest(await response.Content.ReadFromJsonAsync<object>(cancellationToken: ct));

        var exchangeRate = await response.Content.ReadFromJsonAsync<ExchangeRate>(cancellationToken: ct);
        return Ok(new { Rate = exchangeRate?.Result });
    }
}