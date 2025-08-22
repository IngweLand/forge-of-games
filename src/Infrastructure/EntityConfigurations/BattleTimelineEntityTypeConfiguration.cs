using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleTimelineEntityTypeConfiguration : IEntityTypeConfiguration<BattleTimelineEntity>
{
    public void Configure(EntityTypeBuilder<BattleTimelineEntity> builder)
    {
        builder.ToTable("battle_timelines");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.InGameBattleId).IsRequired();
        builder.Property(p => p.Entries).IsRequired()
            .HasConversion<JsonValueConverter<ISet<BattleTimelineEntry>>>(new SetValueComparer<BattleTimelineEntry>());

        builder.HasIndex(p => p.InGameBattleId);
    }
}
