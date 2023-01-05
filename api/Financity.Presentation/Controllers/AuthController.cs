using Financity.Application.Auth.Commands;
using Financity.Application.Auth.Queries;
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

    [HttpPost("request-reset-password")]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RequestResetPassword(RequestResetPasswordCommand command)
    {
        await HandleCommandAsync(command);
        return Accepted();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        await HandleCommandAsync(command);
        return NoContent();
    }

    [HttpPost("change-password")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        await HandleCommandAsync(command);
        return NoContent();
    }

    [HttpPut("user")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserDetails))]
    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
    {
        var result = await HandleCommandAsync(command);
        return Ok(result);
    }
}