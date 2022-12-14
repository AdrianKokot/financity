using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        var appearanceBuilder = builder.OwnsOne(x => x.Appearance);

        appearanceBuilder.Property(x => x.Color).HasMaxLength(64);
        appearanceBuilder.Property(x => x.IconName).HasMaxLength(64);

        builder.Property(x => x.Name).HasMaxLength(64);

        builder.HasIndex(nameof(Label.WalletId), nameof(Label.Name)).IsUnique();
    }
}