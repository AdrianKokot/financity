﻿using System.Collections.Immutable;
using System.Security.Claims;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Configuration;
using Financity.Domain.Entities;
using Financity.Presentation.Auth.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Services;

public sealed class TokenService : ITokenService
{
    private readonly JwtConfiguration _configuration;
    private readonly ClaimsIdentityOptions _options;

    public TokenService(IOptions<JwtConfiguration> jwtOptions, IOptions<IdentityOptions> identityOptions)
    {
        _configuration = jwtOptions.Value;
        _options = identityOptions.Value.ClaimsIdentity;
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
        if (user.Email is null || user.UserName is null) return ImmutableArray<Claim>.Empty;

        return new Claim[]
        {
            new(_options.UserIdClaimType, user.Id.ToString()),
            new(_options.EmailClaimType, user.Email),
            new(_options.UserNameClaimType, user.UserName)
        };
    }
}