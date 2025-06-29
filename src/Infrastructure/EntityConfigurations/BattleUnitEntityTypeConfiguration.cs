using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleUnitEntityTypeConfiguration : IEntityTypeConfiguration<BattleUnitEntity>
{
    public void Configure(EntityTypeBuilder<BattleUnitEntity> builder)
    {
        builder.ToTable("battle_units");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.UnitId).IsRequired();
        builder.Property(p => p.Level).IsRequired();
        builder.Property(p => p.Side).IsRequired();
        
        builder.HasIndex(p => p.UnitId);
        builder.HasIndex(p => p.Level);
        builder.HasIndex(p => p.Side);
        builder.HasIndex(p => new {HeroId = p.UnitId, p.Level, p.Side}).IsUnique();
    }
}
