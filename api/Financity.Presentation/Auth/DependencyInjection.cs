using Financity.Application.Abstractions.Data;
using Financity.Presentation.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Financity.Presentation.Auth;

public static class DependencyInjection
{
    public static AuthenticationBuilder AddAuthConfiguration(this AuthenticationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));
        return builder;
    }
}