using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Financity.Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Financity.Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _manager;

    public AuthController(UserManager<User> manager, SignInManager<User> signInManager,
                          IConfiguration configuration)
    {
        _signInManager = signInManager;
        _configuration = configuration;
        _manager = manager;
        // use that:
        // https://kags.me.ke/post/asp-net-core-identity-authentication/   
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string userName, string password)
    {
        var user = new User()
        {
            Email = userName,
            UserName = userName
        };

        var result = await _manager.CreateAsync(user, password);

        return Ok(result.Succeeded);
        if (result.Succeeded)
        {
            
        }
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var user = await _manager.GetUserAsync(User);
        return Ok(new
        {
            Id = user.Id
        });
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await _manager.FindByEmailAsync(userName);

        if (user == null || !await _manager.CheckPasswordAsync(user, password)) return Unauthorized();
        var token = GetToken(new List<Claim>()
        {
            // new Claim(JwtRegisteredClaimNames.Sub)
            // new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            // new Claim(JwtRegisteredClaimNames.Email, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        });

        return Ok(new
        {
            token = token
        });

        var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);

        return Ok(result.Succeeded);
        // await _manager.
        // _manager.CheckPasswordAsync()
        // await HttpContext.SignInAsync(new ClaimsPrincipal(new[] {new ClaimsIdentity()}));
        return Ok();
    }
    
    private string GetToken(List<Claim> authClaims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:TokenValidationParameters:IssuerSigningKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = _configuration["JwtSettings:TokenValidationParameters:ValidIssuer"],
            Audience = _configuration["JwtSettings:TokenValidationParameters:ValidAudience"],
            Expires = AppDateTime.Now.AddHours(3),
            Subject = new ClaimsIdentity(authClaims),
            SigningCredentials = credentials,
        });

        return token;

    }
}