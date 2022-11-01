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
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification)
        => HandleQueryAsync(new GetLabelsQuery(querySpecification));

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateLabelCommandResult))]
    public Task<IActionResult> CreateEntity(CreateLabelCommand command)
        => HandleQueryAsync(command);

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LabelDetails))]
    public Task<IActionResult> GetEntity(Guid id)
        => HandleQueryAsync(new GetLabelQuery(id));

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntity(Guid id, UpdateLabelCommand command)
    {
        command.Id = id;
        return HandleQueryAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> DeleteEntity(Guid id)
        => HandleQueryAsync(new DeleteLabelCommand(id));
}