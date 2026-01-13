using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EventCityFetchStateEntityTypeConfiguration : IEntityTypeConfiguration<EventCityFetchState>
{
    public void Configure(EntityTypeBuilder<EventCityFetchState> builder)
    {
        builder.ToTable("event_city_fetch_states");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EventId).IsRequired();
        builder.Property(p => p.PlayerId).IsRequired();
        builder.Property(p => p.InGamePlayerId).IsRequired();
        builder.Property(p => p.GameWorldId).IsRequired();
        builder.Property(p => p.FetchTimestamps).IsRequired()
            .HasConversion<JsonValueConverter<ICollection<DateTime>>>(new CollectionValueComparer<DateTime>());

        builder.HasIndex(p => p.EventId);
        builder.HasIndex(p => new {p.EventId, p.PlayerId}).IsUnique();
    }
}
