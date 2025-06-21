using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class BattleSummaryEntityTypeConfiguration : IEntityTypeConfiguration<BattleSummaryEntity>
{
    public void Configure(EntityTypeBuilder<BattleSummaryEntity> builder)
    {
        builder.ToTable("battles");

        builder.HasKey(p => p.Id);

        builder.Ignore(p => p.Key);


        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.BattleDefinitionId).IsRequired();
        builder.Property(p => p.PlayerSquads).IsRequired();
        builder.Property(p => p.EnemySquads).IsRequired();
        builder.Property(p => p.InGameBattleId).IsRequired();
        builder.Property(p => p.ResultStatus).IsRequired();
        builder.Property(p => p.WorldId).IsRequired();

        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGameBattleId);
        builder.HasIndex(p => p.BattleDefinitionId);
        builder.HasIndex(p => p.ResultStatus);
        builder.HasIndex(p => p.Difficulty);
        builder.HasIndex(p => new {p.WorldId, p.InGameBattleId}).IsUnique();

        builder.HasMany(b => b.Units).WithMany(h => h.Battles).UsingEntity(j => j.ToTable("battles_to_units"));
    }
}
