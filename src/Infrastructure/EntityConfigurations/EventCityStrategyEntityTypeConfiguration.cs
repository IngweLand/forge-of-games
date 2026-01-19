using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EventCityStrategyEntityTypeConfiguration : IEntityTypeConfiguration<EventCityStrategy>
{
    public void Configure(EntityTypeBuilder<EventCityStrategy> builder)
    {
        builder.ToTable("event_city_strategy");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CityId).IsRequired().HasConversion<string>();
        builder.Property(p => p.WonderId).IsRequired().HasConversion<string>();

        builder.HasIndex(p => p.HasPremiumBuildings);
        builder.HasIndex(p => p.HasPremiumExpansion);
        builder.HasIndex(p => p.PlayerId);
        builder.HasIndex(p => new {p.CityId, p.WonderId});
        
        builder.HasOne(x => x.Data).WithOne(x => x.EventCityStrategy)
            .HasForeignKey<EventCityStrategyDataEntity>(x => x.EventCityStrategyId).IsRequired();
        builder.HasOne(x => x.InGameEvent).WithMany().HasForeignKey(x => x.InGameEventId).IsRequired();
    }
}
