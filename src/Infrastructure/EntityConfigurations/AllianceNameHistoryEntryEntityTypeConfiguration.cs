using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceNameHistoryEntryEntityTypeConfiguration : IEntityTypeConfiguration<AllianceNameHistoryEntry>
{
    public void Configure(EntityTypeBuilder<AllianceNameHistoryEntry> builder)
    {
        builder.ToTable("alliance_name_history_entries");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(500);
        builder.Property(p => p.ChangedAt).IsRequired();

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => new { p.AllianceId, p.Name, p.ChangedAt}).IsUnique();
    }
}