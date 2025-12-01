using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceAthRankingEntityTypeConfiguration : IEntityTypeConfiguration<AllianceAthRanking>
{
    public void Configure(EntityTypeBuilder<AllianceAthRanking> builder)
    {
        builder.ToTable("alliance_ath_rankings");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.League).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.InGameEventId).IsRequired();

        builder.HasIndex(p => p.Points).IsDescending();
        builder.HasIndex(p => p.League);
        builder.HasIndex(p => p.InGameEventId);
        builder.HasIndex(p => new {p.AllianceId, p.InGameEventId}).IsUnique();
    }
}
