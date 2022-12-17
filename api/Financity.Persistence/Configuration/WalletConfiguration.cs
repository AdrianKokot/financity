using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(x => x.Name)
               .HasMaxLength(64);

        builder.Property(x => x.StartingAmount)
               .HasDefaultValue(0);

        builder.HasIndex(nameof(Wallet.OwnerId), nameof(Wallet.Name)).IsUnique();
    }
}