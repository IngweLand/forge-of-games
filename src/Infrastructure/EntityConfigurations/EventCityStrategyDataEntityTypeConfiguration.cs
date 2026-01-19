using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EventCityStrategyDataEntityTypeConfiguration : IEntityTypeConfiguration<EventCityStrategyDataEntity>
{
    public void Configure(EntityTypeBuilder<EventCityStrategyDataEntity> builder)
    {
        builder.ToTable("event_city_strategy_data");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Data).IsRequired()
            .HasConversion<BytearrayCompressionConverter>(new ByteArrayValueComparer());
    }
}
