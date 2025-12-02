using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleSquadsEntityTypeConfiguration : IEntityTypeConfiguration<BattleSquadsEntity>
{
    public void Configure(EntityTypeBuilder<BattleSquadsEntity> builder)
    {
        builder.ToTable("battle_squads");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Squads).IsRequired().HasConversion<StringCompressionConverter>();
        builder.Property(p => p.Side).IsRequired();
        
        builder.HasIndex(p => p.Side);
    }
}
