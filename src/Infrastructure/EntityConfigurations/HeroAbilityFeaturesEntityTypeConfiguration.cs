using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class HeroAbilityFeaturesEntityTypeConfiguration : IEntityTypeConfiguration<HeroAbilityFeaturesEntity>
{
    public void Configure(EntityTypeBuilder<HeroAbilityFeaturesEntity> builder)
    {
        builder.ToTable("hero_ability_features");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HeroId).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Locale).IsRequired();
        builder.Property(x => x.Tags).IsRequired()
            .HasConversion<JsonValueConverter<ISet<string>>>(new SetValueComparer<string>());
        builder.Property(x => x.Attributes).IsRequired()
            .HasConversion<JsonValueConverter<ISet<string>>>(new SetValueComparer<string>());
        
        builder.HasIndex(x => new {x.Locale, x.HeroId}).IsUnique()
            .IncludeProperties(x => new {x.Tags, x.Attributes});
    }
}
