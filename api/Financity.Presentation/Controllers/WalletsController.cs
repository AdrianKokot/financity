using Financity.Application.Common.FilteredQuery;
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
    public Task<IActionResult> GetFilteredEntityList(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
        => HandleQueryAsync(new GetWalletsQuery(querySpecification), cancellationToken);


    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken cancellationToken)
        => HandleQueryAsync(new GetWalletQuery(id), cancellationToken);


    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateWalletCommandResult))]
    public Task<IActionResult> CreateEntity(CreateWalletCommand command, CancellationToken cancellationToken)
        => HandleQueryAsync(command, cancellationToken);
}