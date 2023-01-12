using Financity.Application.Common.Queries;
using Financity.Application.Wallets.Commands;
using Financity.Application.Wallets.Queries;
using Financity.Presentation.Controllers.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class WalletsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<WalletListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification<WalletListItem> querySpecification,
                                             CancellationToken ct
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
        var result = await HandleCommandAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletDetails))]
    public async Task<IActionResult> UpdateEntity(Guid id, UpdateWalletCommand command, CancellationToken ct)
    {
        command.Id = id;
        await HandleCommandAsync(command, ct);
        return await HandleQueryAsync(new GetWalletQuery(id), ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await HandleCommandAsync(new DeleteWalletCommand(id), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}/share")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserWithSharedAccessListItem))]
    public Task<IActionResult> GetWalletUsersWithSharedAccessList(
        [FromQuery] QuerySpecification<UserWithSharedAccessListItem> querySpecification, Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetWalletUsersWithSharedAccessQuery(id, querySpecification), ct);
    }

    [HttpPost("{id:guid}/share")]
    [SwaggerResponse(StatusCodes.Status204NoContent, Type = typeof(Unit))]
    public async Task<IActionResult> GiveAccess(GiveWalletAccessCommand command, Guid id, CancellationToken ct)
    {
        command.WalletId = id;
        await HandleCommandAsync(command, ct);
        return NoContent();
    }

    [HttpPut("{id:guid}/share")]
    [SwaggerResponse(StatusCodes.Status204NoContent, Type = typeof(Unit))]
    public async Task<IActionResult> RevokeAccess(RevokeWalletAccessCommand command, Guid id, CancellationToken ct)
    {
        command.WalletId = id;
        await HandleCommandAsync(command, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/resign")]
    [SwaggerResponse(StatusCodes.Status204NoContent, Type = typeof(Unit))]
    public async Task<IActionResult> ResignAccess(ResignWalletAccessCommand command, Guid id, CancellationToken ct)
    {
        command.WalletId = id;
        await HandleCommandAsync(command, ct);
        return NoContent();
    }

    [HttpGet("stats")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletsStats))]
    public Task<IActionResult> GetWalletsStats([FromQuery(Name = "transactionDate_gte")] DateTime? from,
                                               [FromQuery(Name = "transactionDate_lte")]
                                               DateTime? to,
                                               [FromQuery(Name = "walletId_in")] HashSet<Guid>? includeWalletsWithId,
                                               [FromQuery(Name = "currencyId_eq")] string currencyId,
                                               CancellationToken ct)
    {
        return HandleQueryAsync(new GetWalletsStatsQuery
        {
            CurrencyId = currencyId,
            From = DateOnly.FromDateTime(from ?? DateTime.UtcNow),
            To = DateOnly.FromDateTime(to ?? DateTime.UtcNow),
            WalletIds = includeWalletsWithId ?? new HashSet<Guid>()
        }, ct);
    }

    [HttpGet("{id:guid}/stats")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(WalletStats))]
    public Task<IActionResult> GetWalletStats([FromQuery(Name = "transactionDate_gte")] DateTime? from,
                                              [FromQuery(Name = "transactionDate_lte")]
                                              DateTime? to, Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetWalletStatsQuery
        {
            From = DateOnly.FromDateTime(from ?? DateTime.UtcNow),
            To = DateOnly.FromDateTime(to ?? DateTime.UtcNow),
            WalletId = id
        }, ct);
    }
}