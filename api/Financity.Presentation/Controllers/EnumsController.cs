using Financity.Application.Enums.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class EnumsController : BaseController
{
    [HttpGet("transaction-type")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    public Task<IActionResult> GetTransactionTypeEnum(CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionTypeQuery(), ct);
    }
}