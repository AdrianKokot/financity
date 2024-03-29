﻿using Financity.Application.Common.Queries;
using Financity.Application.Currencies.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public sealed class CurrenciesController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification<CurrencyListItem> querySpecification,
                                             CancellationToken ct)
    {
        return HandleQueryAsync(new GetCurrenciesQuery(querySpecification), ct);
    }

    [HttpGet("used-by-user")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CurrencyListItem>))]
    public Task<IActionResult> GetCurrenciesUsedByUserList(
        [FromQuery] QuerySpecification<CurrencyListItem> querySpecification,
        CancellationToken ct)
    {
        return HandleQueryAsync(new GetCurrenciesUsedByUserQuery(querySpecification), ct);
    }
}