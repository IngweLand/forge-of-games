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

        builder.Ignore(p => p.Data);

        builder.Property(p => p.CityId).IsRequired().HasConversion<string>();
        builder.Property(p => p.OpenedExpansionsHash).IsRequired().HasMaxLength(16).IsUnicode(false).IsFixedLength();
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.CompressedData).IsRequired();
        builder.Property(p => p.AgeId).IsRequired();
        builder.Property(p => p.HappinessUsageRatio).IsRequired();
        builder.Property(p => p.TotalArea).IsRequired();

        builder.HasIndex(p => p.CityId);
        builder.HasIndex(p => p.OpenedExpansionsHash);
        builder.HasIndex(p => p.HasPremiumBuildings);
        builder.HasIndex(p => p.AgeId);
        builder.HasIndex(p => p.Coins).IsDescending();
        builder.HasIndex(p => p.Food).IsDescending();
        builder.HasIndex(p => p.Goods).IsDescending();
        builder.HasIndex(p => p.TotalArea);
        builder.HasIndex(p => new {p.PlayerId, p.CityId, p.CollectedAt}).IsUnique();
    }
}
