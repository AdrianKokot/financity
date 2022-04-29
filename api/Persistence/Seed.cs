using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence;

public static class Seed
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedCurrencies();
    }

    private static void SeedCurrencies(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(c =>
            {
                c.HasData(new Currency() {Id = 1, Code = "PLN", CreatedAt = DateTime.Now, CreatedBy = "System"});
            }
        );
    }
}