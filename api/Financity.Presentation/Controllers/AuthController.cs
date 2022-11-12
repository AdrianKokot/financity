using Financity.Domain.Entities;
using Financity.Presentation.Abstractions;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Financity.Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly UserManager<User> _manager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<User> manager,
                          ITokenService tokenService)
    {
        _tokenService = tokenService;
        _manager = manager;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string userName, string password)
    {
        var user = new User
        {
            Email = userName,
            UserName = userName
        };

        var result = await _manager.CreateAsync(user, password);

        return Ok(result.Succeeded);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var user = await _manager.GetUserAsync(User);
        return Ok(new
        {
            user.Id
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _manager.FindByEmailAsync(userName);

        if (user == null || !await _manager.CheckPasswordAsync(user, password)) return Unauthorized();
        var token = _tokenService.GetTokenForUser(user);

        return Ok(new
        {
            token
        });
    }
}