using Financity.Application.Common.Queries;
using Financity.Application.Wallets.Commands;
using Financity.Application.Wallets.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class WalletsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletListItem>))]
    public Task<IActionResult> GetFilteredEntityListAsync(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
    {
        return HandleQueryAsync(new GetWalletsQuery(querySpecification), cancellationToken);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletDetails))]
    public Task<IActionResult> GetEntityAsync(Guid id, CancellationToken cancellationToken)
    {
        return HandleQueryAsync(new GetWalletQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateWalletCommandResult))]
    public Task<IActionResult> CreateEntityAsync(CreateWalletCommand command, CancellationToken cancellationToken)
    {
        return HandleQueryAsync(command, cancellationToken);
    }
}