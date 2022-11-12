using System.Security.Claims;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Financity.Presentation.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Auth;

public sealed class TokenService : ITokenService
{
    private readonly IJwtConfiguration _configuration;

    public TokenService(IJwtConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetTokenForUser(User user)
    {
        var handler = new JsonWebTokenHandler();

        var identity = new ClaimsIdentity(GetUserClaims(user));

        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _configuration.ValidIssuer,
            Audience = _configuration.ValidAudience,
            Expires = AppDateTime.Now.AddHours(_configuration.ExpireAfterHours),
            Subject = identity,
            SigningCredentials = _configuration.Credentials
        });

        return token;
    }

    private IEnumerable<Claim> GetUserClaims(User user)
    {
        return new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
    }
}