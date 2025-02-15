using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players");

        builder.HasKey(p => new {p.WorldId, p.InGamePlayerId});

        builder.Ignore(p => p.Key);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Age).IsRequired().HasMaxLength(255);
        builder.Property(p => p.AllianceName).HasMaxLength(500);
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGamePlayerId);
        builder.HasIndex(p => p.Age);
        builder.HasIndex(p => p.RankingPoints).IsDescending();

        builder.HasMany(p => p.Rankings).WithOne().HasForeignKey(p => new {p.WorldId, p.InGamePlayerId});
    }
}
