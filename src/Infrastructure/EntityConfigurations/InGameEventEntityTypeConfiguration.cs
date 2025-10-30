using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class InGameEventEntityTypeConfiguration : IEntityTypeConfiguration<InGameEventEntity>
{
    public void Configure(EntityTypeBuilder<InGameEventEntity> builder)
    {
        builder.ToTable("in_game_events");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EventId).IsRequired();
        builder.Property(p => p.StartAt).IsRequired();
        builder.Property(p => p.EndAt).IsRequired();
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(20);
        builder.Property(p => p.DefinitionId).IsRequired().HasConversion<string>();

        builder.HasIndex(p => p.EventId);
        builder.HasIndex(p => p.DefinitionId);
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => new {p.WorldId, p.DefinitionId, p.EventId}).IsUnique();
    }
}
