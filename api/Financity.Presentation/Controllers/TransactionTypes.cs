using Financity.Application.TransactionTypes.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class TransactionTypes : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactionTypeListItem>))]
    public Task<IActionResult> GetEntityList(CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionTypesQuery(), ct);
    }
}