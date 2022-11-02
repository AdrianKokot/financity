using Financity.Application.Categories.Commands;
using Financity.Application.Categories.Queries;
using Financity.Application.Common.FilteredQuery;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class CategoriesController : BaseController
{
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryListItem>))]
    public Task<IActionResult> GetEntityListAsync([FromQuery] QuerySpecification querySpecification)
    {
        return HandleQueryAsync(new GetCategoriesQuery(querySpecification));
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateCategoryCommandResult))]
    public Task<IActionResult> CreateEntityAsync(CreateCategoryCommand command)
    {
        return HandleQueryAsync(command);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CategoryDetails))]
    public Task<IActionResult> GetEntityAsync(Guid id)
    {
        return HandleQueryAsync(new GetCategoryQuery(id));
    }

    [HttpPut("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateEntityAsync(Guid id, UpdateCategoryCommand command)
    {
        command.Id = id;
        return HandleQueryAsync(command);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public Task<IActionResult> DeleteEntityAsync(Guid id)
    {
        return HandleQueryAsync(new DeleteCategoryCommand(id));
    }
}