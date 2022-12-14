using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(64);

        builder.HasIndex(nameof(Budget.UserId), nameof(Budget.Name)).IsUnique();
    }
}