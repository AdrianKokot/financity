using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public sealed class WalletAccessConfiguration : IEntityTypeConfiguration<WalletAccess>
{
    public void Configure(EntityTypeBuilder<WalletAccess> builder)
    {
        builder.HasKey(x => new {x.UserId, x.WalletId});
    }
}