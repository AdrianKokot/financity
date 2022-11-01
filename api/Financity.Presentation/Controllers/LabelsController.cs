using Financity.Application.Common.FilteredQuery;
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
    public async Task<IActionResult> GetEntityListAsync(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
    {
        return await HandleQueryAsync(new GetLabelsQuery(querySpecification), cancellationToken);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateLabelCommandResult))]
    public async Task<IActionResult> CreateEntityAsync(CreateLabelCommand command)
    {
        return await HandleQueryAsync(command);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEntityAsync(Guid id, UpdateLabelCommand command)
    {
        command.Id = id;
        return await HandleQueryAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteEntityAsync(Guid id)
    {
        return await HandleQueryAsync(new DeleteLabelCommand(id));
    }
}