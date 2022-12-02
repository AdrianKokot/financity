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
                new Currency { Id = "PLN", Name = "Polski Złoty" },
                new Currency { Id = "EUR", Name = "Euro" },
                new Currency { Id = "USD", Name = "United States Dollar" }
            );
        });
    }
}