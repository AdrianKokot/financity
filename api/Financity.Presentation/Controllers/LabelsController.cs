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
    public async Task<IActionResult> GetFilteredEntityListAsync(
        [FromQuery] QuerySpecification querySpecification,
        CancellationToken cancellationToken
    )
    {
        return await HandleQuery(new GetLabelsQuery(querySpecification), cancellationToken);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateLabelCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateLabelCommand command)
    {
        return await HandleQuery(command);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateEntity(Guid id, UpdateLabelCommand command)
    {
        command.Id = id;
        return await HandleQuery(command);
    }
}