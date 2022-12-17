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

        builder.HasMany(x => x.TrackedCategories)
               .WithMany(x => x.Budgets)
               .UsingEntity<Dictionary<string, object>>(
                   $"{nameof(Budget)}{nameof(Category)}",
                   x => x.HasOne<Category>()
                         .WithMany()
                         .HasForeignKey($"{nameof(Category)}{nameof(Category.Id)}")
                         .OnDelete(DeleteBehavior.Cascade),
                   x => x.HasOne<Budget>()
                         .WithMany()
                         .HasForeignKey($"{nameof(Budget)}{nameof(Budget.Id)}")
                         .OnDelete(DeleteBehavior.Cascade)
               );
    }
}