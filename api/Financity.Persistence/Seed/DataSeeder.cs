using Financity.Application.Abstractions.Data;
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

    public static async Task<int> RequestExternalApiForCurrencies(IExchangeRateService service,
                                                                  IApplicationDbContext dbContext,
                                                                  CancellationToken ct = default)
    {
        var currenciesCount = await dbContext.GetDbSet<Currency>().CountAsync(ct);

        if (currenciesCount > 0) return currenciesCount;

        var currencies = (await service.GetCurrencies(ct)).ToList();

        await dbContext.GetDbSet<Currency>().AddRangeAsync(currencies, ct);

        await dbContext.SaveChangesAsync(ct);

        return currencies.Count;
    }
}