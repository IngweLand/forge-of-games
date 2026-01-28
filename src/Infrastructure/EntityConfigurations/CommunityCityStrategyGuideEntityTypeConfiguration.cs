using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class
    CommunityCityStrategyGuideEntityTypeConfiguration : IEntityTypeConfiguration<CommunityCityStrategyGuideEntity>
{
    public void Configure(EntityTypeBuilder<CommunityCityStrategyGuideEntity> builder)
    {
        builder.ToTable("community_city_strategy_guides");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Text).IsRequired();
    }
}
