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
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetRecipientsQuery(querySpecification), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateRecipientCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateRecipientCommand command, CancellationToken ct)
    {
        var result = await HandleCommandAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RecipientDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetRecipientQuery(id), ct);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RecipientDetails))]
    public async Task<IActionResult> UpdateEntity(Guid id, UpdateRecipientCommand command, CancellationToken ct)
    {
        command.Id = id;
        await HandleCommandAsync(command, ct);
        return await HandleQueryAsync(new GetRecipientQuery(id), ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await HandleCommandAsync(new DeleteRecipientCommand(id), ct);
        return NoContent();
    }
}