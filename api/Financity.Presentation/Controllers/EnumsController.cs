using Financity.Application.Enums.Queries;
using Financity.Application.Enums.Queries.Abstract;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class EnumsController : BaseController
{
    [HttpGet("transaction-type")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumListItem>))]
    public Task<IActionResult> GetTransactionTypeEnum(CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionTypeQuery(), ct);
    }

    [HttpGet("wallet-access-level")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumListItem>))]
    public Task<IActionResult> GetWalletAccessLevel(CancellationToken ct)
    {
        return HandleQueryAsync(new GetWalletAccessLevelQuery(), ct);
    }
}