using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players");

        builder.HasKey(p => p.Id);

        builder.Ignore(p => p.Key);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Age).IsRequired().HasMaxLength(255);
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.UpdatedAt).IsRequired();
        builder.Property(p => p.IsPresentInGame).HasDefaultValue(true);

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGamePlayerId);
        builder.HasIndex(p => p.Age);
        builder.HasIndex(p => p.IsPresentInGame);
        builder.HasIndex(p => p.RankingPoints).IsDescending();
        builder.HasIndex(p => new {p.WorldId, p.InGamePlayerId}).IsUnique();

        builder.HasMany(p => p.Rankings).WithOne().HasForeignKey(p => p.PlayerId);
        builder.HasMany(p => p.AgeHistory).WithOne().HasForeignKey(p => p.PlayerId);
        builder.HasMany(p => p.NameHistory).WithOne().HasForeignKey(p => p.PlayerId);
        builder.HasMany(p => p.PvpRankings).WithOne().HasForeignKey(p => p.PlayerId);
        builder.HasMany(p => p.PvpWins).WithOne(b => b.Winner).HasForeignKey(b => b.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(p => p.PvpLosses).WithOne(b => b.Loser).HasForeignKey(b => b.LoserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(p => p.CitySnapshots).WithOne(x => x.Player).HasForeignKey(p => p.PlayerId);
        builder.HasMany(p => p.Squads).WithOne(x => x.Player).HasForeignKey(p => p.PlayerId);
    }
}