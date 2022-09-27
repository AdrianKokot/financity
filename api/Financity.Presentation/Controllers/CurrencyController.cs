using Financity.Application.Currency.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers;

[Controller]
[Route("api/currencies")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<CurrencyDto>> GetCurrencies()
    {
        return await _mediator.Send(new GetCurrenciesQuery());
    }
}