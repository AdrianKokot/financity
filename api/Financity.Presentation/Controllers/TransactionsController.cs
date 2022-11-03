using Financity.Application.Common.Queries;
using Financity.Application.Transactions.Commands;
using Financity.Application.Transactions.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class TransactionsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TransactionListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionsQuery(querySpecification), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateTransactionCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateTransactionCommand command, CancellationToken ct)
    {
        var result = await GetQueryResultAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TransactionDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetTransactionQuery(id), ct);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntity(Guid id, UpdateTransactionCommand command, CancellationToken ct)
    {
        command.Id = id;
        return HandleQueryAsync(command, ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await GetQueryResultAsync(new DeleteTransactionCommand(id), ct);
        return NoContent();
    }
}