using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class TopHeroInsightsEntityTypeConfiguration : IEntityTypeConfiguration<TopHeroInsightsEntity>
{
    public void Configure(EntityTypeBuilder<TopHeroInsightsEntity> builder)
    {
        builder.ToTable("top_hero_insights");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.Heroes).IsRequired()
            .HasConversion<JsonValueConverter<ISet<string>>>(new SetValueComparer<string>());
        builder.Property(p => p.Mode).IsRequired().HasConversion<string>();

        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.Mode);
        builder.HasIndex(p => p.AgeId);
        builder.HasIndex(p => p.FromLevel);
        builder.HasIndex(p => p.ToLevel);
    }
}
