using Financity.Application.Categories.Commands;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class CategoriesController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryListItem>))]
    public Task<IActionResult> GetEntityList([FromQuery] QuerySpecification querySpecification, CancellationToken ct)
    {
        return HandleQueryAsync(new GetCategoriesQuery(querySpecification), ct);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreateCategoryCommandResult))]
    public async Task<IActionResult> CreateEntity(CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await GetQueryResultAsync(command, ct);
        return CreatedAtAction(nameof(GetEntity), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CategoryDetails))]
    public Task<IActionResult> GetEntity(Guid id, CancellationToken ct)
    {
        return HandleQueryAsync(new GetCategoryQuery(id), ct);
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntity(Guid id, UpdateCategoryCommand command, CancellationToken ct)
    {
        command.Id = id;
        return HandleQueryAsync(command, ct);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEntity(Guid id, CancellationToken ct)
    {
        await GetQueryResultAsync(new DeleteCategoryCommand(id), ct);
        return NoContent();
    }
}