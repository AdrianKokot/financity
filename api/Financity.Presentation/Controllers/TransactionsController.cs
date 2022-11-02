using Financity.Application.Common.Queries;
using Financity.Application.Transactions.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class TransactionsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactionListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification specification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionsQuery(specification), ct);
    }
}