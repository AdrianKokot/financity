using Financity.Application.Common.FilteredQuery;
using Financity.Application.Transactions.Queries;
using Financity.Domain.Entities;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class TransactionsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Transaction>))]
    public async Task<IActionResult> GetFilteredEntityListAsync(
        [FromQuery] QuerySpecification specification,
        [FromQuery] string? walletId,
        CancellationToken cancellationToken
    )
    {
        return await HandleQuery(new GetTransactionsQuery(specification, walletId), cancellationToken);
    }
}