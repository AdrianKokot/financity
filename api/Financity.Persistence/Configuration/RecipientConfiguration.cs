using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(64);

        builder.HasIndex(nameof(Recipient.WalletId), nameof(Recipient.Name)).IsUnique();
    }
}