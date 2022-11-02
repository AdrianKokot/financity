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
    public Task<IActionResult> GetFilteredEntityListAsync(
        [FromQuery] QuerySpecification specification,
        CancellationToken cancellationToken
    )
    {
        return HandleQueryAsync(new GetTransactionsQuery(specification), cancellationToken);
    }
}