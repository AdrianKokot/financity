using Financity.Application.Wallets.Commands;
using Financity.Application.Wallets.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class WalletsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletDto>))]
    public async Task<IActionResult> GetWallets()
        => await HandleQuery(new GetWalletsQuery());

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateWalletCommandResult))]
    public async Task<IActionResult> CreateWallet(CreateWalletCommand command)
        => await HandleQuery(command);
}