using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence.Seed;

public static class DataSeeder
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedCurrencies();
    }

    private static void SeedCurrencies(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(c =>
        {
            c.HasData(
                new Currency { Code = "PLN", Name = "Polski Złoty" },
                new Currency { Code = "EUR", Name = "Euro" },
                new Currency { Code = "USD", Name = "United States Dollar" }
            );
        });
    }
}