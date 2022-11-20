using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Auth.Configuration;

public sealed class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtConfiguration _config;

    public ConfigureJwtBearerOptions(IOptions<JwtConfiguration> jwtOptions)
    {
        _config = jwtOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAlgorithms = new[] {_config.Algorithm},
            ValidateIssuer = _config.ValidateIssuer,
            ValidateAudience = _config.ValidateAudience,
            ValidateLifetime = _config.ValidateLifetime,
            ValidateIssuerSigningKey = _config.ValidateIssuerSigningKey,
            ValidIssuer = _config.ValidIssuer,
            ValidAudience = _config.ValidAudience,
            IssuerSigningKey = _config.IssuerSigningKey
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}