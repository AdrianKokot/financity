using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financity.Persistence.Configuration;

public class EntryTypeConfiguration : IEntityTypeConfiguration<EntryType>
{
    public void Configure(EntityTypeBuilder<EntryType> builder)
    {
    }
}