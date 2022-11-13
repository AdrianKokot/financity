using Financity.Application.Users.Commands;
using Financity.Application.Users.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Financity.Presentation.Controllers;

public class AuthController : BaseController
{
    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(RegisterCommandResult))]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        return await HandleQueryAsync(command);
    }


    [HttpGet("user")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDetails))]
    public async Task<IActionResult> GetUser()
    {
        return await HandleQueryAsync(new GetUserQuery(User));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginCommandResult))]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        return await HandleQueryAsync(command);
    }
}