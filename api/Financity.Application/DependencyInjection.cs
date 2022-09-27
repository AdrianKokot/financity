using System.Reflection;
using Financity.Application.Behaviors;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

        return services;
    }
}