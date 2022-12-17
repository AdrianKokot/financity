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
                         .HasForeignKey($"{nameof(Wallet)}{nameof(Wallet.Id)}")
                         .OnDelete(DeleteBehavior.Cascade),
                   x => x.HasOne<User>()
                         .WithMany()
                         .HasForeignKey($"{nameof(User)}{nameof(User.Id)}")
                         .OnDelete(DeleteBehavior.Cascade)
               );

        builder.Property(x => x.Name)
               .HasMaxLength(128);

        builder.HasIndex(x => x.NormalizedEmail).IsUnique();
    }
}