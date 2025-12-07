using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerCitySnapshotEntityTypeConfiguration : IEntityTypeConfiguration<PlayerCitySnapshot>
{
    public void Configure(EntityTypeBuilder<PlayerCitySnapshot> builder)
    {
        builder.ToTable("player_city_snapshots");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CityId).IsRequired().HasConversion<string>();
        builder.Property(p => p.OpenedExpansionsHash).IsRequired().HasMaxLength(16).IsUnicode(false).IsFixedLength();
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.AgeId).IsRequired();
        builder.Property(p => p.HappinessUsageRatio).IsRequired();
        builder.Property(p => p.TotalArea).IsRequired();

        builder.HasIndex(p => p.OpenedExpansionsHash);
        builder.HasIndex(p => p.HasPremiumHomeBuildings);
        builder.HasIndex(p => p.HasPremiumFarmBuildings);
        builder.HasIndex(p => p.HasPremiumCultureBuildings);
        builder.HasIndex(p => p.TotalArea);
        builder.HasIndex(p => new {p.PlayerId, p.CityId, p.CollectedAt}).IsUnique();
        builder.HasIndex(p => new {p.CityId, p.AgeId});
        
        builder.HasIndex(p => p.Coins).IsDescending();
        builder.HasIndex(p => p.Coins1H).IsDescending();
        builder.HasIndex(p => p.Coins24H).IsDescending();
        builder.HasIndex(p => p.CoinsPerArea).IsDescending();
        builder.HasIndex(p => p.Coins1HPerArea).IsDescending();
        builder.HasIndex(p => p.Coins24HPerArea).IsDescending();

        builder.HasIndex(p => p.Food).IsDescending();
        builder.HasIndex(p => p.Food1H).IsDescending();
        builder.HasIndex(p => p.Food24H).IsDescending();
        builder.HasIndex(p => p.FoodPerArea).IsDescending();
        builder.HasIndex(p => p.Food1HPerArea).IsDescending();
        builder.HasIndex(p => p.Food24HPerArea).IsDescending();

        builder.HasIndex(p => p.Goods).IsDescending();
        builder.HasIndex(p => p.Goods1H).IsDescending();
        builder.HasIndex(p => p.Goods24H).IsDescending();
        builder.HasIndex(p => p.GoodsPerArea).IsDescending();
        builder.HasIndex(p => p.Goods1HPerArea).IsDescending();
        builder.HasIndex(p => p.Goods24HPerArea).IsDescending();

        builder.HasOne(x => x.Data).WithOne(x => x.PlayerCitySnapshot)
            .HasForeignKey<PlayerCitySnapshotDataEntity>(x => x.PlayerCitySnapshotId).IsRequired();
    }
}
