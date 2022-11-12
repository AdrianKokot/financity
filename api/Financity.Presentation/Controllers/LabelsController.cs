using Financity.Application.Common.Queries;
using Financity.Application.Labels.Commands;
using Financity.Application.Labels.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class LabelsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<LabelListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetLabelsQuery(querySpecification), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateLabelCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateLabelCommand command, CancellationToken ct)
    {
        var result = await GetQueryResultAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LabelDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetLabelQuery(id), ct);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntity(Guid id, UpdateLabelCommand command, CancellationToken ct)
    {
        command.Id = id;
        return HandleQueryAsync(command, ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await GetQueryResultAsync(new DeleteLabelCommand(id), ct);
        return NoContent();
    }
}