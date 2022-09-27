using Financity.Application.Currency.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers;

public sealed class CurrenciesController : BaseController
{
    [HttpGet]
    public async Task<IEnumerable<CurrencyDto>> GetCurrencies()
    {
        return await Mediator.Send(new GetCurrenciesQuery());
    }
}