using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.OwnedWallets)
               .WithOne(x => x.Owner)
               .HasForeignKey(x => x.OwnerId);

        builder.HasMany(x => x.SharedWallets)
               .WithMany(x => x.UsersWithSharedAccess);

        builder.HasMany(x => x.SharedWallets)
               .WithMany(x => x.UsersWithSharedAccess)
               .UsingEntity<Dictionary<string, object>>(
                      $"{nameof(User)}{nameof(Wallet)}",
                      x => x.HasOne<Wallet>()
                            .WithMany()
                            .HasForeignKey($"{nameof(Wallet)}{nameof(Wallet.Id)}",
                                   $"{nameof(Wallet)}{nameof(Wallet.OwnerId)}")
                            .HasPrincipalKey(y => new { y.Id, y.OwnerId })
                            .OnDelete(DeleteBehavior.Cascade),
                      x => x.HasOne<User>()
                            .WithMany()
                            .HasForeignKey($"{nameof(User)}{nameof(User.Id)}")
                            .OnDelete(DeleteBehavior.Cascade),
                      t => t.ToTable(
                             $"{nameof(User)}{nameof(Wallet)}",
                             y => y.HasCheckConstraint(
                                    $"CH_{y.Name}_{nameof(User)}{nameof(User.Id)}_{nameof(Wallet)}{nameof(Wallet.OwnerId)}",
                                    $"\"{nameof(User)}{nameof(User.Id)}\" != \"{nameof(Wallet)}{nameof(Wallet.OwnerId)}\"")
                      )
               );

        builder.Property(x => x.Name)
               .HasMaxLength(128);

        builder.HasIndex(x => x.NormalizedEmail).IsUnique();
    }
}