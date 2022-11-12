using Financity.Presentation.Abstractions;
using Microsoft.AspNetCore.Authentication;

namespace Financity.Presentation.Auth;

public static class DependencyInjection
{
    public static AuthenticationBuilder AddTokenConfiguration(this AuthenticationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        return builder;
    }
}