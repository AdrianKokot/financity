using Financity.Application.Accounts.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class AccountsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountListItem>))]
    public async Task<IActionResult> GetAccounts()
    {
        return await HandleQuery(new GetAccountsQuery());
    }
}