﻿using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence.Seed;

public static class DataSeeder
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedCurrencies();
        modelBuilder.SeedAccounts();
    }

    private static void SeedCurrencies(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(c => { c.HasData(new Currency {Code = "PLN", Name = "Polski Złoty"}); }
        );
    }

    private static void SeedAccounts(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(c =>
            {
                c.HasData(new Account());
            }
        );
    }
}