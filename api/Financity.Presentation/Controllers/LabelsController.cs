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
    public Task<IActionResult> GetEntityListAsync([FromQuery] QuerySpecification querySpecification)
    {
        return HandleQueryAsync(new GetLabelsQuery(querySpecification));
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateLabelCommandResult))]
    public Task<IActionResult> CreateEntityAsync(CreateLabelCommand command)
    {
        return HandleQueryAsync(command);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LabelDetails))]
    public Task<IActionResult> GetEntityAsync(Guid id)
    {
        return HandleQueryAsync(new GetLabelQuery(id));
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntityAsync(Guid id, UpdateLabelCommand command)
    {
        command.Id = id;
        return HandleQueryAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> DeleteEntityAsync(Guid id)
    {
        return HandleQueryAsync(new DeleteLabelCommand(id));
    }
}