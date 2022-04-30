using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public class EntryCategoryConfiguration : IEntityTypeConfiguration<EntryCategory>
{
    public void Configure(EntityTypeBuilder<EntryCategory> builder)
    {
    }
}