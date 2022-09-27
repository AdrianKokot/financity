using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Account> Accounts { get; set; }
    DbSet<Domain.Entities.Currency> Currencies { get; set; }
    DbSet<Domain.Entities.Wallet> Wallets { get; set; }
    DbSet<Domain.Entities.Label> Labels { get; set; }
    DbSet<Domain.Entities.Recipient> Recipients { get; set; }
    DbSet<Domain.Entities.Transaction> Transactions { get; set; }
    DbSet<Domain.Entities.Category> Categories { get; set; }
}