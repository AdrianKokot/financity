using Financity.Application.Common.FilteredQuery;
using Financity.Application.Currencies.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public sealed class CurrenciesController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyListItem>))]
    public Task<IActionResult> GetFilteredEntityList(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
        => HandleQueryAsync(new GetCurrenciesQuery(querySpecification), cancellationToken);
}