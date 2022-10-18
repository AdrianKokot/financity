using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<Currency> Currencies { get; set; }
    DbSet<Wallet> Wallets { get; set; }
    DbSet<Label> Labels { get; set; }
    DbSet<Recipient> Recipients { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<Category> Categories { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}