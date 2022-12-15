using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Financity.Persistence.Configuration;

public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(x => x.TransactionType)
               .HasConversion<string>()
               .HasMaxLength(32);

        builder.HasIndex(x => x.TransactionType);

        builder.Property(x => x.Note).HasMaxLength(512);

        builder.HasOne(x => x.Category)
               .WithMany(x => x.Transactions)
               .HasForeignKey(x => new { x.CategoryId, x.CategoryWalletId })
               .HasPrincipalKey(x => new { x.Id, x.WalletId })
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Recipient)
               .WithMany(x => x.Transactions)
               .HasForeignKey(x => new { x.RecipientId, x.RecipientWalletId })
               .HasPrincipalKey(x => new { x.Id, x.WalletId })
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Labels)
               .WithMany(x => x.Transactions)
               .UsingEntity<Dictionary<string, object>>(
                   x => x.HasOne<Label>()
                         .WithMany()
                         .HasForeignKey($"{nameof(Label)}{nameof(Label.Id)}",
                             $"{nameof(Label)}{nameof(Label.WalletId)}")
                         .HasPrincipalKey(y => new { y.Id, y.WalletId })
                         .OnDelete(DeleteBehavior.Cascade),
                   x => x.HasOne<Transaction>()
                         .WithMany()
                         .HasForeignKey($"{nameof(Transaction)}{nameof(Transaction.Id)}",
                             $"{nameof(Transaction)}{nameof(Transaction.WalletId)}")
                         .HasPrincipalKey(y => new { y.Id, y.WalletId })
                         .OnDelete(DeleteBehavior.Cascade),
                   t => t.ToTable(
                       $"{nameof(Transaction)}{nameof(Label)}",
                       y => y.HasCheckConstraint(
                           $"CH_{y.Name}_{nameof(Transaction)}{nameof(Transaction.WalletId)}_{nameof(Label)}{nameof(Label.WalletId)}",
                           $"\"{nameof(Transaction)}{nameof(Transaction.WalletId)}\" = \"{nameof(Label)}{nameof(Label.WalletId)}\"")
                   )
               );

        builder.ToTable(x =>
        {
            x.HasCheckConstraint($"CH_{x.Name}_{nameof(Transaction.WalletId)}_{nameof(Transaction.CategoryWalletId)}",
                $"\"{nameof(Transaction.CategoryWalletId)}\" is null or \"{nameof(Transaction.CategoryWalletId)}\" = \"{nameof(Transaction.WalletId)}\"");

            x.HasCheckConstraint($"CH_{x.Name}_{nameof(Transaction.WalletId)}_{nameof(Transaction.RecipientWalletId)}",
                $"\"{nameof(Transaction.RecipientWalletId)}\" is null or \"{nameof(Transaction.RecipientWalletId)}\" = \"{nameof(Transaction.WalletId)}\"");
        });
    }
}