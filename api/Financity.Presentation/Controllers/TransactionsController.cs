using Financity.Application.Common.FilteredQuery;
using Financity.Application.Transactions.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class TransactionsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactionListItem>))]
    public Task<IActionResult> GetFilteredEntityList(
        [FromQuery] QuerySpecification specification,
        CancellationToken cancellationToken
    )
        => HandleQueryAsync(new GetTransactionsQuery(specification), cancellationToken);
}