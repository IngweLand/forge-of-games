using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PvpBattleEntityTypeConfiguration : IEntityTypeConfiguration<PvpBattle>
{
    public void Configure(EntityTypeBuilder<PvpBattle> builder)
    {
        builder.ToTable("pvp_battles", t =>
        {
            t.HasCheckConstraint("CK_pvp_battles_WinnerId_LoserId_Different", "[WinnerId] <> [LoserId]");
        });

        builder.HasKey(p => p.Id);
        
        builder.Ignore(p => p.Key);
        
        
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.PerformedAt).IsRequired();
        builder.Property(p => p.WinnerUnits).IsRequired();
        builder.Property(p => p.LoserUnits).IsRequired();
        
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGameBattleId);
        builder.HasIndex(p => p.PerformedAt).IsDescending();
        builder.HasIndex(p => new {p.WorldId, p.InGameBattleId}).IsUnique();
    }
}