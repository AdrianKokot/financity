using Financity.Application.Common.FilteredQuery;
using Financity.Application.Wallets.Commands;
using Financity.Application.Wallets.Queries;
using Financity.Domain.Entities;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class WalletsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletListItem>))]
    public async Task<IActionResult> GetFilteredEntityListAsync(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
    {
        return await HandleQuery(new GetWalletsQuery(querySpecification), cancellationToken);
    }

    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Wallet))]
    public async Task<IActionResult> GetEntityAsync(string id)
    {
        return await HandleQuery(new GetWalletQuery(id));
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateWalletCommandResult))]
    public async Task<IActionResult> CreateWallet(CreateWalletCommand command)
    {
        return await HandleQuery(command);
    }
}