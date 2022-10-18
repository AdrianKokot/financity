using Financity.Application.Accounts.Queries;
using Financity.Domain.Entities;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class AccountsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountDto>))]
    public async Task<IActionResult> GetAccounts()
        => await HandleQuery(new GetAccountsQuery());
}