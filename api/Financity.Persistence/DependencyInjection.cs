using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Financity.Persistence.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Financity"))
        );

        services.AddIdentity<User, IdentityRole<Guid>>(options => { options.SignIn.RequireConfirmedAccount = false; })
                .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}