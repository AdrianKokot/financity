using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

    protected async Task<IActionResult> HandleQueryAsync<TResponse>(IRequest<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(query, cancellationToken);

        if (result == null || result.GetType().IsInstanceOfType(Unit.Value)) return NoContent();

        return Ok(result);
    }
}