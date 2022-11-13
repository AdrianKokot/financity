using Financity.Application.Abstractions.Data;
using Financity.Presentation.Services;
using Microsoft.AspNetCore.Authentication;

namespace Financity.Presentation.Auth;

public static class DependencyInjection
{
    public static AuthenticationBuilder AddAuthConfiguration(this AuthenticationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        return builder;
    }
}