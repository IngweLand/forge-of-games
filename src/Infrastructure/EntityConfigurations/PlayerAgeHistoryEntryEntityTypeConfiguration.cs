using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerAgeHistoryEntryEntityTypeConfiguration : IEntityTypeConfiguration<PlayerAgeHistoryEntry>
{
    public void Configure(EntityTypeBuilder<PlayerAgeHistoryEntry> builder)
    {
        builder.ToTable("player_age_history_entries");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Age).IsRequired().HasMaxLength(500);
        builder.Property(p => p.ChangedAt).IsRequired();

        builder.HasIndex(p => new { p.PlayerId, p.Age, p.ChangedAt}).IsUnique();
    }
}