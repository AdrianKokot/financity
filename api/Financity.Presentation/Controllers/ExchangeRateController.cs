using Financity.Application.Transactions.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

[Route("api/exchange-rate")]
public class ExchangeRateController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ExchangeRate))]
    public Task<IActionResult> GetExchangeRate([FromQuery] GetExchangeRateQuery query, CancellationToken ct)
    {
        return HandleQueryAsync(query, ct);
    }
}