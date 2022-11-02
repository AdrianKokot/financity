using Financity.Application.Common.Queries;
using Financity.Application.Recipients.Commands;
using Financity.Application.Recipients.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class RecipientsController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<RecipientListItem>))]
    public Task<IActionResult> GetEntityListAsync([FromQuery] QuerySpecification querySpecification)
    {
        return HandleQueryAsync(new GetRecipientsQuery(querySpecification));
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateRecipientCommandResult))]
    public async Task<IActionResult> CreateEntityAsync(CreateRecipientCommand command)
    {
        var result = await GetQueryResultAsync(command);
        return CreatedAtAction(nameof(GetEntityAsync), new {id = result.Id}, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RecipientDetails))]
    public Task<IActionResult> GetEntityAsync(Guid id)
    {
        return HandleQueryAsync(new GetRecipientQuery(id));
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntityAsync(Guid id, UpdateRecipientCommand command)
    {
        command.Id = id;
        return HandleQueryAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntityAsync(Guid id)
    {
        await GetQueryResultAsync(new DeleteRecipientCommand(id));
        return NoContent();
    }
}