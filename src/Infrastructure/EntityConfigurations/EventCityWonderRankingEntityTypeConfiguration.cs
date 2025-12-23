using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EventCityWonderRankingEntityTypeConfiguration : IEntityTypeConfiguration<EventCityWonderRanking>
{
    public void Configure(EntityTypeBuilder<EventCityWonderRanking> builder)
    {
        builder.ToTable("event_city_wonder_rankings");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.WonderLevel).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.InGameEventId).IsRequired();

        builder.HasIndex(p => p.InGameEventId);
        builder.HasIndex(p => new {p.PlayerId, p.InGameEventId}).IsUnique();
    }
}
