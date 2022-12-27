using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var appearanceBuilder = builder.OwnsOne(x => x.Appearance);

        appearanceBuilder.Property(x => x.Color).HasMaxLength(64);
        appearanceBuilder.Property(x => x.IconName).HasMaxLength(64);

        builder.Property(x => x.Name).HasMaxLength(64);

        builder.Property(x => x.TransactionType)
               .HasConversion<string>()
               .HasMaxLength(7)
               .HasColumnName($"{nameof(Category.TransactionType)}Id");

        builder.HasIndex(x => x.TransactionType);

        builder.HasIndex(nameof(Category.WalletId), nameof(Category.Name)).IsUnique();
    }
}