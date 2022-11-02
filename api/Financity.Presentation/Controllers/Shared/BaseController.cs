using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

    protected async Task<IActionResult> HandleQueryAsync<TResponse>(IRequest<TResponse> query, CancellationToken ct = default)
    {
        return Ok(await GetQueryResultAsync(query, ct));
    }

    protected Task<TResponse> GetQueryResultAsync<TResponse>(IRequest<TResponse> query, CancellationToken ct = default)
    {
        return Mediator.Send(query, ct);
    } 
}