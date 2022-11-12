using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence.Database;

public sealed class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
{
    protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
    {
        return new ApplicationDbContext(options, null!);
    }
}