using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EventCitySnapshotEntityTypeConfiguration : IEntityTypeConfiguration<EventCitySnapshot>
{
    public void Configure(EntityTypeBuilder<EventCitySnapshot> builder)
    {
        builder.ToTable("event_city_snapshots");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CityId).IsRequired().HasConversion<string>();
        builder.Property(p => p.WonderId).IsRequired().HasConversion<string>();
        builder.Property(p => p.CollectedAt).IsRequired();

        builder.HasIndex(p => p.HasPremiumBuildings);
        builder.HasIndex(p => new {p.PlayerId, p.CityId, p.WonderId});
        
        builder.HasOne(x => x.Data).WithOne(x => x.EventCitySnapshot)
            .HasForeignKey<EventCitySnapshotDataEntity>(x => x.EventCitySnapshotId).IsRequired();
    }
}
