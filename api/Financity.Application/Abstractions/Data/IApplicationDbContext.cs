using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Currency> Currencies { get; set; }
    DbSet<Wallet> Wallets { get; set; }
    DbSet<Label> Labels { get; set; }
    DbSet<Recipient> Recipients { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<Category> Categories { get; set; }
    public DbSet<T> GetDbSet<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}