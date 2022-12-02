using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.OwnedWallets)
               .WithOne(x => x.Owner)
               .HasForeignKey(x => x.OwnerId);

        builder.HasMany(x => x.SharedWallets)
               .WithMany(x => x.UsersWithSharedAccess);
    }
}