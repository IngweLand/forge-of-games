using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerAllianceNameHistoryEntryEntityTypeConfiguration : IEntityTypeConfiguration<PlayerAllianceNameHistoryEntry>
{
    public void Configure(EntityTypeBuilder<PlayerAllianceNameHistoryEntry> builder)
    {
        builder.ToTable("player_alliance_name_history_entries");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.AllianceName).IsRequired().HasMaxLength(500);

        builder.HasIndex(p => p.AllianceName);
        // This index should technically be unique.
        // However, it does not work for us because the game allows names with trailing spaces
        // and SQL server ignores them when comparing strings.
        // One workaround would be to add non-break space character \u00A0 to all names.
        // However, it's more work and we are checking duplicates before inserting anyway.
        builder.HasIndex(p => new { p.PlayerId, p.AllianceName});
    }
}