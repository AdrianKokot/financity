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
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct
    )
    {
        return HandleQueryAsync(new GetWalletsQuery(querySpecification), ct);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetWalletQuery(id), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateWalletCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateWalletCommand command, CancellationToken ct)
    {
        var result = await GetQueryResultAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new {id = result.Id}, result);
    }
}