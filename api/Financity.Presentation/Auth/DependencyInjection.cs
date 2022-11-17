using Financity.Application.Abstractions.Data;
using Financity.Presentation.Configuration;
using Financity.Presentation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Financity.Presentation.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection builder, IJwtConfiguration jwtConfig)
    {
        builder.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                   options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidAlgorithms = new[] { jwtConfig.Algorithm },
                           ValidateIssuer = jwtConfig.ValidateIssuer,
                           ValidateAudience = jwtConfig.ValidateAudience,
                           ValidateLifetime = jwtConfig.ValidateLifetime,
                           ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                           ValidIssuer = jwtConfig.ValidIssuer,
                           ValidAudience = jwtConfig.ValidAudience,
                           IssuerSigningKey = jwtConfig.IssuerSigningKey
                       };
                   });

        builder.AddScoped<ITokenService, TokenService>();
        builder.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));

        builder.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.Api, policy => { policy.RequireAuthenticatedUser(); });
        });

        return builder;
    }
}