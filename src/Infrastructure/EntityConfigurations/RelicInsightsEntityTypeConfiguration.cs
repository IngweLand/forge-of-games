using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class RelicInsightsEntityTypeConfiguration : IEntityTypeConfiguration<RelicInsightsEntity>
{
    public void Configure(EntityTypeBuilder<RelicInsightsEntity> builder)
    {
        builder.ToTable("relic_insights");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UnitId).IsRequired();
        builder.Property(p => p.FromLevel).IsRequired();
        builder.Property(p => p.ToLevel).IsRequired();
        builder.Property(p => p.Relics).IsRequired()
            .HasConversion<JsonValueConverter<ICollection<string>>>(new CollectionValueComparer<string>());

        builder.HasIndex(p => p.UnitId);
        builder.HasIndex(p => p.FromLevel);
        builder.HasIndex(p => p.ToLevel);
        builder.HasIndex(p => new {p.UnitId, p.FromLevel, p.ToLevel}).IsUnique();
    }
}
