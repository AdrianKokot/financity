using Financity.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence;

public class ApplicationDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
{
    private readonly IDateTime _dateTime;

    public ApplicationDbContextFactory(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }
    protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
    {
        return new ApplicationDbContext(options, _dateTime);
    }
}