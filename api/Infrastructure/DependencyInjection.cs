using Financity.Application.Common.Interfaces;
using Financity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Financity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}