using Financity.Application.Budgets.Commands;
using Financity.Application.Budgets.Queries;
using Financity.Application.Common.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class BudgetsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<BudgetListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetBudgetsQuery(querySpecification), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateBudgetCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateBudgetCommand command, CancellationToken ct)
    {
        var result = await GetQueryResultAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(BudgetDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetBudgetQuery(id), ct);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntity(Guid id, UpdateBudgetCommand command, CancellationToken ct)
    {
        command.Id = id;
        return HandleQueryAsync(command, ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await GetQueryResultAsync(new DeleteBudgetCommand(id), ct);
        return NoContent();
    }
}