using Financity.Application.Users.Commands;
using Financity.Application.Users.Queries;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers;

public class AuthController : BaseController
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        return await HandleQueryAsync(command);
    }


    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        return await HandleQueryAsync(new GetUserQuery(User));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        return await HandleQueryAsync(command);
    }
}